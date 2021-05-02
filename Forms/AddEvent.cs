using System;
using System.Windows.Forms;
using Timotheus.Schedule;

namespace Timotheus.Forms
{
    /// <summary>
    /// Add event dialog which contains fields where the users can define the values of the variables found in Event, and adds the event to the current calendar in MainWindow.
    /// </summary>
    public partial class AddEvent : Form
    {
        /// <summary>
        /// Calendar that the event should be added to.
        /// </summary>
        private readonly Calendar calendar;

        /// <summary>
        /// Constructor. Loads initial data and loads localization based on culture and directory set by MainWindow.
        /// </summary>
        public AddEvent(Calendar calendar, string Address)
        {
            this.calendar = calendar;
            InitializeComponent();

            Add_StartBox.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00");
            AddEvent_EndBox.Text = DateTime.Now.AddMinutes(30).Hour.ToString("00") + ":" + DateTime.Now.AddMinutes(30).Minute.ToString("00");
            AddEvent_StartPicker.Value = DateTime.Now;
            AddEvent_EndPicker.Value = DateTime.Now.AddMinutes(30);
            AddEvent_LocationBox.Text = Address;

            Text = Program.Localization.Get(this);
            AddEvent_NameLabel.Text = Program.Localization.Get(AddEvent_NameLabel);
            AddEvent_StartLabel.Text = Program.Localization.Get(AddEvent_StartLabel);
            AddEvent_EndLabel.Text = Program.Localization.Get(AddEvent_EndLabel);
            AddEvent_LocationLabel.Text = Program.Localization.Get(AddEvent_LocationLabel);
            AddEvent_DescriptionLabel.Text = Program.Localization.Get(AddEvent_DescriptionLabel);
            AddEvent_AllDayBox.Text = Program.Localization.Get(AddEvent_AllDayBox);
            AddEvent_AddButton.Text = Program.Localization.Get(AddEvent_AddButton);
            AddEvent_CancelButton.Text = Program.Localization.Get(AddEvent_CancelButton);
        }

        /// <summary>
        /// Adds event to the current calendar in MainWindow
        /// </summary>
        private void Add(object sender, EventArgs e)
        {
            int hour = 0;
            int minute = 0;
            
            string startTime = Add_StartBox.Text.Trim();
            string endTime = AddEvent_EndBox.Text.Trim();

            try
            {
                if (AddEvent_NameBox.Text.Trim() == string.Empty)
                    throw new Exception("Exception_EmptyName");

                if (!AddEvent_AllDayBox.Checked)
                {
                    hour = int.Parse(startTime.Substring(0, -3 + startTime.Length));
                    minute = int.Parse(startTime.Substring(-2 + startTime.Length, 2));
                }
                DateTime Event_Start = new DateTime(AddEvent_StartPicker.Value.Year, AddEvent_StartPicker.Value.Month, AddEvent_StartPicker.Value.Day, hour, minute, 0);

                if (!AddEvent_AllDayBox.Checked)
                {
                    hour = int.Parse(endTime.Substring(0, -3 + endTime.Length));
                    minute = int.Parse(endTime.Substring(-2 + endTime.Length, 2));
                }
                DateTime Event_End = new DateTime(AddEvent_EndPicker.Value.Year, AddEvent_EndPicker.Value.Month, AddEvent_EndPicker.Value.Day, hour, minute, 0);
                
                Event ev = new Event(Event_Start, Event_End, AddEvent_NameBox.Text, AddEvent_DescriptionBox.Text, AddEvent_LocationBox.Text, null);
                calendar.events.Add(ev);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error("Exception_InvalidInput", ex.Message);
            }
        }

        /// <summary>
        /// Closes the dialog without adding the event
        /// </summary>
        private void Close(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
                    Close(null, null);
                    return true;
                }
                else if (keyData == Keys.Enter && !AddEvent_DescriptionBox.Focused)
                {
                    Add(null, null);
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