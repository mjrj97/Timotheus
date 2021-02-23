using System;
using System.Windows.Forms;

namespace Timotheus.Forms
{
    public partial class SyncCalendar : Form
    {
        public SyncCalendar()
        {
            InitializeComponent();
        }

        private void SyncButton_Click(object sender, EventArgs e)
        {
            try
            {
                MainWindow.window.calendar.Sync();
                MainWindow.window.UpdateTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sync error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
