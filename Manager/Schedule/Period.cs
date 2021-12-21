using System;
using System.Collections.Generic;

namespace Timotheus.Schedule
{
    /// <summary>
    /// Object that contains a start and end date.
    /// </summary>
    public class Period
    {
        private DateTime _start;
        /// <summary>
        /// Start date of the period.
        /// </summary>
        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
            }
        }

        private DateTime _end;
        /// <summary>
        /// End date of the period.
        /// </summary>
        public DateTime End
        { 
            get
            {
                return _end;
            }
            set
            {
                _end = value;
            }
        }
        /// <summary>
        /// The type of period.
        /// </summary>
        public PeriodType Type;

        /// <summary>
        /// Name of the spring period.
        /// </summary>
        private static readonly string spring = Localization.Localization.Calendar_Spring;
        /// <summary>
        /// Name of the fall period.
        /// </summary>
        private static readonly string fall = Localization.Localization.Calendar_Fall;
        /// <summary>
        /// Name of the all period.
        /// </summary>
        private static readonly string all = Localization.Localization.Calendar_All;
        /// <summary>
        /// List of the names of the months.
        /// </summary>
        private static List<string> months;

        /// <summary>
        /// Base constructor. Leaves all values as null.
        /// </summary>
        public Period()
        {
            if (months == null)
            {
                months = new List<string>(Timotheus.Culture.DateTimeFormat.MonthNames);
                months = months.ConvertAll(d => d.ToLower());
            }
        }
        /// <summary>
        /// Define a periods start and end date.
        /// </summary>
        /// <param name="Start">Start date of the period.</param>
        /// <param name="End">End date of the period.</param>
        public Period(DateTime Start, DateTime End) : this()
        {
            this.Start = Start;
            this.End = End;

            TimeSpan difference = End - Start;

            if (difference.Days > 400)
                Type = PeriodType.All;
            else if (difference.Days <= 400 && difference.Days > 200)
                Type = PeriodType.Year;
            else if (difference.Days <= 200 && difference.Days > 100)
                Type = PeriodType.Halfyear;
            else if (difference.Days <= 100 || difference.Days > 25)
                Type = PeriodType.Month;
            else
                Type = PeriodType.None;
        }
        /// <summary>
        /// Define a periods by its start date and derives the end by the type.
        /// </summary>
        /// <param name="Start">Start date of the period.</param>
        /// <param name="Type">Type of period. ie. Month</param>
        public Period(DateTime Start, PeriodType Type) : this()
        {
            this.Start = Start;
            this.Type = Type;

            switch (Type)
            {
                case PeriodType.All:
                    this.Start = DateTime.MinValue;
                    End = DateTime.MaxValue;
                    break;
                case PeriodType.Year:
                    End = Start.AddYears(1);
                    break;
                case PeriodType.Halfyear:
                    End = Start.AddMonths(6);
                    break;
                case PeriodType.Month:
                    End = Start.AddMonths(1);
                    break;
                default:
                    break;
            }
        }
        public Period(string text)
        {
            SetPeriod(text);
        }

        /// <summary>
        /// Sets the period according to a string.
        /// </summary>
        /// <param name="inputText">Text to be converted to a period ie. April 2021</param>
        public void SetPeriod(string inputText)
        {
            string text = inputText.Trim();
            
            if (text.ToLower() == all.ToLower())
            {
                Type = PeriodType.All;
                Start = DateTime.MinValue;
                End = DateTime.MaxValue;
            }
            else
            {
                string[] words = text.Split(' ');

                if (words.Length == 1)
                {
                    int year = int.Parse(words[0]);
                    Start = new DateTime(year, 1, 1);
                    End = Start.AddYears(1);
                    Type = PeriodType.Year;
                }
                else if (words.Length == 2)
                {
                    //Find word with year.
                    int i = 1;
                    _ = int.TryParse(words[0], out int year);
                    if (year == 0)
                    {
                        i = 0;
                        _= int.TryParse(words[1], out year);
                    }

                    //Find out what the other word means.
                    if (words[i].Trim().ToLower() == fall.ToLower())
                    {
                        Start = new DateTime(year, 7, 1);
                        End = Start.AddMonths(6);
                        Type = PeriodType.Halfyear;
                    }
                    else if (words[i].Trim().ToLower() == spring.ToLower())
                    {
                        Start = new DateTime(year, 1, 1);
                        End = Start.AddMonths(6);
                        Type = PeriodType.Halfyear;
                    }
                    else
                    {
                        int j = 0;
                        while (j < months.Count - 1 && words[i].ToLower() != months[j].ToLower())
                        {
                            j++;
                        }
                        if (j < 12)
                        {
                            Start = new DateTime(year, j + 1, 1);
                            End = Start.AddMonths(1);
                            Type = PeriodType.Month;
                        }
                        else
                        {
                            Start = new DateTime(year, 1, 1);
                            End = Start.AddYears(1);
                            Type = PeriodType.Year;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the periods type.
        /// </summary>
        /// <param name="type">Could be year, month etc.</param>
        public void SetType(PeriodType type)
        {
            if (type == PeriodType.All)
            {
                Start = DateTime.MinValue;
                End = DateTime.MaxValue;
            }
            switch (type)
            {
                case PeriodType.All:
                    break;
                case PeriodType.Year:
                    if (Type == PeriodType.All)
                    {
                        Start = new DateTime(DateTime.Now.Year, 1, 1);
                        End = new DateTime(DateTime.Now.Year + 1, 1, 1);
                    }
                    else
                    {
                        Start = new DateTime(Start.Year, 1, 1);
                        End = new DateTime(Start.Year + 1, 1, 1);
                    }
                    break;
                case PeriodType.Halfyear:
                    if (Type == PeriodType.All)
                    {
                        if (DateTime.Now.Month > 6)
                        {
                            Start = new DateTime(DateTime.Now.Year, 7, 1);
                            End = new DateTime(DateTime.Now.Year + 1, 1, 1);
                        }
                        else
                        {
                            Start = new DateTime(DateTime.Now.Year, 1, 1);
                            End = new DateTime(DateTime.Now.Year, 7, 1);
                        }
                    }
                    else
                    {
                        if (Start.Month > 6)
                        {
                            Start = new DateTime(Start.Year, 7, 1);
                            End = new DateTime(Start.Year + 1, 1, 1);
                        }
                        else
                        {
                            Start = new DateTime(Start.Year, 1, 1);
                            End = new DateTime(Start.Year, 7, 1);
                        }
                    }
                    break;
                case PeriodType.Month:
                    if (Type == PeriodType.All)
                    {
                        Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        End = Start.AddMonths(1);
                    }
                    else
                    {
                        Start = new DateTime(Start.Year, Start.Month, 1);
                        End = Start.AddMonths(1);
                    }
                    break;
                default:
                    break;
            }

            Type = type;
        }

        /// <summary>
        /// Moves the period forward in time according to the type set. If Type == PeriodType.Year, the start and end is moved 1 year.
        /// </summary>
        public void Add()
        {
            switch (Type)
            {
                case PeriodType.Year:
                    Start = Start.AddYears(1);
                    End = End.AddYears(1);
                    break;
                case PeriodType.Halfyear:
                    Start = Start.AddMonths(6);
                    End = End.AddMonths(6);
                    break;
                case PeriodType.Month:
                    Start = Start.AddMonths(1);
                    End = End.AddMonths(1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Moves the period backwards in time according to the type set. If Type == PeriodType.Year, the start and end is moved -1 year.
        /// </summary>
        public void Subtract()
        {
            switch (Type)
            {
                case PeriodType.Year:
                    Start = Start.AddYears(-1);
                    End = End.AddYears(-1);
                    break;
                case PeriodType.Halfyear:
                    Start = Start.AddMonths(-6);
                    End = End.AddMonths(-6);
                    break;
                case PeriodType.Month:
                    Start = Start.AddMonths(-1);
                    End = End.AddMonths(-1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check if period is in another period.
        /// </summary>
        /// <param name="period">The time interval to be checked against.</param>
        public bool In(Period period)
        {
            return In(period.Start, period.End);
        }
        /// <summary>
        /// Check if period is in between two dates.
        /// </summary>
        /// <param name="Start">First date of the interval.</param>
        /// <param name="End">Last date of the interval.</param>
        public bool In(DateTime Start, DateTime End)
        {
            return (this.Start >= Start && this.Start <= End) || (this.End >= Start && this.End <= End);
        }

        /// <summary>
        /// Returns the period as a string (ie. April 2021)
        /// </summary>
        public override string ToString()
        {
            string text = string.Empty;

            switch (Type)
            {
                case PeriodType.All:
                    text = all;
                    break;
                case PeriodType.Year:
                    text = Start.Year.ToString();
                    break;
                case PeriodType.Halfyear:
                    if (Start.Month > 6)
                        text = fall + " " + Start.Year;
                    else
                        text = spring + " " + Start.Year;
                    break;
                case PeriodType.Month:
                    text = Start.ToString("MMMM", Timotheus.Culture) + " " + Start.Year;
                    break;
            }

            return text;
        }
    }

    /// <summary>
    /// Used to define the type of period.
    /// </summary>
    public enum PeriodType
    {
        All = 0,
        Year = 1,
        Halfyear = 2,
        Month = 3,
        None = 4
    }
}