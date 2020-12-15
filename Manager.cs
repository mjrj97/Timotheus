using System.ComponentModel;
using System.Windows.Forms;

namespace Manager
{
    public partial class MainWindow : Form
    {
        int year = 2020;

        BindingList<Event> events = new BindingList<Event>();

        public MainWindow()
        {
            events.Add(new Event(2019, 2020, "Event name", "Description of event"));

            InitializeComponent();
            UpdateYearText();

            dataGridView1.DataSource = new BindingSource(events,null);
        }

        private void UpdateYearText()
        {
            Year.Text = year.ToString();
        }

        private void addYear_Click(object sender, System.EventArgs e)
        {
            year++;
            UpdateYearText();
        }

        private void subtractYear_Click(object sender, System.EventArgs e)
        {
            year--;
            UpdateYearText();
        }
    }
}