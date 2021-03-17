using System;
using System.Collections.Generic;
using Timotheus.Forms;

namespace Timotheus.Utility
{
    /// <summary>
    /// Data type consisting of an income and expense corresponding to a bank transaction at a specific date.
    /// </summary>
    public class Transaction
    {
        // Private versions of the data listed below. Used to make a custom setter for the data.
        private DateTime date;
        private double inValue;
        private double outValue;

        /// <summary>
        /// Date of the transaction.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                AddTransaction();
            }
        }
        /// <summary>
        /// Appendix number corresponding to the transaction.
        /// </summary>
        public int Appendix { get; set; }
        /// <summary>
        /// Description/Name of the transaction.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Account attributed to the transaction (e.g. materials, grants).
        /// </summary>
        public int AccountNumber { get; set; }
        /// <summary>
        /// The income from this transaction. Always positive.
        /// </summary>
        public double InValue
        {
            get
            {
                return inValue;
            }
            set
            {
                inValue = Math.Abs(value);
                UpdateBalance();
            }
        }
        /// <summary>
        /// The expense from this transaction. Always positive.
        /// </summary>
        public double OutValue
        {
            get
            {
                return outValue;
            }
            set
            {
                outValue = Math.Abs(value);
                UpdateBalance();
            }
        }
        /// <summary>
        /// The balance after this transaction.
        /// </summary>
        public double Balance { get; private set; }

        /// <summary>
        /// List of transactions sorted by date.
        /// </summary>
        public static List<Transaction> list = new List<Transaction>();

        /// <summary>
        /// Constructor. Creates a transaction and adds it to the list (Sorted by date).
        /// </summary>
        public Transaction(DateTime Date, int Appendix, string Description, int AccountNumber, double InValue, double OutValue)
        {
            this.Date = Date;
            this.Appendix = Appendix;
            this.Description = Description;
            this.AccountNumber = AccountNumber;
            this.InValue = InValue;
            this.OutValue = OutValue;
        }

        /// <summary>
        /// Updates the balance of this, and every following transaction.
        /// </summary>
        private void UpdateBalance()
        {
            double previousBalance = 0.0;
            int i = list.IndexOf(this);
            if (i > 0)
                previousBalance = list[i - 1].Balance;
            Balance = InValue - OutValue + previousBalance;
            if (i < list.Count - 1)
                list[i + 1].UpdateBalance();
        }

        /// <summary>
        /// Adds the transaction to the list (Sorted by date).
        /// </summary>
        private void AddTransaction()
        {
            int insertIndex = -1;
            list.Remove(this);
            if (list.Count >= 1)
            {
                if (list[0].date > date)
                    insertIndex = 0;
                else
                {
                    int i = 1;
                    while (i < list.Count && insertIndex == -1)
                    {
                        if (list[i - 1].date <= date && list[i].date > date)
                            insertIndex = i;
                        i++;
                    }
                }
            }
            if (insertIndex == -1)
                list.Add(this);
            else
                list.Insert(insertIndex, this);
            if (MainWindow.window != null)
            {
                MainWindow.window.UpdateAccountingTable();
                list[0].UpdateBalance();
            }
        }
    }
}