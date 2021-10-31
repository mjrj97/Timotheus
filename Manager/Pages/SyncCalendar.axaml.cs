using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using ReactiveUI;

namespace Timotheus
{
    public partial class SyncCalendar : Window
    {
        private SyncData data;
        internal Schedule.Calendar? calendar;
        private Schedule.Period period;

        public SyncCalendar()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!data.UseCurrent)
                    calendar.SetupSync(data.Username, data.Password, data.URL);

                if (data.SyncAll)
                    calendar.Sync();
                else if (data.SyncPeriod)
                    calendar.Sync(period);
                else
                    calendar.Sync(new Schedule.Period(data.Start, data.End.AddDays(1)));

                Close();
            }
            catch (Exception ex)
            {
                data.Error = ex.Message;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Show(Window parent, Schedule.Calendar calendar, Schedule.Period period)
        {
            SyncCalendar dialog = new();
            dialog.calendar = calendar;
            dialog.data = new SyncData(calendar.IsSetup());
            dialog.DataContext = dialog.data;
            dialog.data.Period = period.ToString();
            dialog.period = period;

            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();
        }
    }

    public class SyncData : ReactiveObject
    {
        private string _Period = string.Empty;
        public string Period
        {
            get => Localization.Localization.SyncCalendar_PeriodCalendarButton + ": " + _Period;
            set => this.RaiseAndSetIfChanged(ref _Period, value);
        }

        private bool _UseCurrent = false;
        public bool UseCurrent
        {
            get => _UseCurrent;
            set => this.RaiseAndSetIfChanged(ref _UseCurrent, value);
        }

        private bool _CanUseCurrent = false;
        public bool CanUseCurrent
        {
            get => _UseCurrent;
            set => this.RaiseAndSetIfChanged(ref _UseCurrent, value);
        }

        private bool _SyncAll = true;
        public bool SyncAll
        {
            get => _SyncAll;
            set => this.RaiseAndSetIfChanged(ref _SyncAll, value);
        }

        private bool _SyncPeriod = false;
        public bool SyncPeriod
        {
            get => _SyncPeriod;
            set => this.RaiseAndSetIfChanged(ref _SyncPeriod, value);
        }

        private string _URL = string.Empty;
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        private string _Username = string.Empty;
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _Password = string.Empty;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private DateTime _Start = DateTime.Now;
        public DateTime Start
        {
            get { return _Start; }
            set { _Start = value; }
        }

        private DateTime _End = DateTime.Now;
        public DateTime End
        {
            get { return _End; }
            set { _End = value; }
        }

        private string _Error = string.Empty;
        public string Error
        {
            get => _Error;
            set => this.RaiseAndSetIfChanged(ref _Error, value);
        }

        public SyncData(bool UseCurrent)
        {
            this.CanUseCurrent = UseCurrent;
            this.UseCurrent = UseCurrent;
        }
    }
}