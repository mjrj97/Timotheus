using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Manager.Schedule
{
    public class Calendar
    {
        private readonly string username;
        private readonly string password;
        private readonly string url;
        private List<Event> events = new List<Event>();

        private string timezone;
        private string version;
        private string prodid;

        //Constructor
        public Calendar(string username, string password, string url)
        {
            this.username = username;
            this.password = password;
            this.url = url;

            LoadFromURL();
        }

        //Setters
        public void AddEvent(Event ev)
        {
            Random rnd = new Random();
            string id;
            if (ev.ID == null)
                id = rnd.Next(0, 1000000).ToString();
            else
                id = ev.ID;
            string request = 
            "BEGIN:VCALENDAR\n" +
            "VERSION:" + version + "\n" +
            "PRODID:" + prodid + "\n" + 
            timezone +
            "BEGIN:VEVENT\n" +
            "SUMMARY:" + ev.Name + "\n" +
            "DESCRIPTION:" + ev.Description + "\n" +
            "DTSTART;TZID=Europe/Copenhagen:" + DateToString(ev.StartTime) + "\n" +
            "DTEND;TZID=Europe/Copenhagen:" + DateToString(ev.EndTime) + "\n" +
            "UID:" + id + "\n" +
            "DTSTAMP:" + DateToString(DateTime.Now) + "Z\n" +
            "END:VEVENT\n" +
            "END:VCALENDAR";

            string[] lines = HttpRequest("POST", request);

            if (lines != null)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] != null && lines[i].Contains("</href>"))
                    {
                        int start = 0;
                        int end = 0;
                        for (int j = lines[i].Length - 1; j >= 0; j--)
                        {
                            if (end == 0 && lines[i][j] == '<')
                                end = j;
                            if (end != 0 && start == 0 && lines[i][j] == '/')
                                start = j + 1;
                        }
                        if (lines[i].Contains(".ics"))
                            end -= 4;
                        ev.ID = lines[i].Substring(start, end-start);
                        System.Diagnostics.Debug.WriteLine(ev.ID + " : " + i);
                    }
                }
            }

            events.Add(ev.Copy());
        }

        public void DeleteEvent(string ID)
        {
            events.Remove(FindEvent(ID));
            HttpRequest("DELETE", ID);
        }

        public void LoadFromURL()
        {
            string tempTimeZone = "";
            string tempVersion = "";
            string tempProdID = "";

            DateTime tempStartTime = DateTime.Now;
            DateTime tempEndTime = DateTime.Now;
            string tempName = "";
            string tempDescription = "";
            string tempID = "";

            string[] lines = HttpRequest();
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
                    if (lines[i].Contains("UID"))
                        tempID = GetValue(lines[i]).Trim();
                    if (lines[i].Contains("DTSTART"))
                        tempStartTime = StringToDate(GetValue(lines[i]));
                    if (lines[i].Contains("DTEND"))
                        tempEndTime = StringToDate(GetValue(lines[i]));
                    if (lines[i].Contains("END:VEVENT"))
                        events.Add(new Event(tempStartTime, tempEndTime, tempName, tempDescription, tempID));
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
                System.Diagnostics.Debug.Write(evs[i].ID + ": ");
                if (evs[i].ID == null)
                {
                    System.Diagnostics.Debug.Write("STATUS 1 \n");
                    AddEvent(evs[i]);
                }
                else
                {
                    Event ev = FindEvent(evs[i].ID);
                    if (ev == null)
                    {
                        System.Diagnostics.Debug.Write("STATUS 2 \n");
                        AddEvent(evs[i]);
                    }
                    else
                    {
                        if (evs[i].Name.Equals(Event.DELETE_TAG))
                        {
                            System.Diagnostics.Debug.Write("STATUS 3 \n");
                            DeleteEvent(evs[i].ID);
                        }
                        else if (!evs[i].Equals(ev))
                        {
                            System.Diagnostics.Debug.Write("STATUS 4 \n");
                            DeleteEvent(evs[i].ID);
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
                if (events[i].ID.Equals(ID))
                {
                    ev = events[i];
                }
            }
            return ev;
        }

        //HTTP Request
        public string[] HttpRequest(string method, string dataString)
        {
            //Set up request for URL
            WebRequest request;

            if (method == "DELETE")
            {
                request = WebRequest.Create(url + dataString + ".ics");
                System.Diagnostics.Debug.WriteLine(url + dataString + ".ics");
            }
            else
                request = WebRequest.Create(url);

            NetworkCredential credentials = new NetworkCredential(username, password);
            request.Credentials = credentials;

            if (method != null)
                request.Method = method;
            if (method == "POST")
            {
                byte[] data = Encoding.ASCII.GetBytes(dataString);
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
            for (int i = 0; i < responseFromServer.Length; i++)
            {
                System.Diagnostics.Debug.Write(responseFromServer[i]);
            }
            response.Close();
            return responseFromServer;
        }
        public string[] HttpRequest()
        {
            return HttpRequest(null, null);
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
        public string DateToString(DateTime date)
        {
            return date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + "T" + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
        }

        public DateTime StringToDate(string date)
        {
            return new DateTime(int.Parse(date.Substring(0,4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)), int.Parse(date.Substring(9, 2)), int.Parse(date.Substring(11, 2)), int.Parse(date.Substring(13, 2)));
        }
    }
}