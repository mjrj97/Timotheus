namespace Timotheus.ViewModels
{
    public class PersonViewModel
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public PersonViewModel(string Name)
        {
            this.Name = Name;
        }
    }
}