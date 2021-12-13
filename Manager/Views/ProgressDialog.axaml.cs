using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.ComponentModel;
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

        private readonly BackgroundWorker bw;

        public ProgressDialog()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);

            bw = new();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += DoWork;
            bw.RunWorkerCompleted += Completed;
            bw.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
            bw.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                if (bw.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    bw.ReportProgress(i);
                }
            }
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage / 100.0;
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