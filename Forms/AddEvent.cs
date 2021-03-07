using Timotheus.Schedule;
using Timotheus.Utility;
using System;
using System.Windows.Forms;

namespace Timotheus.Forms
{
    public partial class AddEvent : Form
    {
        //Constructor
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

            LocalizationLoader locale = new LocalizationLoader(System.Globalization.CultureInfo.CurrentCulture.Name);

            Add_NameLabel.Text = locale.GetLocalization(Add_NameLabel.Name);
            Add_StartLabel.Text = locale.GetLocalization(Add_StartLabel.Name);
            Add_EndLabel.Text = locale.GetLocalization(Add_EndLabel.Name);
            Add_LocationLabel.Text = locale.GetLocalization(Add_LocationLabel.Name);
            Add_DescriptionLabel.Text = locale.GetLocalization(Add_DescriptionLabel.Name);
            Add_AllDayBox.Text = locale.GetLocalization(Add_AllDayBox.Name);
            Add_AddButton.Text = locale.GetLocalization(Add_AddButton.Name);
            Add_CancelButton.Text = locale.GetLocalization(Add_CancelButton.Name);
        }

        //Adds event to the current calendar in MainWindow
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

        //Closes the dialog without adding the event
        private void CloseButton(object sender, EventArgs e)
        {
            Close();
        }

        //Processes the hotkeys
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
        private void AllDayBox_CheckedChanged(object sender, EventArgs e)
        {
            Add_StartBox.Enabled = !Add_AllDayBox.Checked;
            Add_EndBox.Enabled = !Add_AllDayBox.Checked;
        }
    }
}