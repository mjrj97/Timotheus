﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using Timotheus.IO;
using Timotheus.Utility;
using Timotheus.Schedule;
using ReactiveUI;

namespace Timotheus
{
    public class MainController : ReactiveObject
    {
        /// <summary>
        /// Register containing all the keys loaded at startup or manually from a key file (.tkey or .txt)
        /// </summary>
        public Register keys;

        /// <summary>
        /// Current calendar used by the program.
        /// </summary>
        public Calendar _Calendar = new();
        public Calendar Calendar
        {
            get
            {
                return _Calendar;
            }
            set
            {
                _Calendar = value;
                UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Type of period used by Calendar_View.
        /// </summary>
        public Period calendarPeriod = new(DateTime.Now.Date, PeriodType.Year);

        public int SelectedPeriod
        {
            get { return (int)calendarPeriod.Type; }
            set
            {
                calendarPeriod.SetType((PeriodType)value);
                UpdateCalendarTable();
            }
        }

        private string _PeriodText = string.Empty;
        public string PeriodText
        {
            get => _PeriodText;
            set => this.RaiseAndSetIfChanged(ref _PeriodText, value);
        }

        private ObservableCollection<Event> _Events = new ObservableCollection<Event>();
        public ObservableCollection<Event> Events
        {
            get => _Events;
            set => this.RaiseAndSetIfChanged(ref _Events, value);
        }

        public MainController() {
            string KeyPath = Program.Registry.Get("KeyPath");
            keys = LoadKey(KeyPath, false);

            Calendar = new(keys.Get("Calendar-Email"), keys.Get("Calendar-Password"), keys.Get("Calendar-URL"));
            PeriodText = calendarPeriod.ToString();
        }

        /// <summary>
        /// Updates the contents of the event table.
        /// </summary>
        public void UpdateCalendarTable()
        {
            Events.Clear();
            for (int i = 0; i < Calendar.events.Count; i++)
            {
                if (Calendar.events[i].In(calendarPeriod) && !Calendar.events[i].Deleted)
                    Events.Add(Calendar.events[i]);
            }
            PeriodText = calendarPeriod.ToString();
        }

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        public void UpdatePeriod(bool add)
        {
            if (add)
                calendarPeriod.Add();
            else
                calendarPeriod.Subtract();
            UpdateCalendarTable();
        }

        /// <summary>
        /// Removes the event from list.
        /// </summary>
        public void Remove(Event ev) {
            ev.Deleted = true;
            UpdateCalendarTable();
        }

        /// <summary>
        /// Loads and returns the key from the path.
        /// </summary>
        /// <param name="path">Path to the key file.</param>
        /// <param name="requirePasswordDialog">Whether a password dialog should be required. If false it tries to get the password stored in the Progarm.Registry.</param>
        private static Register LoadKey(string path, bool requirePasswordDialog)
        {
            Register keys = null;
            if (path != string.Empty)
            {
                string extension = Path.GetExtension(path);
                switch (extension)
                {
                    case ".tkey":
                        string encodedPassword = requirePasswordDialog ? string.Empty : Program.Registry.Get("KeyPassword");
                        byte[] encodedBytes = Program.encoding.GetBytes(encodedPassword);
                        if (requirePasswordDialog)
                            Program.Registry.Remove("KeyPassword");

                        if (encodedPassword != string.Empty)
                        {
                            try
                            {
                                byte[] decodedBytes = Cipher.Decrypt(encodedBytes, Cipher.defkey);
                                string password = Program.encoding.GetString(decodedBytes);

                                keys = new Register(path, password, ':');
                                Program.Registry.Set("KeyPath", path);
                            }
                            catch (System.Security.Cryptography.CryptographicException e)
                            {
                                //Program.Error("Exception_WrongPassword", e.Message);
                                keys = new Register(':');
                            }
                        }
                        else
                        {
                            try
                            {
                                /*PasswordDialog passwordDialog = new PasswordDialog()
                                {
                                    Owner = this
                                };
                                if (passwordDialog.ShowDialog() == DialogResult.OK)
                                {
                                    keys = new Register(path, passwordDialog.Password, ':');
                                    if (passwordDialog.Check)
                                    {
                                        byte[] decodedBytes = Program.encoding.GetBytes(passwordDialog.Password);
                                        encodedBytes = Cipher.Encrypt(decodedBytes, Cipher.defkey);
                                        encodedPassword = Program.encoding.GetString(encodedBytes);
                                        Program.Registry.Set("KeyPassword", encodedPassword);
                                    }
                                    Program.Registry.Set("KeyPath", path);
                                }
                                else*/
                                {
                                    keys = new Register(':');
                                }
                            }
                            catch (System.Security.Cryptography.CryptographicException e)
                            {
                                //Program.Error("Exception_WrongPassword", e.Message);
                                keys = new Register(':');
                            }
                        }
                        break;
                    case ".txt":
                        keys = new Register(path, ':');
                        Program.Registry.Set("KeyPath", path);
                        break;
                }

                //InsertKeys();
            }
            else
                keys = new Register(':');

            return keys;
        }
    }
}