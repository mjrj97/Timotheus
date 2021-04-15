using System;
using System.Windows.Forms;
using Timotheus.Utility;
using Timotheus.Accounting;

namespace Timotheus.Forms
{
    /// <summary>
    /// Dialog used to add a transaction to the list.
    /// </summary>
    public partial class AddTransaction : Form
    {
        /// <summary>
        /// Date of the transaction
        /// </summary>
        public DateTime Transaction_Date;
        /// <summary>
        /// Number of the appendix associated with the transaction.
        /// </summary>
        public int Transaction_Appendix;
        /// <summary>
        /// Text that describes the transaction.
        /// </summary>
        public string Transaction_Description;
        /// <summary>
        /// Account number associated with the transaction.
        /// </summary>
        public int Transaction_Account;
        /// <summary>
        /// Income.
        /// </summary>
        public double Transaction_In;
        /// <summary>
        /// Expense.
        /// </summary>
        public double Transaction_Out;

        /// <summary>
        /// Decimal separator used by the cultured defined by program.
        /// </summary>
        private readonly char decimalSeparator;

        /// <summary>
        /// Constructor. Loads the localization and initializes the components.
        /// </summary>
        public AddTransaction(Account[] accounts)
        {
            InitializeComponent();
            decimalSeparator = Convert.ToChar(Program.culture.NumberFormat.NumberDecimalSeparator);
            AddTransaction_AccountPicker.DataSource = accounts;

            Text = Localization.Get(this);
            AddTransaction_AddButton.Text = Localization.Get(AddTransaction_AddButton);
            AddTransaction_CancelButton.Text = Localization.Get(AddTransaction_CancelButton);
            AddTransaction_AccountLabel.Text = Localization.Get(AddTransaction_AccountLabel);
            AddTransaction_AppendixLabel.Text = Localization.Get(AddTransaction_AppendixLabel);
            AddTransaction_Currency1.Text = Localization.Get("AddTransaction_Currency", "$");
            AddTransaction_Currency2.Text = Localization.Get("AddTransaction_Currency", "$");
            AddTransaction_DateLabel.Text = Localization.Get(AddTransaction_DateLabel);
            AddTransaction_InLabel.Text = Localization.Get(AddTransaction_InLabel);
            AddTransaction_OutLabel.Text = Localization.Get(AddTransaction_OutLabel);
        }

        /// <summary>
        /// Adds the transaction to the list.
        /// </summary>
        private void Add(object sender, EventArgs e)
        {
            try
            {
                Transaction_Date = AddTransaction_DatePicker.Value.Date;
                Transaction_Appendix = ParseStringToInt(AddTransaction_AppendixBox.Text);
                Transaction_Description = AddTransaction_DescriptionBox.Text;
                Transaction_Account = ((Account)AddTransaction_AccountPicker.SelectedItem).ID;
                Transaction_In = ParseStringToDouble(AddTransaction_InBox.Text);
                Transaction_Out = ParseStringToDouble(AddTransaction_OutBox.Text);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error(ex.Message, "Exception_InvalidInput");
            }
        }

        /// <summary>
        /// Closes the dialog without adding the transaction.
        /// </summary>
        private void Close(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
                    Close(null, null);
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

        /// <summary>
        /// Forces the text box to only include numbers and the appropriate decimal separator.
        /// </summary>
        private void OnlyNumbersBox(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != decimalSeparator))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == decimalSeparator) && ((sender as TextBox).Text.IndexOf(decimalSeparator) > -1))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Attempts to parse a string to a int. Returns 0 if string is empty.
        /// </summary>
        /// <param name="text">The text string to be parsed.</param>
        private int ParseStringToInt(string text)
        {
            int value = 0;

            if (text.Length > 0)
                value = int.Parse(AddTransaction_AppendixBox.Text);

            return value;
        }

        /// <summary>
        /// Attempts to parse a string to a double. Returns 0 if string is empty.
        /// </summary>
        /// <param name="text">The text string to be parsed.</param>
        private double ParseStringToDouble(string text)
        {
            double value = 0.0f;

            if (text.Length > 0)
                value = double.Parse(text, Program.culture);

            return value;
        }
    }
}
