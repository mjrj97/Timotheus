using System;
using System.Windows.Forms;

namespace Timotheus
{
    public partial class OpenCalendar : Form
    {
        public OpenCalendar()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
