using Timotheus.Schedule;
using Timotheus.Utility;
using System;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Net;

namespace Timotheus.Forms
{
    public partial class MainWindow : Form
    {
        public static MainWindow window;
        public SortableBindingList<Event> shownEvents = new SortableBindingList<Event>();

        private int year;
        public Calendar calendar = new Calendar();
        public SFTP sftp;
        
        //Constructor
        public MainWindow()
        {
            window = this;
            year = DateTime.Now.Year;
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            Year.Text = year.ToString();
            CalendarView.DataSource = new BindingSource(shownEvents, null);
            PasswordBox.PasswordChar = '*';

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullName = Path.Combine(desktopPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 4)
                    UsernameBox.Text = content[4].Trim();
                if (content.Length > 5)
                    PasswordBox.Text = content[5].Trim();
                if (content.Length > 3)
                    HostBox.Text = content[3].Trim();
                if (content.Length > 6)
                    RemoteDirectoryBox.Text = content[6].Trim();
                if (content.Length > 7)
                    LocalDirectoryBox.Text = content[7].Trim();
            }
        }

        //Update contents
        private void UpdateYear(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Text == "+")
                year++;
            else if (button.Text == "-")
                year--;
            Year.Text = year.ToString();
            UpdateTable();
        }

        public void UpdateTable()
        {
            shownEvents.Clear();
            for (int i = 0; i < calendar.events.Count; i++)
            {
                if (calendar.events[i].StartTime.Year == year && !calendar.events[i].Deleted)
                    shownEvents.Add(calendar.events[i]);
            }
            CalendarView.Sort(CalendarView.Columns[0], ListSortDirection.Ascending);
        }

        //Buttons
        private void Add_Click(object sender, EventArgs e)
        {
            AddEvent addEvent = new AddEvent
            {
                Owner = this
            };
            addEvent.ShowDialog();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (shownEvents.Count > 0)
            {
                Event ev = shownEvents[CalendarView.CurrentCell.OwningRow.Index];
                int index = 0;
                for (int i = 0; i < calendar.events.Count; i++)
                {
                    if (ev.Equals(calendar.events[i]))
                        index = i;
                }
                calendar.events[index].Deleted = true;
                UpdateTable();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Stream stream;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "iCalendar files (*.ics)|*.ics"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(calendar.GetCalendarICS(Path.GetFileNameWithoutExtension(saveFileDialog.FileName)));
                    stream.Write(data);
                    stream.Close();
                }
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenCalendar open = new OpenCalendar
            {
                Owner = this
            };
            open.ShowDialog();
        }

        private void SyncCalendar(object sender, EventArgs e)
        {
            SyncCalendar sync = new SyncCalendar
            {
                Owner = this
            };
            sync.ShowDialog();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog openFolderDialog = new FolderBrowserDialog())
            {
                if (openFolderDialog.ShowDialog() == DialogResult.OK)
                {
                    LocalDirectoryBox.Text = openFolderDialog.SelectedPath;
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Delete && tabControl.SelectedIndex == 0)
                {
                    Remove_Click(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
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