using System;
using Timotheus.Utility;

namespace Timotheus.Persons
{
    public class Board
    {
        /// <summary>
        /// List of Boardmembers in the defined period.
        /// </summary>
        public SortableBindingList<BoardMember> BoardMembers = new SortableBindingList<BoardMember>();

        public Board()
        {           
            Person testperson = new Person("Jesper", "rewr", new DateTime(1), new DateTime(1));
            BoardMembers.Add(new BoardMember(testperson, Roles.Ordinary,"string","jr@lillebror.it", "C:\\Users\\JESPER\\Desktop\\DSC_0321.JPG"));

        }
    }
}
