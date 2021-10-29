using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Threading.Tasks;
using ReactiveUI;

namespace Timotheus
{
    public partial class AddEvent : Window
    {
        private readonly EventData eventData;
        public Schedule.Event? ev;

        public AddEvent()
        {
            ev = null;
            eventData = new EventData();
            InitializeComponent();
            DataContext = eventData;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ev = eventData.ToEvent();
                Close();
            }
            catch (Exception ex)
            {
                eventData.Error = ex.Message;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public new static Task<Schedule.Event?> Show(Window parent)
        {
            AddEvent dialog = new();

            TaskCompletionSource<Schedule.Event?> tcs = new();
            dialog.Closed += delegate
            {
                tcs.TrySetResult(dialog.ev);
            };
            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();

            return tcs.Task;
        }
    }

    internal class EventData : ReactiveObject
    {
        private string _EventName = string.Empty;
        public string EventName
        {
            get { return _EventName; }
            set { _EventName = value; }
        }

        #region Start
        private string _StartTime = DateTime.Now.ToString("t");
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        private string _StartDay = DateTime.Now.Day.ToString();
        public string StartDay
        {
            get { return _StartDay; }
            set { _StartDay = value; }
        }

        private int _StartMonth = DateTime.Now.Month - 1;
        public int StartMonth
        {
            get { return _StartMonth; }
            set { _StartMonth = value; }
        }

        private string _StartYear = DateTime.Now.Year.ToString();
        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }
        #endregion

        #region End
        private string _EndTime = DateTime.Now.AddMinutes(90).ToString("t");
        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        private string _EndDay = DateTime.Now.AddMinutes(90).Day.ToString();
        public string EndDay
        {
            get { return _EndDay; }
            set { _EndDay = value; }
        }

        private int _EndMonth = DateTime.Now.AddMinutes(90).Month - 1;
        public int EndMonth
        {
            get { return _EndMonth; }
            set { _EndMonth = value; }
        }

        private string _EndYear = DateTime.Now.AddMinutes(90).Year.ToString();
        public string EndYear
        {
            get { return _EndYear; }
            set { _EndYear = value; }
        }
        #endregion

        private bool _AllDayEvent = false;
        public bool AllDayEvent
        {
            get { return _AllDayEvent; }
            set { _AllDayEvent = value; }
        }

        private string _Location = string.Empty;
        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

        private string _Description = string.Empty;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _Error = string.Empty;
        public string Error
        {
            get => _Error;
            set => this.RaiseAndSetIfChanged(ref _Error, value);
        }

        public Schedule.Event ToEvent()
        {
            if (EventName.Trim() == string.Empty)
                throw new Exception(Localization.Localization.Exception_EmptyName);

            DateTime Start = new DateTime(int.Parse(StartYear), StartMonth + 1, int.Parse(StartDay));
            DateTime End = new DateTime(int.Parse(EndYear), EndMonth + 1, int.Parse(EndDay));

            if (!AllDayEvent)
            {
                int hour, minute;
                
                hour = int.Parse(StartTime.Substring(0, -3 + StartTime.Length));
                minute = int.Parse(StartTime.Substring(-2 + StartTime.Length, 2));
                Start = Start.AddMinutes(minute + hour * 60);

                hour = int.Parse(EndTime.Substring(0, -3 + EndTime.Length));
                minute = int.Parse(EndTime.Substring(-2 + EndTime.Length, 2));
                End = End.AddMinutes(minute + hour * 60);
            }

            return new Schedule.Event(Start, End, EventName, Description, Location, string.Empty);
        }
    }
}
