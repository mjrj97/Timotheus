using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Manager.Schedule
{
    public class Calendar
    {
        private readonly NetworkCredential credentials;
        private readonly string url;
        private List<Event> events = new List<Event>();

        private string timezone;
        private string version;
        private string prodid;

        //Constructor
        public Calendar(string username, string password, string url)
        {
            credentials = new NetworkCredential(username, password);
            this.url = url;

            LoadFromURL();
        }

        //Setters
        public void AddEvent(Event ev)
        {
            string request =
            "BEGIN:VCALENDAR\n" +
            "VERSION:" + version + "\n" +
            "PRODID:" + prodid + "\n" +
            timezone +
            "BEGIN:VEVENT\n" +
            "SUMMARY:" + ev.Name + "\n" +
            "DESCRIPTION:" + ev.Description + "\n" +
            "LOCATION:" + ev.Location + "\n";
            if (ev.StartTime.Hour == ev.EndTime.Hour && ev.StartTime.Minute == ev.EndTime.Minute && ev.StartTime.Second == ev.EndTime.Second && ev.StartTime.Hour == 0 && ev.StartTime.Minute == 0 && ev.StartTime.Second == 0)
            {
                request += "DTSTART;TZID=Europe/Copenhagen:" + DateToString(ev.StartTime) + "\n" +
                "DTEND;TZID=Europe/Copenhagen:" + DateToString(ev.EndTime) + "\n";
            }
            else
            {
                request += "DTSTART;TZID=Europe/Copenhagen:" + DateTimeToString(ev.StartTime) + "\n" +
                "DTEND;TZID=Europe/Copenhagen:" + DateTimeToString(ev.EndTime) + "\n";
            }
            request += "UID:" + ev.UID + "\n" +
            "DTSTAMP:" + DateTimeToString(DateTime.Now) + "Z\n" +
            "END:VEVENT\n" +
            "END:VCALENDAR";

            HttpRequest(url + ev.UID + ".ics", credentials, "PUT", request);
            events.Add(ev.Copy());
        }

        public void DeleteEvent(string ID)
        {
            events.Remove(FindEvent(ID));
            HttpRequest(url + ID + ".ics", credentials, "DELETE", ID);
        }

        public void LoadFromURL()
        {
            string tempTimeZone = String.Empty;
            string tempVersion = String.Empty;
            string tempProdID = String.Empty;

            DateTime tempStartTime = DateTime.Now;
            DateTime tempEndTime = DateTime.Now;
            string tempName = String.Empty;
            string tempLocation = String.Empty;
            string tempDescription = String.Empty;
            string tempUID = String.Empty;

            string[] lines = HttpRequest(url, credentials);
            int timeZoneStart = 0;
            int timeZoneEnd = 0;

            events.Clear();
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
                    if (timeZoneStart != 0 && timeZoneEnd == 0)
                        tempTimeZone += lines[i] + "\n";
                    if (lines[i].Contains("END:VTIMEZONE"))
                        timeZoneEnd = i;
                }
                else
                {
                    //Read events
                    if (lines[i].Contains("SUMMARY"))
                        tempName = GetValue(lines[i]);
                    if (lines[i].Contains("DESCRIPTION"))
                        tempDescription = GetValue(lines[i]);
                    if (lines[i].Contains("LOCATION"))
                        tempLocation = GetValue(lines[i]);
                    if (lines[i].Contains("UID"))
                        tempUID = GetValue(lines[i]).Trim();
                    if (lines[i].Contains("DTSTART"))
                        tempStartTime = StringToDate(GetValue(lines[i]));
                    if (lines[i].Contains("DTEND"))
                        tempEndTime = StringToDate(GetValue(lines[i]));
                    if (lines[i].Contains("END:VEVENT"))
                    {
                        events.Add(new Event(tempStartTime, tempEndTime, tempName, tempDescription, tempLocation, tempUID));
                        
                        tempName = String.Empty;
                        tempDescription = String.Empty;
                        tempLocation = String.Empty;
                        tempUID = String.Empty;
                    }
                }
            }

            timezone = tempTimeZone;
            version = tempVersion;
            prodid = tempProdID;
        }

        public void Sync(List<Event> evs)
        {
            for (int i = 0; i < evs.Count; i++)
            {
                if (evs[i].UID == null)
                    AddEvent(evs[i]);
                else
                {
                    Event ev = FindEvent(evs[i].UID);
                    if (ev == null)
                    {
                        if (evs[i].Name != Event.DELETE_TAG)
                            AddEvent(evs[i]);
                    }
                    else
                    {
                        if (evs[i].Name.Equals(Event.DELETE_TAG))
                            DeleteEvent(evs[i].UID);
                        else if (!evs[i].Equals(ev))
                        {
                            DeleteEvent(evs[i].UID);
                            AddEvent(evs[i]);
                        }
                    }
                }
            }
        }

        //Getters
        public void GetEvents(List<Event> events)
        {
            events.Clear();
            for (int i = 0; i < this.events.Count; i++)
            {
                events.Add(this.events[i].Copy());
            }
        }

        public Event FindEvent(string ID)
        {
            Event ev = null;
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].UID.Equals(ID))
                {
                    ev = events[i];
                }
            }
            return ev;
        }

        //HTTP Request
        public static string[] HttpRequest(string url, NetworkCredential credentials, string method, string dataString)
        {
            //Set up request for URL
            WebRequest request = WebRequest.Create(url);
            request.Credentials = credentials;

            if (method != null)
                request.Method = method;
            if (method == "PUT")
            {
                byte[] data = Encoding.UTF8.GetBytes(dataString);
                request.Headers.Add("Accept-charset", "UTF-8");
                request.ContentType = "text/calendar";
                request.ContentLength = data.Length;

                using var stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
            }

            //Get response from URL
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string[] responseFromServer = reader.ReadToEnd().Split("\n");
            response.Close();
            return responseFromServer;
        }
        public static string[] HttpRequest(string url, NetworkCredential credentials)
        {
            return HttpRequest(url, credentials, null, null);
        }

        //Get value after colon
        private string GetValue(string line)
        {
            int colon = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ':' && colon == 0)
                    colon = i;
            }
            return line.Substring(colon + 1);
        }

        //Conversions
        public string DateTimeToString(DateTime date)
        {
            return date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + "T" + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
        }

        public string DateToString(DateTime date)
        {
            return date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2");
        }

        public DateTime StringToDate(string date)
        {
            if (date.Trim().Length == 8)
                return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)), 0, 0, 0);
            else
                return new DateTime(int.Parse(date.Substring(0,4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)), int.Parse(date.Substring(9, 2)), int.Parse(date.Substring(11, 2)), int.Parse(date.Substring(13, 2)));
        }
    }
}