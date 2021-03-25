using Timotheus.Schedule;
using Timotheus.Utility;
using System;
using System.Windows.Forms;

namespace Timotheus.Forms
{
    /// <summary>
    /// Add event dialog which contains fields where the users can define the values of the variables found in Event, and adds the event to the current calendar in MainWindow.
    /// </summary>
    public partial class AddEvent : Form
    {
        /// <summary>
        /// Constructor. Loads initial data and loads localization based on culture and directory set by MainWindow.
        /// </summary>
        public AddEvent()
        {
            InitializeComponent();

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddMinutes(30);

            Add_StartBox.Text = start.Hour.ToString("00") + ":" + start.Minute.ToString("00");
            AddEvent_EndBox.Text = end.Hour.ToString("00") + ":" + end.Minute.ToString("00");
            AddEvent_StartPicker.Value = start;
            AddEvent_EndPicker.Value = end;
            AddEvent_LocationBox.Text = MainWindow.window.Settings_AddressBox.Text;

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture.Name);

            Text = locale.GetLocalization(this);
            AddEvent_NameLabel.Text = locale.GetLocalization(AddEvent_NameLabel);
            AddEvent_StartLabel.Text = locale.GetLocalization(AddEvent_StartLabel);
            AddEvent_EndLabel.Text = locale.GetLocalization(AddEvent_EndLabel);
            AddEvent_LocationLabel.Text = locale.GetLocalization(AddEvent_LocationLabel);
            AddEvent_DescriptionLabel.Text = locale.GetLocalization(AddEvent_DescriptionLabel);
            AddEvent_AllDayBox.Text = locale.GetLocalization(AddEvent_AllDayBox);
            AddEvent_AddButton.Text = locale.GetLocalization(AddEvent_AddButton);
            AddEvent_CancelButton.Text = locale.GetLocalization(AddEvent_CancelButton);
        }

        /// <summary>
        /// Adds event to the current calendar in MainWindow
        /// </summary>
        private void AddButton(object sender, EventArgs e)
        {
            int hour = 0;
            int minute = 0;
            
            string startTime = Add_StartBox.Text.Trim();
            string endTime = AddEvent_EndBox.Text.Trim();

            DateTime start;
            DateTime end;

            try
            {
                if (AddEvent_NameBox.Text.Trim() == String.Empty)
                    throw new Exception("Name cannot be empty.");

                if (!AddEvent_AllDayBox.Checked)
                {
                    hour = int.Parse(startTime.Substring(0, -3 + startTime.Length));
                    minute = int.Parse(startTime.Substring(-2 + startTime.Length, 2));
                }
                start = new DateTime(AddEvent_StartPicker.Value.Year, AddEvent_StartPicker.Value.Month, AddEvent_StartPicker.Value.Day, hour, minute, 0);

                if (!AddEvent_AllDayBox.Checked)
                {
                    hour = int.Parse(endTime.Substring(0, -3 + endTime.Length));
                    minute = int.Parse(endTime.Substring(-2 + endTime.Length, 2));
                }
                end = new DateTime(AddEvent_EndPicker.Value.Year, AddEvent_EndPicker.Value.Month, AddEvent_EndPicker.Value.Day, hour, minute, 0);

                Event ev = new Event(start, end, AddEvent_NameBox.Text, AddEvent_DescriptionBox.Text, AddEvent_LocationBox.Text, null);
                MainWindow.window.calendar.events.Add(ev);
                MainWindow.window.UpdateCalendarTable();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Closes the dialog without adding the event
        /// </summary>
        private void CloseButton(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Processes the hotkeys. Escape closes the dialog. Enter adds the event.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Escape)
                {
                    CloseButton(null, null);
                    return true;
                }
                else if (keyData == Keys.Enter && !AddEvent_DescriptionBox.Focused)
                {
                    AddButton(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Enables and disables the start/end time boxes if the events last all day.
        /// </summary>
        private void AllDayBox_CheckedChanged(object sender, EventArgs e)
        {
            Add_StartBox.Enabled = !AddEvent_AllDayBox.Checked;
            AddEvent_EndBox.Enabled = !AddEvent_AllDayBox.Checked;
        }
    }
}