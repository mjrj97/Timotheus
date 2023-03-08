using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Timotheus.IO;
using Timotheus.Schedule;

namespace Timotheus.Utility
{
    public static class PDF
    {
        public static void CreateTable(List<Event> events, string path, string title, string subtitle, string footer, string logoPath, Register columns)
        {
            List<Event> SortedList = events.OrderBy(o => o.Start).ToList();
            List<Key> Columns = columns.RetrieveAll();

            int pages = 1;
            int eventsLeft = SortedList.Count - 28;
            while (eventsLeft > 0)
            {
                eventsLeft -= 40;
                pages++;
            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new();
            document.Info.Title = title;

            PdfPage page = document.AddPage();
            (page.Height, page.Width) = (page.Width, page.Height);
            // WIDTH = 842 pt
            // HEIGHT = 595 pt

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XSolidBrush headingColor = new(XColor.FromArgb(5, 105, 115));
            XSolidBrush alternatingGray = new(XColor.FromArgb(245, 245, 245));
            XFont heading1 = new("Verdana", 18, XFontStyle.Bold);
            XFont heading2 = new("Verdana", 14, XFontStyle.Regular);
            XFont xfooter = new("Verdana", 9, XFontStyle.Regular);

            XFont tableHeading = new("Verdana", 10, XFontStyle.Bold);
            XFont tableContent = new("Verdana", 10, XFontStyle.Regular);

            #region Localization
            string welcome = Localization.PDF_Welcome;
            string schedule = Localization.PDF_Schedule;
            string date = Localization.PDF_Date;
            string start = Localization.PDF_Start;
            string activity = Localization.PDF_Activity;
            string leader = Localization.PDF_Leader;
            string musician = Localization.PDF_Musician;
            string coffee = Localization.PDF_Coffee;
            #endregion

            //PAGE HEADER
            if (File.Exists(logoPath))
                gfx.DrawImage(XImage.FromFile(logoPath), new XRect(28, 28, 85, 85));
            else if (logoPath.Trim() != string.Empty)
                throw new Exception(Localization.Exception_ImageNotFound);
            gfx.DrawString(title, heading1, headingColor, new XRect(28, 125, 200,18), XStringFormats.TopLeft);
            gfx.DrawString(subtitle, heading2, headingColor, new XRect(28, 148, 200, 18), XStringFormats.TopLeft);

            //TABLE COLUMN HEADERS
            gfx.DrawString(date, tableHeading, XBrushes.Black, new XRect(28, 177, 200, 10), XStringFormats.TopLeft);
            gfx.DrawString(start, tableHeading, XBrushes.Black, new XRect(110, 177, 200, 10), XStringFormats.TopLeft);
            gfx.DrawString(activity, tableHeading, XBrushes.Black, new XRect(156, 177, 200, 10), XStringFormats.TopLeft);

            {
				int totalWidth = 0;
				for (int i = 0; i < Columns.Count; i++)
				{
					int width = int.Parse(Columns[i].Value);

					gfx.DrawString(Columns[i].Name, tableHeading, XBrushes.Black, new XRect(450 + totalWidth, 177, 200, 10), XStringFormats.TopLeft);

					totalWidth += width;
				}
			}

            //TABLE CONTENTS
            bool gray = true;
            for (int i = 0; i < SortedList.Count && i < 28; i++)
            {
                if (i > 0 && SortedList[i - 1].Start.Date != SortedList[i].Start.Date)
                    gray = !gray;
                if (gray)
                    gfx.DrawRectangle(alternatingGray, new XRect(26, 190+12*i, 786, 12));

                string name = SortedList[i].Name;
                Register values = new(':', SortedList[i].Description);
                DateTime time = SortedList[i].Start;

                if (i == 0 || (SortedList[i - 1].Start.Date != SortedList[i].Start.Date))
                    gfx.DrawString(time.ToString("ddd. d. MMM", Timotheus.Culture), tableContent, XBrushes.Black, new XRect(28, 190 + 12 * i, 85, 12), XStringFormats.CenterLeft);
                gfx.DrawString((time.Minute == 0 && time.Hour == 0) ? "" : time.ToString("t", Timotheus.Culture), tableContent, XBrushes.Black, new XRect(110, 190 + 12 * i, 57, 12), XStringFormats.CenterLeft);
                gfx.DrawString(name, tableContent, XBrushes.Black, new XRect(156, 190 + 12 * i, 294, 12), XStringFormats.CenterLeft);

				int totalWidth = 0;
				for (int j = 0; j < Columns.Count; j++)
				{
					int width = int.Parse(Columns[j].Value);

					string columnContent = values.Retrieve(Columns[j].Name);

					gfx.DrawString(columnContent, tableContent, XBrushes.Black, new XRect(450 + totalWidth, 190 + 12 * i, 79, 12), XStringFormats.CenterLeft);

					totalWidth += width;
				}
            }

            //PAGE FOOTER
            gfx.DrawString("1 / " + pages + "                            " + footer, xfooter, XBrushes.Black, new XRect(28, page.Height - 18 - 28, 200, 18), XStringFormats.TopLeft);

            if (SortedList.Count > 28)
            {
                for (int h = 0; h < pages-1; h++)
                {
                    PdfPage additionalPage = document.AddPage();
                    (additionalPage.Height, additionalPage.Width) = (additionalPage.Width, additionalPage.Height);
                    XGraphics addGFX = XGraphics.FromPdfPage(additionalPage);

                    int j = 28 + h*40;
                    for (int i = 0; i < (SortedList.Count- j) && i < 40; i++)
                    {
                        if (SortedList[j + i - 1].Start.Date != SortedList[j + i].Start.Date)
                            gray = !gray;
                        if (gray)
                            addGFX.DrawRectangle(alternatingGray, new XRect(26, 46 + 12 * i, 786, 12));

                        string name = SortedList[j+i].Name;
                        Register values = new(':', SortedList[j+i].Description);
                        DateTime time = SortedList[j+i].Start;

                        if (SortedList[j + i - 1].Start.Date != SortedList[j + i].Start.Date)
                            addGFX.DrawString(time.ToString("ddd. d. MMM", Timotheus.Culture), tableContent, XBrushes.Black, new XRect(28, 46 + 12 * i, 85, 12), XStringFormats.CenterLeft);
                        addGFX.DrawString((time.Minute == 0 && time.Hour == 0) ? "" : time.ToString("t", Timotheus.Culture), tableContent, XBrushes.Black, new XRect(113, 46 + 12 * i, 57, 12), XStringFormats.CenterLeft);
                        addGFX.DrawString(name, tableContent, XBrushes.Black, new XRect(170, 46 + 12 * i, 294, 12), XStringFormats.CenterLeft);

						int totalWidth = 0;
						for (int k = 0; k < Columns.Count; k++)
						{
							int width = int.Parse(Columns[k].Value);

							string columnContent = values.Retrieve(Columns[k].Name);

							addGFX.DrawString(columnContent, tableContent, XBrushes.Black, new XRect(450 + totalWidth, 46 + 12 * i, 79, 12), XStringFormats.CenterLeft);

							totalWidth += width;
						}
					}

                    //PAGE FOOTER
                    addGFX.DrawString((h+2) + " / " + pages + "                            " + footer, xfooter, XBrushes.Black, new XRect(28, page.Height - 18 - 28, 200, 18), XStringFormats.TopLeft);
                }
            }

            document.Save(path);

            Process p = new()
            {
                StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                }
            };
            p.Start();
        }

        public static void CreateBook(List<Event> events, string path, string title, string subtitle, string comment, string backpage, string logoPath)
        {
            List<Event> SortedList = events.OrderBy(o => o.Start).ToList();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new();
            double padding = 50;

            XSolidBrush Blue = new(XColor.FromArgb(5, 105, 115));
            XSolidBrush Black = new(XColor.FromArgb(0, 0, 0));
            XSolidBrush Biege = new(XColor.FromArgb(240, 240, 240));

            // FRONT AND BACK PAGE
            {
                PdfPage page = document.AddPage();
                (page.Height, page.Width) = (page.Width, page.Height);
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XTextFormatterEx2 tf = new(gfx);

                double rectWidth = page.Width / 2 - padding * 2;
                gfx.DrawLine(new XPen(Biege, 0.1), new XPoint(842/2, 0), new XPoint(842/2, 595));

                XImage Logo = XImage.FromFile(logoPath);
                XFont Title = new("Verdana", 20, XFontStyle.Bold);
                XFont Subtitle = new("Verdana", 16, XFontStyle.Bold);
                XFont Comment = new("Verdana", 12, XFontStyle.Regular);

                double ImageHeight = 254;
                double ImageWidth = Logo.PixelWidth * ImageHeight / Logo.PixelHeight;
                tf.PrepareDrawString(title, Title, new XRect(0, 0, rectWidth, double.MaxValue), out int lastFittingChar, out double titleHeight);
                tf.PrepareDrawString(subtitle, Subtitle, new XRect(0, 0, rectWidth, double.MaxValue), out int lastFittingChar2, out double subtitleHeight);

                if (File.Exists(logoPath))
                    gfx.DrawImage(Logo, new XRect(page.Width - padding - rectWidth / 2 - ImageWidth / 2, (page.Height - ImageHeight) / 2 - titleHeight, ImageWidth, ImageHeight));
                else if (logoPath.Trim() != string.Empty)
                    throw new Exception(Localization.Exception_ImageNotFound);

                gfx.DrawString(title, Title, Blue, new XRect(page.Width - padding - rectWidth, (page.Height - ImageHeight) / 2 - titleHeight * 4, rectWidth, titleHeight), XStringFormats.TopCenter);

                gfx.DrawString(subtitle, Subtitle, Blue, new XRect(page.Width - padding - rectWidth, (page.Height + ImageHeight) / 2 + titleHeight, rectWidth, subtitleHeight), XStringFormats.TopCenter);
                gfx.DrawString(comment, Comment, Black, new XRect(page.Width - padding - rectWidth, (page.Height + ImageHeight) / 2 + titleHeight + subtitleHeight * 1.5, rectWidth, subtitleHeight), XStringFormats.TopCenter);

                // WHEN ADDING TEXT AFTER TEXT, THE GetTextHeight fails to account properly for newlines. Therefore, split string by newline
                string[] lines = backpage.Split('\n');

                XFont Header = new("Verdana", 10, XFontStyle.Bold);
                XFont Text = new("Verdana", 10, XFontStyle.Regular);

                XFont CurrentFont;
                XBrush CurrentBrush;
                double currentHeight = 0.0;

                for (int i = 0; i < lines.Length; i++)
                {
                    string text;

                    if (lines[i].StartsWith('#'))
                    {
                        CurrentFont = Header;
                        CurrentBrush = Blue;
                        text = lines[i][1..];
                    }
                    else
                    {
                        CurrentFont = Text;
                        CurrentBrush = Black;
                        text = lines[i];
                    }

                    tf.DrawString(text, CurrentFont, CurrentBrush, new XRect(padding, padding + currentHeight, rectWidth, page.Height - padding * 2));

                    tf.PrepareDrawString(text, CurrentFont, new XRect(0, 0, rectWidth, double.MaxValue), out int lastFittingChar3, out double height);
                    if (height != double.MinValue)
                        currentHeight += height;
                    else
                        currentHeight += CurrentFont.Height;
                }
            }

            // EVENT PAGES
            {
                PdfPage page = document.AddPage();
                (page.Height, page.Width) = (page.Width, page.Height);
                // WIDTH = 842 pt
                // HEIGHT = 595 pt

                XGraphics gfx = XGraphics.FromPdfPage(page);
                XTextFormatterEx2 tf = new(gfx);

                double rectWidth = page.Width / 2 - padding * 2;
                gfx.DrawLine(new XPen(Biege, 0.1), new XPoint(842 / 2, 0), new XPoint(842 / 2, 595));

                XFont Month = new("Verdana", 12, XFontStyle.Bold);
                XFont Date = new("Verdana", 10, XFontStyle.Bold);
                XFont Event = new("Verdana", 10, XFontStyle.Regular);
                
                double lineSpacing = 5;
                double dateWidth = 50;
                double currentHeight = 0.0;
                double currentX = padding;

                string[] months =
                {
                    "Januar",
                    "Februar",
                    "Marts",
                    "April",
                    "Maj",
                    "Juni",
                    "Juli",
                    "August",
                    "September",
                    "Oktober",
                    "November",
                    "December"
                };

                double totalHeight = 0.0;
                for (int i = 0; i < SortedList.Count; i++)
                {
                    Event e = SortedList[i];
                    if (i == 0 || SortedList[i - 1].Start.Month != e.Start.Month)
                    {
                        if (i != 0)
                            totalHeight += lineSpacing * 2;
                        tf.PrepareDrawString(months[e.Start.Month - 1], Month, new XRect(0, 0, rectWidth, double.MaxValue), out int lastFittingChar2, out double monthHeight);
                        totalHeight += monthHeight + lineSpacing;
                    }

                    //double height = GetTextHeight(gfx, Event, e.Name, rectWidth - dateWidth);
                    
                    tf.PrepareDrawString(e.Name, Event, new XRect(), out int lastFittingChar, out double height);
                    totalHeight += height + lineSpacing;
                }

                for (int i = 0; i < SortedList.Count; i++)
                {
                    Event e = SortedList[i];
                    double monthHeight = 0.0;
                    if (i == 0 || SortedList[i - 1].Start.Month != e.Start.Month)
                    {
                        if (i != 0)
                            currentHeight += lineSpacing * 2;
                        tf.PrepareDrawString(months[e.Start.Month - 1], Month, new XRect(0, 0, rectWidth, double.MaxValue), out int lastFittingChar2, out monthHeight);
                    }

                    tf.PrepareDrawString(e.Name, Event, new XRect(0,0, rectWidth - dateWidth,double.MaxValue), out int lastFittingChar, out double height);
                    if ((height + monthHeight) > (page.Height - 2 * padding - currentHeight))
                    {
                        currentHeight = 0.0;
                        currentX = page.Width - padding - rectWidth;
                    }

                    if (monthHeight > 0.0)
                    {
                        tf.DrawString(months[e.Start.Month - 1], Month, Blue, new XRect(currentX, padding + currentHeight, rectWidth, monthHeight), XStringFormats.TopLeft);
                        currentHeight += monthHeight + lineSpacing;
                    }

                    if (i == 0 || SortedList[i - 1].Start.Day != e.Start.Day)
                    {
                        string day;
                        if (e.Start.Day != e.End.Day)
                            day = e.Start.Day + "-" + e.End.Day + ".";
                        else
                            day = e.Start.Day.ToString() + ".";
                        tf.DrawString(day, Date, Black, new XRect(currentX, padding + currentHeight, dateWidth, height), XStringFormats.TopLeft);
                    }
                    tf.DrawString(e.Name, Event, Black, new XRect(dateWidth + currentX, padding + currentHeight, rectWidth - dateWidth, height), XStringFormats.TopLeft);
                    currentHeight += height + lineSpacing;
                }
            }

            document.Save(path);

            Process p = new()
            {
                StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                }
            };
            p.Start();
        }
    }
}