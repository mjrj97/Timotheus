using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.ComponentModel;
using System.Threading.Tasks;
using Timotheus.Utility;

namespace Timotheus.Views
{
    public partial class ProgressDialog : Dialog
    {
        private double _progress;
        private double Progress 
        { 
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                NotifyPropertyChanged(nameof(Progress));
            }
        }

        private string _message = Localization.Localization.SFTP_Message;
        private string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged(nameof(Message));
            }
        }

        private BackgroundWorker bw;

        public ProgressDialog()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);
        }

        public async Task ShowDialog(Window parent, IProgressible progressible)
        {
            bw = progressible.Worker;
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerCompleted += Completed;
            bw.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
            bw.RunWorkerAsync();

            await ShowDialog(parent);
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage / 100.0;
            Message = e.UserState.ToString();
        }

        private void Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            bw.CancelAsync();
        }
    }
}