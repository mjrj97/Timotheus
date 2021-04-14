using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

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
        private string url;
        /// <summary>
        /// A list of the events in the local calendar.
        /// </summary>
        public readonly List<Event> events = new List<Event>();
        /// <summary>
        /// HTTP Client used to make HTTP requests.
        /// </summary>
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Calendar timezone.
        /// </summary>
        private string timezone;
        /// <summary>
        /// iCalendar version.
        /// </summary>
        private string version;
        /// <summary>
        /// Identifier for the product that created the iCalendar object.
        /// </summary>
        private string prodid;

        /// <summary>
        /// Creates a Calendar object and pulls calendar data from URL using specified credentials.
        /// </summary>
        public Calendar(string username, string password, string url)
        {
            SetupSync(username, password, url);
            events = LoadFromLines(HttpRequest(url, credentials));
        }
        /// <summary>
        /// Creates a Calendar object and loads event data from .ics file.
        /// </summary>
        public Calendar(string[] lines)
        {
            events = LoadFromLines(lines);
        }
        /// <summary>
        /// Creates an empty Calendar object.
        /// </summary>
        public Calendar()
        {
            url = string.Empty;
            timezone = GenerateTimeZone();
            version = "2.0";
            prodid = "-//mjrj97//Timotheus//EN";
        }

        /// <summary>
        /// Add an event on the remote calendar.
        /// </summary>
        /// <param name="ev">Adds the event to the remote calendar. Assumes that the UID is one the remote calendar.</param>
        private void AddEvent(Event ev)
        {
            string request =
            "BEGIN:VCALENDAR\n" +
            "VERSION:" + version + "\n" +
            "PRODID:" + prodid + "\n" +
            timezone + "\n" +
            GetEventICS(ev) + "\n" +
            "END:VCALENDAR";

            HttpRequest(url + ev.UID + ".ics", credentials, "PUT", System.Text.Encoding.UTF8.GetBytes(request));
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
        /// <returns>
        /// A list Events found in string array.
        /// </returns>
        /// <param name="lines">String array in iCal format.</param>
        public List<Event> LoadFromLines(string[] lines)
        {
            string tempTimeZone = string.Empty;
            string tempVersion = string.Empty;
            string tempProdID = string.Empty;

            DateTime tempStartTime = DateTime.Now;
            DateTime tempEndTime = DateTime.Now;
            DateTime tempCreated = DateTime.Now;
            string tempName = string.Empty;
            string tempLocation = string.Empty;
            string tempDescription = string.Empty;
            string tempUID = string.Empty;

            int timeZoneStart = 0;
            int timeZoneEnd = 0;

            List<Event> events = new List<Event>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (timeZoneStart == 0)
                {
                    //Read calendar version and ID
                    if (lines[i].Contains("VERSION") && tempVersion.Length < 1)
                        tempVersion = GetValue(lines[i]);
                    if (lines[i].Contains("PRODID") && tempProdID.Length < 1)
                        tempProdID = GetValue(lines[i]);

                    //Set up time zone
                    if (lines[i].Contains("BEGIN:VTIMEZONE"))
                    {
                        tempTimeZone += lines[i] + "\n";
                        timeZoneStart = i;
                    }
                }
                else if (timeZoneEnd == 0)
                {
                    if (lines[i].Contains("END:VTIMEZONE"))
                    {
                        timeZoneEnd = i;
                        tempTimeZone += "END:VTIMEZONE";
                    }
                    else if (timeZoneStart != 0 && timeZoneEnd == 0)
                        tempTimeZone += lines[i] + "\n";
                }
                else
                {
                    //Read events
                    if (lines[i].Contains("SUMMARY"))
                        tempName = GetValue(lines[i]);
                    if (lines[i].Contains("DESCRIPTION"))
                        tempDescription = ConvertFromCALString(GetValue(lines[i]));
                    if (lines[i].Contains("LOCATION"))
                        tempLocation = ConvertFromCALString(GetValue(lines[i]));
                    if (lines[i].Contains("UID"))
                        tempUID = GetValue(lines[i]);
                    if (lines[i].Contains("DTSTART"))
                        tempStartTime = StringToDate(GetValue(lines[i]));
                    if (lines[i].Contains("DTEND"))
                        tempEndTime = StringToDate(GetValue(lines[i]));
                    if (lines[i].Contains("DTSTAMP"))
                        tempCreated = StringToDate(GetValue(lines[i]));
                    if (lines[i].Contains("END:VEVENT"))
                    {
                        events.Add(new Event(tempStartTime, tempEndTime, tempCreated, tempName, tempDescription, tempLocation, tempUID));

                        tempName = string.Empty;
                        tempDescription = string.Empty;
                        tempLocation = string.Empty;
                        tempUID = string.Empty;
                    }
                }
            }

            if (tempTimeZone == string.Empty)
                timezone = GenerateTimeZone();
            else
                timezone = tempTimeZone;
            version = tempVersion;
            prodid = tempProdID;

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
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(username + ":" + password)));
            client.DefaultRequestHeaders.Add("Accept-charset", "UTF-8");
        }

        /// <summary>
        /// Syncs the events in the time interval from a to b with the remote calendar. (As long as either the start time or end time is in the interval)
        /// </summary>
        public void Sync(DateTime a, DateTime b)
        {
            List<Event> remoteEvents = LoadFromLines(HttpRequest(url, credentials));
            bool[] foundLocal = new bool[events.Count];
            bool[] foundRemote = new bool[remoteEvents.Count];

            for (int i = 0; i < events.Count; i++)
            {
                for (int j = 0; j < remoteEvents.Count; j++)
                {
                    if (events[i].UID == remoteEvents[j].UID)
                    {
                        foundLocal[i] = true;
                        foundRemote[j] = true;

                        if (events[i].IsInPeriod(a,b))
                        {
                            if (events[i].Deleted)
                                DeleteEvent(events[i]);
                            else if (!events[i].Equals(remoteEvents[j]))
                            {
                                if (events[i].Changed >= remoteEvents[j].Changed)
                                {
                                    DeleteEvent(events[i]);
                                    AddEvent(events[i]);
                                }
                                else
                                    events[i].Update(remoteEvents[j]);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].IsInPeriod(a, b))
                {
                    if (!foundLocal[i])
                        AddEvent(events[i]);
                }
            }
            for (int i = 0; i < remoteEvents.Count; i++)
            {
                if (remoteEvents[i].IsInPeriod(a, b))
                {
                    if (!foundRemote[i])
                        events.Add(remoteEvents[i]);
                }
            }
        }
        /// <summary>
        /// Syncs the entire local calendar with the entire remote calendar server.
        /// </summary>
        public void Sync()
        {
            Sync(DateTime.MinValue, DateTime.MaxValue);
        }

        /// <summary>
        /// Returns a calendars iCal equivalent string.
        /// </summary>
        /// <param name="name">Name of the calendar.</param>
        public string GetCalendarICS(string name)
        {
            string ics = "BEGIN:VCALENDAR\nVERSION:" + version +
            "\nMETHOD:PUBLISH\nPRODID:" + prodid +
            "\nX-WR-CALNAME:" + name + "\n"
            + timezone + "\n";
            for (int i = 0; i < events.Count; i++)
            {
                if (!events[i].Deleted)
                    ics += GetEventICS(events[i]) + "\n";
            }
            ics += "END:VCALENDAR";
            return ics;
        }

        /// <summary>
        /// Converts a Event into a iCal string.
        /// </summary>
        /// <param name="ev">Calendar event that should be converted.</param>
        public static string GetEventICS(Event ev)
        {
            string evString = "BEGIN:VEVENT\n" +
            "UID:" + ev.UID + "\n";
            if (ev.StartTime.Hour == ev.EndTime.Hour && ev.StartTime.Minute == ev.EndTime.Minute && ev.StartTime.Second == ev.EndTime.Second && ev.StartTime.Hour == 0 && ev.StartTime.Minute == 0 && ev.StartTime.Second == 0)
            {
                evString += "DTSTART;TZID=Europe/Copenhagen:" + DateToString(ev.StartTime) + "\n" +
                "DTEND;TZID=Europe/Copenhagen:" + DateToString(ev.EndTime) + "\n";
            }
            else
            {
                evString += "DTSTART;TZID=Europe/Copenhagen:" + DateTimeToString(ev.StartTime) + "\n" +
                "DTEND;TZID=Europe/Copenhagen:" + DateTimeToString(ev.EndTime) + "\n";
            }
            if (ev.Description != string.Empty)
                evString += "DESCRIPTION:" + ConvertToCALString(ev.Description) + "\n";
            evString += "DTSTAMP:" + DateTimeToString(ev.Created) + "Z\n";
            if (ev.Location != string.Empty)
                evString += "LOCATION:" + ConvertToCALString(ev.Location) + "\n";
            evString += "SUMMARY:" + ev.Name + "\nEND:VEVENT";

            return evString;
        }

        /// <summary>
        /// Returns a iCal timezone based in Europe/Copenhagen.
        /// </summary>
        public string GenerateTimeZone()
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
        /// Checks if the calendar is connected a remote server (Has URL and credentials).
        /// </summary>
        public bool IsSetup()
        {
            return url != string.Empty && credentials != null;
        }

        /// <summary>
        /// Sends a HTTP request to a URL and returns the response as a string array.
        /// </summary>
        public static string[] HttpRequest(string url, NetworkCredential credentials, string method, byte[] data)
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
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            string[] responseFromServer = reader.ReadToEnd().Replace("\r\n ", "").Split("\n");
            response.Close();
            return responseFromServer;
        }
        /// <summary>
        /// Sends a HTTP request to a URL and returns the response as a string array.
        /// </summary>
        public static string[] HttpRequest(string url, NetworkCredential credentials, string method)
        {
            return HttpRequest(url, credentials, method, null);
        }
        /// <summary>
        /// Sends a HTTP request to a URL and returns the response as a string array.
        /// </summary>
        public static string[] HttpRequest(string url, NetworkCredential credentials)
        {
            return HttpRequest(url, credentials, null, null);
        }

        /// <summary>
        /// Returns text after the last colon in a string.
        /// </summary>
        /// <param name="line">Line from iCalendar with format PROPERTY:VALUE eg. VERSION:2.0</param>
        private string GetValue(string line)
        {
            int i = 0;
            while (i < line.Length && line[i] != ':')
            {
                i++;
            }
            return line.Substring(i + 1, line.Length - i - 1).Trim();
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