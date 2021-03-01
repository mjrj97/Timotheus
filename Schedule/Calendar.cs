using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Timotheus.Schedule
{
    public class Calendar
    {
        private NetworkCredential credentials;
        private string url;
        public readonly List<Event> events = new List<Event>();
        private readonly HttpClient client = new HttpClient();

        private string timezone;
        private string version;
        private string prodid;

        //Constructors
        public Calendar(string username, string password, string url)
        {
            SetupSync(username, password, url);
            events = LoadFromLines(HttpRequest(url, credentials));
        }
        public Calendar(string path)
        {
            events = LoadFromLines(File.ReadAllLines(path));
        }
        public Calendar()
        {
            url = string.Empty;
            timezone = GenerateTimeZone();
            version = "2.0";
            prodid = "Calendar";
        }

        //Add an event on the remote calendar
        public void AddEvent(Event ev)
        {
            string request =
            "BEGIN:VCALENDAR\n" +
            "VERSION:" + version + "\n" +
            "PRODID:" + prodid + "\n" +
            timezone + "\n" +
            GetEventICS(ev) + "\n" +
            "END:VCALENDAR";

            HttpRequest(url + ev.UID + ".ics", credentials, "PUT", Encoding.UTF8.GetBytes(request));
        }

        //Removes an event on the remote calendar
        public void DeleteEvent(string ID)
        {
            HttpRequest(url + ID + ".ics", credentials, "DELETE");
        }

        //Loads events from a calendar ics string
        public List<Event> LoadFromLines(string[] lines)
        {
            string tempTimeZone = String.Empty;
            string tempVersion = String.Empty;
            string tempProdID = String.Empty;

            DateTime tempStartTime = DateTime.Now;
            DateTime tempEndTime = DateTime.Now;
            DateTime tempCreated = DateTime.Now;
            string tempName = String.Empty;
            string tempLocation = String.Empty;
            string tempDescription = String.Empty;
            string tempUID = String.Empty;

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
                        
                        tempName = String.Empty;
                        tempDescription = String.Empty;
                        tempLocation = String.Empty;
                        tempUID = String.Empty;
                    }
                }
            }

            if (tempTimeZone == String.Empty)
                timezone = GenerateTimeZone();
            else
                timezone = tempTimeZone;
            version = tempVersion;
            prodid = tempProdID;

            return events;
        }

        //Sets up the calendar for syncing with a remote calendar
        public void SetupSync(string username, string password, string url)
        {
            this.url = url;
            credentials = new NetworkCredential(username, password);
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));
            client.DefaultRequestHeaders.Add("Accept-charset", "UTF-8");
        }

        //Syncs the calendar with a remote calendar server
        public void Sync()
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

                        if (events[i].Deleted)
                            DeleteEvent(events[i].UID);
                        else if (!events[i].Equals(remoteEvents[j]))
                        {
                            if (events[i].Changed >= remoteEvents[j].Changed)
                            {
                                DeleteEvent(events[i].UID);
                                AddEvent(events[i]);
                            }
                            else
                                events[i].Update(remoteEvents[j]);
                        }
                    }
                }
            }
            for (int i = 0; i < events.Count; i++)
            {
                if (!foundLocal[i])
                    AddEvent(events[i]);
            }
            for (int i = 0; i < remoteEvents.Count; i++)
            {
                if (!foundRemote[i])
                    events.Add(remoteEvents[i]);
            }
        }

        //Returns a calendars iCal equivalent string
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

        //Returns a events iCal equivalent string
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
            if (ev.Description != String.Empty)
                evString += "DESCRIPTION:" + ConvertToCALString(ev.Description) + "\n";
            evString += "DTSTAMP:" + DateTimeToString(ev.Created) + "Z\n";
            if (ev.Location != String.Empty)
                evString += "LOCATION:" + ConvertToCALString(ev.Location) + "\n";
            evString += "SUMMARY:" + ev.Name + "\nEND:VEVENT";

            return evString;
        }

        //Returns a iCal timezone
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

        //Checks if the calendar has a URL
        public bool IsSetup()
        {
            return url != string.Empty;
        }

        //Sends a HTTP request to a URL
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
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string[] responseFromServer = reader.ReadToEnd().Split("\n");
            response.Close();
            return responseFromServer;
        }
        public static string[] HttpRequest(string url, NetworkCredential credentials, string method)
        {
            return HttpRequest(url, credentials, method, null);
        }
        public static string[] HttpRequest(string url, NetworkCredential credentials)
        {
            return HttpRequest(url, credentials, null, null);
        }

        //Returns text after the last colon in a string
        private string GetValue(string line)
        {
            int i = 0;
            while (i < line.Length && line[i] != ':')
            {
                i++;
            }
            return line.Substring(i + 1, line.Length - i - 1).Trim();
        }

        //Converts a DateTime object to a iCal date and time
        public static string DateTimeToString(DateTime date)
        {
            return date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + "T" + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
        }

        //Converts a DateTime object to a iCal date
        public static string DateToString(DateTime date)
        {
            return date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2");
        }

        //Converts a iCal date and time to a DateTime object
        public static DateTime StringToDate(string date)
        {
            if (date.Length == 8)
                return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)), 0, 0, 0);
            else
                return new DateTime(int.Parse(date.Substring(0,4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)), int.Parse(date.Substring(9, 2)), int.Parse(date.Substring(11, 2)), int.Parse(date.Substring(13, 2)));
        }

        //Converts a line from iCal to C# compatible string
        public static string ConvertFromCALString(string text)
        {
            return text.Replace("\\n", "\r\n").Replace("\\,", ",");
        }

        //Converts a string to a iCal compatible string
        public static string ConvertToCALString(string text)
        {
            return text.Replace("\n", "\\n").Replace("\r","").Replace(",", "\\,");
        }
    }
}