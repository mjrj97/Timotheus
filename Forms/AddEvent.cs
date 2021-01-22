using Manager.Schedule;
using System;
using System.Windows.Forms;

namespace Manager
{
    public partial class AddEvent : Form
    {
        //Constructor
        public AddEvent()
        {
            InitializeComponent();

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddMinutes(30);

            StartTimeBox.Text = start.Hour.ToString("00") + ":" + start.Minute.ToString("00");
            EndTimeBox.Text = end.Hour.ToString("00") + ":" + end.Minute.ToString("00");
            StartTimePicker.Value = start;
            EndTimePicker.Value = end;
        }

        //Buttons
        private void Add_Click(object sender, EventArgs e)
        {
            int hour;
            int minute;
            
            string startTime = StartTimeBox.Text.Trim();
            string endTime = EndTimeBox.Text.Trim();

            DateTime start;
            DateTime end;

            try
            {
                if (NameText.Text.Trim() == String.Empty)
                    throw new Exception("Name cannot be empty.");
                if (NameText.Text.Trim().Equals(Event.DELETE_TAG))
                    throw new Exception("Name cannot be " + Event.DELETE_TAG + ".");

                hour = Int32.Parse(startTime.Substring(0, -3 + startTime.Length));
                minute = Int32.Parse(startTime.Substring(-2 + startTime.Length, 2));
                start = new DateTime(StartTimePicker.Value.Year, StartTimePicker.Value.Month, StartTimePicker.Value.Day, hour, minute, 0);

                hour = Int32.Parse(endTime.Substring(0, -3 + endTime.Length));
                minute = Int32.Parse(endTime.Substring(-2 + endTime.Length, 2));
                end = new DateTime(EndTimePicker.Value.Year, EndTimePicker.Value.Month, EndTimePicker.Value.Day, hour, minute, 0);

                Event ev = new Event(start, end, NameText.Text, DescriptionBox.Text, LocationBox.Text, null);
                MainWindow.window.AddEventToCalendar(ev);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Escape)
                {
                    Close();
                    return true;
                }
                else if (keyData == Keys.Enter && !DescriptionBox.Focused)
                {
                    Add_Click(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
