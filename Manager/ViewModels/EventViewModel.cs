using System;
using Timotheus.Schedule;

namespace Timotheus.ViewModels
{
    public class EventViewModel
    {
        private readonly Event ev;

        public string Start
        {
            get
            {
                return ev.Start.ToString("g");
            }
            set
            {
                ev.Start = DateTime.Parse(value);
            }
        }

        public string End
        {
            get
            {
                return ev.End.ToString("g");
            }
            set
            {
                ev.End = DateTime.Parse(value);
            }
        }

        public string Name
        {
            get
            {
                return ev.Name;
            }
            set
            {
                ev.Name = value;
            }
        }

        public string Description
        {
            get
            {
                return ev.Description;
            }
            set
            {
                ev.Description = value;
            }
        }

        public string Location
        {
            get
            {
                return ev.Location;
            }
            set
            {
                ev.Location = value;
            }
        }

        public bool Deleted
        {
            get
            {
                return ev.Deleted;
            }
            set
            {
                ev.Deleted = value;
            }
        }

        public EventViewModel(Event ev)
        {
            this.ev = ev;
        }
    }
}