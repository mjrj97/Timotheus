using System;
using System.Windows.Forms;
using Timotheus.Utility;

namespace Timotheus.Forms
{
    /// <summary>
    /// Dialog used to add a transaction to the list.
    /// </summary>
    public partial class AddTransaction : Form
    {
        /// <summary>
        /// Constructor. Loads the localization and initializes the components.
        /// </summary>
        public AddTransaction()
        {
            InitializeComponent();

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture);

            Text = locale.GetLocalization(this);
            AddTransaction_AddButton.Text = locale.GetLocalization(AddTransaction_AddButton);
            AddTransaction_CancelButton.Text = locale.GetLocalization(AddTransaction_CancelButton);
        }

        /// <summary>
        /// Adds the transaction to the list.
        /// </summary>
        private void Add(object sender, EventArgs e)
        {
            new Transaction(DateTime.Now.Date, 0, "Test transaction", 0, 100.0, 50.0);
            MainWindow.window.UpdateAccountingTable();
            Close();
        }

        /// <summary>
        /// Closes the dialog without adding the transaction.
        /// </summary>
        private void Cancel(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Processes the hotkeys. Escape closes the dialog. Enter adds the transaction.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Escape)
                {
                    Cancel(null, null);
                    return true;
                }
                else if (keyData == Keys.Enter)
                {
                    Add(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
