using System.Diagnostics;
using System.Windows.Forms;

namespace Timotheus.Forms
{
    public partial class Help : Form
    {
        /// <summary>
        /// Dialog with information about the software and contact information.
        /// </summary>
        public Help()
        {
            InitializeComponent();

            Text = Program.Localization.Get(this);
            Help_ContributorsLabel.Text = Program.Localization.Get(Help_ContributorsLabel) + ":\nMartin J. R. Jensen\nJesper Roager";
            Help_VersionLabel.Text = Program.Localization.Get(Help_VersionLabel) + " v. 0.1.0";
            Help_LicenseLabel.Text = Program.Localization.Get(Help_LicenseLabel) + ": Apache-2.0";
            Help_EmailLabel.Text = Program.Localization.Get(Help_EmailLabel);
            Help_SourceLabel.Text = Program.Localization.Get(Help_SourceLabel);
        }

        /// <summary>
        /// Opens link to the GitHub repository.
        /// </summary>
        private void SourceLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help_SourceLink.LinkVisited = true;
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c start https://www.github.com/mjrj97/Manager";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        /// <summary>
        /// Send email to the author.
        /// </summary>
        private void EmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help_EmailLink.LinkVisited = true;
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c start mailto:martin.jensen.1997@hotmail.com";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }
    }
}
