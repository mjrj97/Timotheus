﻿using System;
using System.Globalization;
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
        /// Decimal separator used by the cultured defined by program.
        /// </summary>
        private readonly char decimalSeparator;

        /// <summary>
        /// Constructor. Loads the localization and initializes the components.
        /// </summary>
        public AddTransaction()
        {
            InitializeComponent();
            decimalSeparator = Convert.ToChar(new CultureInfo(Program.culture, false).NumberFormat.NumberDecimalSeparator);
            AddTransaction_AccountPicker.DataSource = new Account[] {
                new Account{ ID = 1, Name = "One" },
                new Account{ ID = 2, Name = "Two" },
                new Account{ ID = 3, Name = "Three" }
            };

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture);

            Text = locale.GetLocalization(this);
            AddTransaction_AddButton.Text = locale.GetLocalization(AddTransaction_AddButton);
            AddTransaction_CancelButton.Text = locale.GetLocalization(AddTransaction_CancelButton);
            AddTransaction_AccountLabel.Text = locale.GetLocalization(AddTransaction_AccountLabel);
            AddTransaction_AppendixLabel.Text = locale.GetLocalization(AddTransaction_AppendixLabel);
            AddTransaction_Currency1.Text = locale.GetLocalization("AddTransaction_Currency", "$");
            AddTransaction_Currency2.Text = locale.GetLocalization("AddTransaction_Currency", "$");
            AddTransaction_DateLabel.Text = locale.GetLocalization(AddTransaction_DateLabel);
            AddTransaction_InLabel.Text = locale.GetLocalization(AddTransaction_InLabel);
            AddTransaction_OutLabel.Text = locale.GetLocalization(AddTransaction_OutLabel);
        }

        /// <summary>
        /// Adds the transaction to the list.
        /// </summary>
        private void Add(object sender, EventArgs e)
        {
            try
            {
                new Transaction(AddTransaction_DatePicker.Value.Date, ParseStringToInt(AddTransaction_AppendixBox.Text), AddTransaction_DescriptionBox.Text, AddTransaction_AccountPicker.SelectedIndex, ParseStringToDouble(AddTransaction_InBox.Text), ParseStringToDouble(AddTransaction_OutBox.Text));
                MainWindow.window.UpdateAccountingTable();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                value = double.Parse(text, new CultureInfo(Program.culture, false));

            return value;
        }
    }

    class Account
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
