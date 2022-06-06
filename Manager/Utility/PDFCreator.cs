using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Timotheus.IO;
using Timotheus.Schedule;

namespace Timotheus.Utility
{
    public static class PDFCreator
    {
        public static void CreatePDF(List<Event> events, string path, string title, string associationName, string associationAddress, string logoPath, string periodName)
        {
            List<Event> SortedList = events.OrderBy(o => o.Start).ToList();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string fileName = $"{path}\\{title}";
            int pages = SortedList.Count / 40 + 1;

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
            XFont footer = new("Verdana", 9, XFontStyle.Regular);

            XFont tableHeading = new("Verdana", 10, XFontStyle.Bold);
            XFont tableContent = new("Verdana", 10, XFontStyle.Regular);

            #region Localization
            string welcome = Localization.Localization.PDF_Welcome;
            string schedule = Localization.Localization.PDF_Schedule;
            string date = Localization.Localization.PDF_Date;
            string start = Localization.Localization.PDF_Start;
            string activity = Localization.Localization.PDF_Activity;
            string leader = Localization.Localization.PDF_Leader;
            string musician = Localization.Localization.PDF_Musician;
            string coffee = Localization.Localization.PDF_Coffee;
            #endregion
            
            //PAGE HEADER
            if (File.Exists(logoPath))
                gfx.DrawImage(XImage.FromFile(logoPath), new XRect(28, 28, 85, 85));
            gfx.DrawString(welcome + " " + associationName, heading1, headingColor, new XRect(28, 125, 200,18), XStringFormats.TopLeft);
            gfx.DrawString(schedule + " " + periodName.ToLower(), heading2, headingColor, new XRect(28, 148, 200, 18), XStringFormats.TopLeft);

            //TABLE COLUMN HEADERS
            gfx.DrawString(date, tableHeading, XBrushes.Black, new XRect(28, 177, 200, 10), XStringFormats.TopLeft);
            gfx.DrawString(start, tableHeading, XBrushes.Black, new XRect(113, 177, 200, 10), XStringFormats.TopLeft);
            gfx.DrawString(activity, tableHeading, XBrushes.Black, new XRect(170, 177, 200, 10), XStringFormats.TopLeft);
            gfx.DrawString(leader, tableHeading, XBrushes.Black, new XRect(467, 177, 200, 10), XStringFormats.TopLeft);
            gfx.DrawString(musician, tableHeading, XBrushes.Black, new XRect(546, 177, 200, 10), XStringFormats.TopLeft);
            gfx.DrawString(coffee, tableHeading, XBrushes.Black, new XRect(625, 177, 200, 10), XStringFormats.TopLeft);

            //TABLE CONTENTS
            for (int i = 0; i < SortedList.Count && i < 28; i++)
            {
                if (i%2==0)
                    gfx.DrawRectangle(alternatingGray, new XRect(26, 190+12*i, 786, 12));

                string name = SortedList[i].Name;
                Register values = new(':', SortedList[i].Description);
                DateTime time = SortedList[i].Start;

                string eventLeader = values.Retrieve(leader);
                string eventMusician = values.Retrieve(musician);
                string eventCoffee = values.Retrieve(coffee);

                gfx.DrawString(time.ToString("ddd. d. MMM", Timotheus.Culture), tableContent, XBrushes.Black, new XRect(28, 190 + 12 * i, 85, 12), XStringFormats.CenterLeft);
                gfx.DrawString((time.Minute == 0 && time.Hour == 0) ? "" : time.ToString("t", Timotheus.Culture), tableContent, XBrushes.Black, new XRect(113, 190 + 12 * i, 57, 12), XStringFormats.CenterLeft);
                gfx.DrawString(name, tableContent, XBrushes.Black, new XRect(170, 190 + 12 * i, 294, 12), XStringFormats.CenterLeft);
                gfx.DrawString(eventLeader, tableContent, XBrushes.Black, new XRect(464, 190 + 12 * i, 79, 12), XStringFormats.CenterLeft);
                gfx.DrawString(eventMusician, tableContent, XBrushes.Black, new XRect(543, 190 + 12 * i, 79, 12), XStringFormats.CenterLeft);
                gfx.DrawString(eventCoffee, tableContent, XBrushes.Black, new XRect(622, 190 + 12 * i, 164, 12), XStringFormats.CenterLeft);
            }

            //PAGE FOOTER
            gfx.DrawString("1 / " + pages + "                            " + associationAddress, footer, XBrushes.Black, new XRect(28, page.Height - 18 - 28, 200, 18), XStringFormats.TopLeft);

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
                        if (i % 2 == 0)
                            addGFX.DrawRectangle(alternatingGray, new XRect(26, 46 + 12 * i, 786, 12));

                        string name = SortedList[j+i].Name;
                        Register values = new(':', SortedList[j+i].Description);
                        DateTime time = SortedList[j+i].Start;

                        string eventLeader = values.Retrieve(leader);
                        string eventMusician = values.Retrieve(musician);
                        string eventCoffee = values.Retrieve(coffee);

                        addGFX.DrawString(time.ToString("ddd. d. MMM", Timotheus.Culture), tableContent, XBrushes.Black, new XRect(28, 46 + 12 * i, 85, 12), XStringFormats.CenterLeft);
                        addGFX.DrawString((time.Minute == 0 && time.Hour == 0) ? "" : time.ToString("t", Timotheus.Culture), tableContent, XBrushes.Black, new XRect(113, 46 + 12 * i, 57, 12), XStringFormats.CenterLeft);
                        addGFX.DrawString(name, tableContent, XBrushes.Black, new XRect(170, 46 + 12 * i, 294, 12), XStringFormats.CenterLeft);
                        addGFX.DrawString(eventLeader, tableContent, XBrushes.Black, new XRect(464, 46 + 12 * i, 79, 12), XStringFormats.CenterLeft);
                        addGFX.DrawString(eventMusician, tableContent, XBrushes.Black, new XRect(543, 46 + 12 * i, 79, 12), XStringFormats.CenterLeft);
                        addGFX.DrawString(eventCoffee, tableContent, XBrushes.Black, new XRect(622, 46 + 12 * i, 164, 12), XStringFormats.CenterLeft);
                    }

                    //PAGE FOOTER
                    addGFX.DrawString((h+2) + " / " + pages + "                            " + associationAddress, footer, XBrushes.Black, new XRect(28, page.Height - 18 - 28, 200, 18), XStringFormats.TopLeft);
                }
            }

            document.Save(fileName);

            Process p = new()
            {
                StartInfo = new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                }
            };
            p.Start();
        }
    }
}