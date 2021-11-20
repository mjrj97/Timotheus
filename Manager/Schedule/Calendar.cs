using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Timotheus.IO;
using Timotheus.Utility;

namespace Timotheus.Schedule
{
    /// <summary>
    /// Calendar object that contains a list of events, and is used to add/remove events on a remote calendar using iCal and CalDAV.
    /// </summary>
    public class Calendar
    {
        /// <summary>
        /// Username and password used to gain access to remote calendar.
        /// </summary>
        private NetworkCredential credentials;
        /// <summary>
        /// This is the CalDAV url/link to the remote calendar.
        /// </summary>
        private string url = string.Empty;

        /// <summary>
        /// A list of the calendars headers.
        /// </summary>
        private readonly Register headers = new(':');
        /// <summary>
        /// Calendar timezone.
        /// </summary>
        private string timezone = string.Empty;
        /// <summary>
        /// A list of the events in the local calendar.
        /// </summary>
        public readonly List<Event> Events;

        /// <summary>
        /// Creates a Calendar object and pulls calendar data from URL using specified credentials.
        /// </summary>
        public Calendar(string username, string password, string url)
        {
            SetupSync(username, password, url);
            Events = LoadFromLines(HttpRequest(url, credentials));
        }
        /// <summary>
        /// Creates a Calendar object and loads event data from .ics file.
        /// </summary>
        public Calendar(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Events = LoadFromLines(lines);
        }
        /// <summary>
        /// Creates an empty Calendar object.
        /// </summary>
        public Calendar()
        {
            url = string.Empty;
            Events = new List<Event>();
            timezone = GenerateTimeZone();

            headers.Add("VERSION", "2.0");
            headers.Add("PRODID", "-//mjrj97//Timotheus//EN");
            headers.Add("X-WR-CALNAME", "Calendar");
        }

        /// <summary>
        /// Add an event on the remote calendar.
        /// </summary>
        /// <param name="ev">Adds the event to the remote calendar. Assumes that the UID is one the remote calendar.</param>
        private void AddEvent(Event ev)
        {
            string request =
            "BEGIN:VCALENDAR\n" +
            "VERSION:" + headers.Get("VERSION") + "\n" +
            "PRODID:" + headers.Get("PRODID") + "\n" +
            timezone + "\n" +
            ev.ToString() + "\n" +
            "END:VCALENDAR";

            HttpRequest(url + ev.UID + ".ics", credentials, "PUT", Encoding.UTF8.GetBytes(request));
        }

        /// <summary>
        /// Removes an event on the remote calendar.
        /// </summary>
        /// <param name="ev">Event that should be deleted. Assumes that the UID is one the remote calendar.</param>
        private void DeleteEvent(Event ev)
        {
            HttpRequest(url + ev.UID + ".ics", credentials, "DELETE");
        }

        /// <summary>
        /// Loads events from a calendar ics string.
        /// </summary>
        /// <param name="lines">String array in iCal format.</param>
        /// <returns> A list of events found in string array.</returns>
        private List<Event> LoadFromLines(string[] lines)
        {
            string tempTimeZone = string.Empty;
            string eventText = string.Empty;
            int timeZoneStart = 0;
            int timeZoneEnd = 0;

            List<Event> events = new();

            if (lines[0] != "BEGIN:VCALENDAR")
                throw new Exception("Exception_InvalidCalendar");
            for (int i = 1; i < lines.Length; i++)
            {
                if (timeZoneStart == 0)
                {
                    //Set up time zone
                    if (lines[i] == "BEGIN:VTIMEZONE")
                    {
                        tempTimeZone += lines[i] + "\n";
                        timeZoneStart = i;
                    }
                    else if (lines[i].Trim() != string.Empty)
                        headers.Add(lines[i]);
                }
                else if (timeZoneEnd == 0)
                {
                    if (lines[i] == "END:VTIMEZONE")
                    {
                        timeZoneEnd = i;
                        tempTimeZone += "END:VTIMEZONE";
                    }
                    else if (timeZoneStart != 0 && timeZoneEnd == 0)
                        tempTimeZone += lines[i] + "\n";
                }
                else
                {
                    if (lines[i] == "BEGIN:VEVENT")
                    {
                        eventText = string.Empty;
                    }
                    eventText += lines[i] + "\n";
                    if (lines[i] == "END:VEVENT")
                    {
                        events.Add(new Event(eventText));
                    }
                }
            }

            if (tempTimeZone == string.Empty)
                timezone = GenerateTimeZone();
            else
                timezone = tempTimeZone;

            return events;
        }

        /// <summary>
        /// Sets up the calendar for syncing with a remote calendar.
        /// </summary>
        /// <param name="username">Usually an email.</param>
        /// <param name="password">Password to the email.</param>
        /// <param name="url">CalDAV link to the remote calendar.</param>
        public void SetupSync(string username, string password, string url)
        {
            this.url = url;
            credentials = new NetworkCredential(username, password);
        }
        /// <summary>
        /// Checks if the calendar is connected a remote server (Has URL and credentials).
        /// </summary>
        public bool IsSetup()
        {
            return url != string.Empty && credentials != null;
        }

        /// <summary>
        /// Saves the calendar to the path in an iCal format (.ics)
        /// </summary>
        public void Save(string path)
        {
            byte[] data = Encoding.UTF8.GetBytes(ToString());
            File.WriteAllBytes(path, data);
        }

        /// <summary>
        /// Syncs the events in the time interval from a to b with the remote calendar. (As long as either the start time or end time is in the interval)
        /// </summary>
        public void Sync(Period period)
        {
            List<Event> remoteEvents = LoadFromLines(HttpRequest(url, credentials));
            bool[] foundLocal = new bool[Events.Count];
            bool[] foundRemote = new bool[remoteEvents.Count];

            for (int i = 0; i < Events.Count; i++)
            {
                for (int j = 0; j < remoteEvents.Count; j++)
                {
                    if (Events[i].UID == remoteEvents[j].UID)
                    {
                        foundLocal[i] = true;
                        foundRemote[j] = true;

                        if (Events[i].In(period))
                        {
                            if (Events[i].Deleted)
                            {
                                DeleteEvent(Events[i]);
                                Events.Remove(Events[i]);
                            }
                            else if (!Events[i].Equals(remoteEvents[j]))
                            {
                                if (Events[i].Changed >= remoteEvents[j].Changed)
                                {
                                    DeleteEvent(Events[i]);
                                    AddEvent(Events[i]);
                                }
                                else
                                    Events[i].Update(remoteEvents[j]);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < Events.Count; i++)
            {
                if (Events[i].In(period))
                {
                    if (!foundLocal[i])
                        AddEvent(Events[i]);
                }
            }
            for (int i = 0; i < remoteEvents.Count; i++)
            {
                if (remoteEvents[i].In(period))
                {
                    if (!foundRemote[i])
                        Events.Add(remoteEvents[i]);
                }
            }
        }
        /// <summary>
        /// Syncs the entire local calendar with the entire remote calendar server.
        /// </summary>
        public void Sync()
        {
            Sync(new Period(DateTime.MinValue, DateTime.MaxValue));
        }

        /// <summary>
        /// Returns a iCal timezone based in Europe/Copenhagen.
        /// </summary>
        private static string GenerateTimeZone()
        {
            return "BEGIN:VTIMEZONE\n" +
            "TZID:Europe/Copenhagen\n" +
            "X-LIC-LOCATION:Europe/Copenhagen\n" +
            "BEGIN:STANDARD\n" +
            "DTSTART:18900101T000000\n" +
            "RDATE:18900101T000000\n" +
            "TZNAME:CMT\n" +
            "TZOFFSETFROM:+005020\n" +
            "TZOFFSETTO:+005020\n" +
            "END:STANDARD\n" +
            "BEGIN:STANDARD\n" +
            "DTSTART:18940101T000000\n" +
            "RDATE:18940101T000000\n" +
            "TZNAME:CEST\n" +
            "TZOFFSETFROM:+005020\n" +
            "TZOFFSETTO:+0100\n" +
            "END:STANDARD\n" +
            "BEGIN:DAYLIGHT\n" +
            "DTSTART:19160514T230000\n" +
            "RDATE:19160514T230000\n" +
            "RDATE:19400515T000000\n" +
            "RDATE:19430329T020000\n" +
            "RDATE:19440403T020000\n" +
            "RDATE:19450402T020000\n" +
            "RDATE:19460501T020000\n" +
            "RDATE:19470504T020000\n" +
            "RDATE:19480509T020000\n" +
            "RDATE:19800406T020000\n" +
            "TZNAME:CEST\n" +
            "TZOFFSETFROM:+0100\n" +
            "TZOFFSETTO:+0200\n" +
            "END:DAYLIGHT\n" +
            "BEGIN:STANDARD\n" +
            "DTSTART:19160930T230000\n" +
            "RDATE:19160930T230000\n" +
            "RDATE:19421102T030000\n" +
            "RDATE:19431004T030000\n" +
            "RDATE:19441002T030000\n" +
            "RDATE:19450815T030000\n" +
            "RDATE:19460901T030000\n" +
            "RDATE:19470810T030000\n" +
            "RDATE:19480808T030000\n" +
            "TZNAME:CET\n" +
            "TZOFFSETFROM:+0200\n" +
            "TZOFFSETTO:+0100\n" +
            "END:STANDARD\n" +
            "BEGIN:STANDARD\n" +
            "DTSTART:19800101T000000\n" +
            "RDATE:19800101T000000\n" +
            "TZNAME:CEST\n" +
            "TZOFFSETFROM:+0100\n" +
            "TZOFFSETTO:+0100\n" +
            "END:STANDARD\n" +
            "BEGIN:STANDARD\n" +
            "DTSTART:19800928T030000\n" +
            "RRULE:FREQ=YEARLY;UNTIL=19950924T010000Z;BYDAY=-1SU;BYMONTH=9\n" +
            "TZNAME:CET\n" +
            "TZOFFSETFROM:+0200\n" +
            "TZOFFSETTO:+0100\n" +
            "END:STANDARD\n" +
            "BEGIN:DAYLIGHT\n" +
            "DTSTART:19810329T020000\n" +
            "RRULE:FREQ=YEARLY;BYDAY=-1SU;BYMONTH=3\n" +
            "TZNAME:CEST\n" +
            "TZOFFSETFROM:+0100\n" +
            "TZOFFSETTO:+0200\n" +
            "END:DAYLIGHT\n" +
            "BEGIN:STANDARD\n" +
            "DTSTART:19961027T030000\n" +
            "RRULE:FREQ=YEARLY;BYDAY=-1SU;BYMONTH=10\n" +
            "TZNAME:CET\n" +
            "TZOFFSETFROM:+0200\n" +
            "TZOFFSETTO:+0100\n" +
            "END:STANDARD\n" +
            "END:VTIMEZONE";
        }
        
        /// <summary>
        /// Exports the Calendar as a PDF.
        /// </summary>
        /// <param name="name">Name of the PDF</param>
        /// <param name="path">Path where to save the PDF</param>
        /// <param name="orgName">Name of the organisation</param>
        /// <param name="orgAddress">Address of the organisation</param>
        /// <param name="orgImagePath">Path to the organisations image</param>
        /// <param name="period">The period of events that should be included</param>
        public void Export(string name, string path, string orgName, string orgAddress, string orgImagePath, Period period)
        {
            List<Event> exportEvents = new List<Event>();
            for (int i = 0; i < Events.Count; i++)
            {
                if (Events[i].In(period))
                    exportEvents.Add(Events[i]);
            }
            PDF.ExportCalendar(exportEvents, path, name, orgName, orgAddress, orgImagePath, period.ToString());
        }

        /// <summary>
        /// Returns a calendars iCal equivalent string.
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new("BEGIN:VCALENDAR\n");
            builder.Append(headers.ToString());
            builder.Append('\n');
            builder.Append(timezone);
            builder.Append('\n');
            for (int i = 0; i < Events.Count; i++)
            {
                if (!Events[i].Deleted)
                {
                    builder.Append(Events[i].ToString());
                    builder.Append('\n');
                }
            }
            builder.Append("END:VCALENDAR");
            return builder.ToString();
        }

        /// <summary>
        /// Sends a HTTP request to a URL and returns the response as a string array.
        /// </summary>
        public static string[] HttpRequest(string url, NetworkCredential credentials, string method = null, byte[] data = null)
        {
            WebRequest request = WebRequest.Create(url);
            request.Credentials = credentials;

            if (method != null)
                request.Method = method;
            if (method == "PUT")
            {
                request.Headers.Add("Accept-charset", "UTF-8");
                request.ContentType = "text/calendar";
                request.ContentLength = data.Length;

                using var stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
            }

            WebResponse response = request.GetResponse();
            StreamReader reader = new(response.GetResponseStream());
            string text = reader.ReadToEnd().Replace("\r\n ", "");
            string[] responseFromServer = Regex.Split(text, "\r\n|\r|\n");
            response.Close();
            return responseFromServer;
        }

        /// <summary>
        /// Converts a DateTime object to a iCal date and time.
        /// </summary>
        public static string DateTimeToString(DateTime date)
        {
            return date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + "T" + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
        }

        /// <summary>
        /// Converts a DateTime object to a iCal date.
        /// </summary>
        public static string DateToString(DateTime date)
        {
            return date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2");
        }

        /// <summary>
        /// Converts a iCal date and time to a DateTime object.
        /// </summary>
        public static DateTime StringToDate(string date)
        {
            if (date.Length == 8)
                return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)), 0, 0, 0);
            else
                return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)), int.Parse(date.Substring(9, 2)), int.Parse(date.Substring(11, 2)), int.Parse(date.Substring(13, 2)));
        }

        /// <summary>
        /// Converts a line from iCal to C# compatible string.
        /// </summary>
        public static string ConvertFromCALString(string text)
        {
            return text.Replace("\\n", "\r\n").Replace("\\,", ",");
        }

        /// <summary>
        /// Converts a string to a iCal compatible string.
        /// </summary>
        public static string ConvertToCALString(string text)
        {
            return text.Replace("\n", "\\n").Replace("\r", "").Replace(",", "\\,");
        }
    }
}