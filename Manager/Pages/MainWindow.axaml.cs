using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System;
using System.IO;
using System.Linq;
using Timotheus.Schedule;
using Timotheus.Utility;

namespace Timotheus
{
    public partial class MainWindow : Window
    {
        public MainController data = new();

        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
            DataGrid filesGrid = this.Find<DataGrid>("Files");
            filesGrid.AddHandler(
                PointerPressedEvent,
                (s, e) =>
                {
                    if (e.MouseButton == MouseButton.Left)
                    {
                        if (e.ClickCount == 2)
                        {
                            var row = ((IControl)e.Source).GetSelfAndVisualAncestors()
                                .OfType<DataGridRow>()
                                .FirstOrDefault();

                            data.GoToDirectory(row.GetIndex());
                        }
                    }
                },
                handledEventsToo: true);
        }

        private async void OpenMessageBox(string words)
        {
            await MessageBox.Show(this, words, "Test title", MessageBox.MessageBoxButtons.YesNoCancel);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null)
            {
                string Message = "";
                foreach (string text in result)
                {
                    Message += text + "\n";
                }
            }

            await MessageBox.Show(this, "Oh shit der er gået noget galt med dit program!", "Test title", MessageBox.MessageBoxButtons.YesNoCancel);
        }

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        private void Period_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Name == "+")
                    data.UpdatePeriod(true);
                else if (button.Name == "-")
                    data.UpdatePeriod(false);
            }
        }

        private async void OpenCalendar_Click(object sender, RoutedEventArgs e)
        {
            Schedule.Calendar calendar = await OpenCalendar.Show(this);
            if (calendar != null)
                data.Calendar = calendar;
        }

        private async void SaveCalendar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            FileDialogFilter filter = new FileDialogFilter();
            filter.Extensions.Add("ics");
            filter.Name = "Calendar (.ics)";

            saveFileDialog.Filters = new System.Collections.Generic.List<FileDialogFilter>();
            saveFileDialog.Filters.Add(filter);

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                try
                {
                    byte[] d = System.Text.Encoding.UTF8.GetBytes(data.Calendar.ToString());
                    File.WriteAllBytes(result, d);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        private async void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
            SyncCalendar.Show(this, data.Calendar, data.calendarPeriod);
            data.UpdateCalendarTable();
        }

        private async void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            Event? ev = await AddEvent.Show(this);
            if (ev != null)
            {
                data.Calendar.events.Add(ev);
                data.UpdateCalendarTable();
            }
        }

        private void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            Event ev = (Event)((Button)e.Source).DataContext;
            if (ev != null)
            {
                data.Remove(ev);
            }
        }

        private async void OpenWindow_Click(object sender, RoutedEventArgs e)
        {
            NewPage newPage = new NewPage();
            await newPage.ShowDialog<string>(this);
        }

        private async void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            FileDialogFilter filter = new FileDialogFilter();
            filter.Extensions.Add("pdf");
            filter.Name = "PDF Files (.pdf)";

            saveFileDialog.Filters = new System.Collections.Generic.List<FileDialogFilter>();
            saveFileDialog.Filters.Add(filter);

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                try
                {
                    FileInfo file = new FileInfo(result);
                    PDF.ExportCalendar(data.Events, file.DirectoryName, file.Name, data.keys.Get("Settings-Name"), data.keys.Get("Settings-Address"), data.keys.Get("Settings-Image"), data.PeriodText);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        private void PeriodBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                data.calendarPeriod.SetPeriod(((TextBox)sender).Text);
                data.UpdateCalendarTable();
            }
        }

        private void UpDirectory_Click(object sender, RoutedEventArgs e)
        {
            data.GoUpDirectory();
        }
    }
}