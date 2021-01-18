﻿using Manager.Schedule;
using System;
using System.ComponentModel;
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
    }
}