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
        }

        //Buttons
        private void Add_Click(object sender, EventArgs e)
        {
            MainWindow.window.events.Add(new Event(DateTime.Now, DateTime.Now.AddMinutes(30), NameText.Text, "", ""));
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
