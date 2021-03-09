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
            Add_EndBox.Text = end.Hour.ToString("00") + ":" + end.Minute.ToString("00");
            Add_StartPicker.Value = start;
            Add_EndPicker.Value = end;
            Add_LocationBox.Text = MainWindow.window.Settings_AddressBox.Text;

            LocalizationLoader locale = new LocalizationLoader(MainWindow.directory, MainWindow.culture);

            Text = locale.GetLocalization(this);
            Add_NameLabel.Text = locale.GetLocalization(Add_NameLabel);
            Add_StartLabel.Text = locale.GetLocalization(Add_StartLabel);
            Add_EndLabel.Text = locale.GetLocalization(Add_EndLabel);
            Add_LocationLabel.Text = locale.GetLocalization(Add_LocationLabel);
            Add_DescriptionLabel.Text = locale.GetLocalization(Add_DescriptionLabel);
            Add_AllDayBox.Text = locale.GetLocalization(Add_AllDayBox);
            Add_AddButton.Text = locale.GetLocalization(Add_AddButton);
            Add_CancelButton.Text = locale.GetLocalization(Add_CancelButton);
        }

        /// <summary>
        /// Adds event to the current calendar in MainWindow
        /// </summary>
        private void AddButton(object sender, EventArgs e)
        {
            int hour = 0;
            int minute = 0;
            
            string startTime = Add_StartBox.Text.Trim();
            string endTime = Add_EndBox.Text.Trim();

            DateTime start;
            DateTime end;

            try
            {
                if (Add_NameBox.Text.Trim() == String.Empty)
                    throw new Exception("Name cannot be empty.");

                if (!Add_AllDayBox.Checked)
                {
                    hour = int.Parse(startTime.Substring(0, -3 + startTime.Length));
                    minute = int.Parse(startTime.Substring(-2 + startTime.Length, 2));
                }
                start = new DateTime(Add_StartPicker.Value.Year, Add_StartPicker.Value.Month, Add_StartPicker.Value.Day, hour, minute, 0);

                if (!Add_AllDayBox.Checked)
                {
                    hour = int.Parse(endTime.Substring(0, -3 + endTime.Length));
                    minute = int.Parse(endTime.Substring(-2 + endTime.Length, 2));
                }
                end = new DateTime(Add_EndPicker.Value.Year, Add_EndPicker.Value.Month, Add_EndPicker.Value.Day, hour, minute, 0);

                Event ev = new Event(start, end, Add_NameBox.Text, Add_DescriptionBox.Text, Add_LocationBox.Text, null);
                MainWindow.window.calendar.events.Add(ev);
                MainWindow.window.UpdateTable();
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
                else if (keyData == Keys.Enter && !Add_DescriptionBox.Focused)
                {
                    AddButton(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        //Enables and disables the start/end time boxes if the events last all day.
        /// <summary>
        /// Enables and disables the start/end time boxes if the events last all day.
        /// </summary>
        private void AllDayBox_CheckedChanged(object sender, EventArgs e)
        {
            Add_StartBox.Enabled = !Add_AllDayBox.Checked;
            Add_EndBox.Enabled = !Add_AllDayBox.Checked;
        }
    }
}