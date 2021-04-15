using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Timotheus.Utility;
using Timotheus.Persons;
using Timotheus.Forms;

namespace Timotheus.Forms
{
    public partial class AddBoardMember : Form
    {
        public AddBoardMember(SortableBindingList<Person>  members)
        {
            InitializeComponent();
            AddBoardMember_PickMemberCombobox.DataSource = members;
           


        }

      
    }
}
