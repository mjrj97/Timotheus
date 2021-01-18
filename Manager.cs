using Manager.Schedule;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Manager
{
    public partial class MainWindow : Form
    {
        private int year = 2020;
        private readonly Calendar calendar;
        private BindingList<Event> events = new BindingList<Event>();

        //Constructor
        public MainWindow()
        {
            InitializeComponent();
            UpdateYearText();

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullName = Path.Combine(desktopPath, "Data.txt");
            StreamReader steamReader = new StreamReader(fullName);
            string[] content = steamReader.ReadToEnd().Split("\n");
            steamReader.Close();

            calendar = new Calendar(content[0].Trim(), content[1].Trim(), content[2].Trim());
            UpdateTable();
            CalendarView.DataSource = new BindingSource(events, null);
        }

        //Update contents
        private void UpdateYearText()
        {
            Year.Text = year.ToString();
        }

        private void UpdateTable()
        {
            events.Clear();
            for (int i = 0; i < calendar.events.Count; i++)
            {
                if (calendar.events[i].StartTime.Year == year)
                    events.Add(calendar.events[i]);
            }
        }

        //Button functionality
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
            events.Add(new Event(DateTime.Now, DateTime.Now.AddMinutes(30), "", "", ""));
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            events.RemoveAt(CalendarView.CurrentCell.OwningRow.Index);
        }
    }
}