using System;
using System.Collections.Generic;

namespace Timotheus.Schedule
{
    /// <summary>
    /// Object that contains a start and end date.
    /// </summary>
    public class Period
    {
        /// <summary>
        /// Start date of the period.
        /// </summary>
        public DateTime Start;
        /// <summary>
        /// End date of the period.
        /// </summary>
        public DateTime End;
        /// <summary>
        /// The type of period.
        /// </summary>
        public PeriodType Type = PeriodType.Year;

        /// <summary>
        /// Name of the spring period.
        /// </summary>
        private static string spring;
        /// <summary>
        /// Name of the fall period.
        /// </summary>
        private static string fall;
        /// <summary>
        /// Name of the all period.
        /// </summary>
        private static string all;
        /// <summary>
        /// List of the names of the months.
        /// </summary>
        private static List<string> months;

        public Period(DateTime Start, DateTime End)
        {
            this.Start = Start;
            this.End = End;

            if (months == null)
                Initialize();
        }

        private void Initialize()
        {
            System.Globalization.DateTimeFormatInfo dtfi = System.Globalization.CultureInfo.GetCultureInfo(Program.culture.Name).DateTimeFormat;
            months = new List<string>(dtfi.MonthNames);
            months = months.ConvertAll(d => d.ToLower());

            spring = Program.Localization.Get("Calendar_Spring", "Spring");
            fall = Program.Localization.Get("Calendar_Fall", "Fall");
            all = Program.Localization.Get("Calendar_All", "All");
        }

        public void SetPeriod(string inputText)
        {
            string[] SpiltText = inputText.Split(" ");
            string text;

            if (inputText.ToLower().Equals(all.ToLower()))
            {
                Type = PeriodType.All;
            }
            else if (SpiltText.Length == 1 && int.TryParse(inputText, out int NewYear) && NewYear > 0 && NewYear < 10000)
            {
                int change = NewYear - End.Year;
                Start = Start.AddYears(change);
                End = End.AddYears(change);
                Type = PeriodType.Year;
            }
            else if (SpiltText.Length == 2)
            {
                if (int.TryParse(SpiltText[0], out NewYear))
                {
                    text = SpiltText[1].ToLower();
                }
                else if (int.TryParse(SpiltText[1], out NewYear))
                {
                    text = SpiltText[0].ToLower();
                }
                else
                {
                    text = null;
                }

                if (text != null)
                {
                    //check if it is halfyear
                    if (NewYear > 0 && NewYear < 10000 && (text.Equals(fall.ToLower()) | text.Equals(spring.ToLower())))
                    {
                        if (text.Equals(fall.ToLower()))
                        {
                            Start = new DateTime(NewYear, 7, 1);
                            End = new DateTime(NewYear + 1, 1, 1);
                        }
                        else if (text.Equals(spring.ToLower()))
                        {
                            Start = new DateTime(NewYear, 1, 1);
                            End = new DateTime(NewYear, 7, 1);
                        }
                        Type = PeriodType.Halfyear;
                    }
                    else if (months.Contains(text) && NewYear > 0 && NewYear < 10000)
                    {
                        try
                        {
                            DateTime NewDateTime = DateTime.Parse(inputText, Program.culture);
                            Start = new DateTime(NewDateTime.Year, NewDateTime.Month, 1);
                            End = Start.AddMonths(1);
                            Type = PeriodType.Month;
                        }
                        catch (FormatException)
                        {
                            //Should there be mesage here?
                        }
                    }
                }
            }
        }

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
                    Start = new DateTime(Start.Year, Start.Month, 1);
                    End = Start.AddMonths(1);
                    break;
                default:
                    break;
            }

            Type = type;
        }

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

        public bool In(Period period)
        {
            return In(period.Start, period.End);
        }
        public bool In(Event ev) {
            return (ev.StartTime >= Start && ev.StartTime <= End) || (ev.EndTime >= Start && ev.EndTime <= End);
        }
        public bool In(DateTime a, DateTime b)
        {
            return (Start >= a && End <= b) || (Start >= a && End <= b);
        }

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
                        text = Start.Year + " " + fall;
                    else
                        text = Start.Year + " " + spring;
                    break;
                case PeriodType.Month:
                    text = Start.ToString("MMMM", Program.culture) + " " + Start.Year;
                    break;
            }

            return text;
        }
    }

    public enum PeriodType
    {
        All,
        Year,
        Halfyear,
        Month
    }
}