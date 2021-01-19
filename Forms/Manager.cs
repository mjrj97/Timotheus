using Manager.Schedule;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Manager
{
    public partial class MainWindow : Form
    {
        public static MainWindow window;
        public BindingList<Event> shownEvents = new BindingList<Event>();
        
        private int year;
        private readonly Calendar calendar;
        
        //Constructor
        public MainWindow()
        {
            window = this;
            year = DateTime.Now.Year;
            InitializeComponent();
            UpdateYearText();

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullName = Path.Combine(desktopPath, "Data.txt");
            StreamReader steamReader = new StreamReader(fullName);
            string[] content = steamReader.ReadToEnd().Split("\n");
            steamReader.Close();
            
            calendar = new Calendar(content[0].Trim(), content[1].Trim(), content[2].Trim());
            UpdateTable();
            CalendarView.DataSource = new BindingSource(shownEvents, null);
        }

        public void AddEventToCalendar(Event ev)
        {
            calendar.events.Add(ev);
            UpdateTable();
        }

        //Update contents
        private void UpdateYearText()
        {
            Year.Text = year.ToString();
        }

        private void UpdateTable()
        {
            shownEvents.Clear();
            for (int i = 0; i < calendar.events.Count; i++)
            {
                if (calendar.events[i].StartTime.Year == year)
                    shownEvents.Add(calendar.events[i]);
            }
        }

        //Buttons
        private void AddYear_Click(object sender, EventArgs e)
        {
            year++;
            UpdateYearText();
            UpdateTable();
        }

        private void SubtractYear_Click(object sender, EventArgs e)
        {
            year--;
            UpdateYearText();
            UpdateTable();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            AddEvent addEvent = new AddEvent();
            addEvent.ShowDialog();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            shownEvents.RemoveAt(CalendarView.CurrentCell.OwningRow.Index);
        }

        //Help
        private void SourceLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SourceLink.LinkVisited = true;
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c start https://www.github.com/mjrj97/Manager";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        private void EmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EmailLink.LinkVisited = true;
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c start mailto:martin.jensen.1997@hotmail.com";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        //Tray icon
        private void Open(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            TrayIcon.Visible = false;
        }
        
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Manager_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                TrayIcon.Visible = true;
            }
        }
    }
}