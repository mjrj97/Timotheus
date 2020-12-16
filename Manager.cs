using Manager.Schedule;
using System.IO;
using System.Windows.Forms;

namespace Manager
{
    public partial class MainWindow : Form
    {
        private int year = 2020;
        
        public MainWindow()
        {
            InitializeComponent();
            UpdateYearText();

            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string fullName = Path.Combine(desktopPath, "Data.txt");
            StreamReader steamReader = new StreamReader(fullName);
            string[] content = steamReader.ReadToEnd().Split("\n");
            steamReader.Close();

            Calendar cal = new Calendar(content[0].Trim(), content[1].Trim(), content[2].Trim());
            CalendarView.DataSource = new BindingSource(cal.events, null);
        }

        private void UpdateYearText()
        {
            Year.Text = year.ToString();
        }

        private void AddYear_Click(object sender, System.EventArgs e)
        {
            year++;
            UpdateYearText();
        }

        private void SubtractYear_Click(object sender, System.EventArgs e)
        {
            year--;
            UpdateYearText();
        }
    }
}