namespace Timotheus.Accounting
{
    public class Account
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Balance { get; private set; }

        public void SetBalance(double value)
        {
            Balance = value;
        }
    }
}