using System;
using System.Windows.Forms;

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
        public string picturepath { get; set; }

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

            Signed = DateTime.MinValue;
            Version = DateTime.MinValue;
            this.picturepath = picturepath;
        }
        public BoardMember(Person member, Roles role, string tlf = null, string email = null, string picturepath = null)
        {
            this.Name = member.Name;
            this.Address = member.Address;
            Comment = string.Empty;
            this.Birthday = member.Birthday;
            this.Entry = member.Entry;
            Role = role;
            this.tlf = tlf;
            this.email = email;
            this.picturepath = picturepath;

            Signed = DateTime.MinValue;
            Version = DateTime.MinValue;
            
        }
        public GroupBox genreategruopeBox()
        {
            GroupBox groupBox = new GroupBox();
            Label label1 = new Label();
            PictureBox pictureBox1 = new PictureBox();
            // 
            // groupBox1
            // 
            groupBox.Controls.Add(label1);
            groupBox.Controls.Add(pictureBox1);
            groupBox.Location = new System.Drawing.Point(3, 3);
            groupBox.Name = "groupBox1";
            groupBox.Size = new System.Drawing.Size(241, 133);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "First board member";
           
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new System.Drawing.Point(6, 22);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(100, 50);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 19);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";

            return groupBox;
        }


    }
    public enum Roles
    {
        Chairman,
        Vicechairman,
        Cashier,
        Secretary,
        Ordinary
    }
    
}