using System;
using System.Collections.Generic;
using Timotheus.Forms;

namespace Timotheus.Utility
{
    public class Transaction
    {
        private DateTime date;
        private double inValue;
        private double outValue;

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
        public int Appendix { get; set; }
        public string Description { get; set; }
        public int AccountNumber { get; set; }
        public double InValue
        {
            get
            {
                return inValue;
            }
            set
            {
                inValue = value;
                UpdateBalance();
            }
        }
        public double OutValue
        {
            get
            {
                return outValue;
            }
            set
            {
                outValue = value;
                UpdateBalance();
            }
        }
        public double Balance { get; private set; }

        public static List<Transaction> list = new List<Transaction>();

        public Transaction(DateTime Date, int Appendix, string Description, int AccountNumber, double InValue, double OutValue)
        {
            this.Date = Date;
            this.Appendix = Appendix;
            this.Description = Description;
            this.AccountNumber = AccountNumber;
            this.InValue = InValue;
            this.OutValue = OutValue;
        }

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