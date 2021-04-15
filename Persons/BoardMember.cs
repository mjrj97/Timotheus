using System;
using System.Windows.Forms;
using System.Drawing;

namespace Timotheus.Persons
{


    public class BoardMember : Person
    {

        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// tlf
        /// </summary>
        public string tlf { get; set; }

        /// <summary>
        /// picturepath
        /// </summary>
        public Image picturepath { get; set; }

        /// <summary>
        /// role
        /// </summary>
        public Roles Role { get; set; }

        public BoardMember(string Name, string Address, DateTime Birthday, DateTime Entry, Roles role, string tlf = null, string email = null, string picturepath = null)
        {
            this.Name = Name;
            this.Address = Address;
            Comment = string.Empty;
            this.Birthday = Birthday;
            this.Entry = Entry;
            Role = role;
            this.tlf = tlf;
            this.email = email;
            if (picturepath != null)
            {
                this.picturepath = Image.FromFile(picturepath);
            }

            Signed = DateTime.MinValue;
            Version = DateTime.MinValue;
            this.picturepath = Image.FromFile(picturepath);
        }
        public BoardMember(Person member, Roles role, string tlf = null, string email = null, string picturepath = null)
        {
            Name = member.Name;
            Address = member.Address;
            Comment = string.Empty;
            Birthday = member.Birthday;
            Entry = member.Entry;
            Role = role;
            this.tlf = tlf;
            this.email = email;
            if (picturepath != null)
            {
                this.picturepath = Image.FromFile(picturepath);
            }
            
            Signed = DateTime.MinValue;
            Version = DateTime.MinValue;
            
        }
      

    }
    public enum Roles
    {
        Chairman,
        Vicechairman,
        Cashier,
        Secretary,
        Ordinary,
        Deputy
    }
    
}