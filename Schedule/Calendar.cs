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
        public List<Event> events = new List<Event>();

        private readonly string timezone;
        private readonly string version;
        private readonly string prodid;

        //Constructor
        public Calendar(string username, string password, string url)
        {
            this.username = username;
            this.password = password;
            this.url = url;

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
                        tempID = GetValue(lines[i]);
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

        public void AddEvent(Event ev)
        {
            string id = "4332";
            string request = 
            "BEGIN:VCALENDAR\n" +
            "VERSION:" + version + "\n" +
            "PRODID:" + prodid + "\n" + 
            timezone +
            "BEGIN:VEVENT\n" +
            "SUMMARY:" + ev.Name + "\n" +
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
                        ev.ID = lines[i].Substring(start, end-start);
                    }
                }
            }
        }

        public void DeleteEvent(string ID)
        {
            HttpRequest("DELETE", ID);
        }

        //HTTP Request
        public string[] HttpRequest(string method, string dataString)
        {
            //Set up request for URL
            WebRequest request;

            if (method == "DELETE")
                request = WebRequest.Create(url + dataString);
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