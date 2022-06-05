using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.Diagnostics;
using Timotheus.Schedule;

namespace Timotheus.Utility
{
    public static class PDFCreator
    {
        public static void CreatePDF(List<Event> events, string path, string title, string associationName, string associationAddress, string logoPath, string periodName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string fileName = $"{path}\\{title}";

            PdfDocument document = new();
            document.Info.Title = title;

            PdfPage page = document.AddPage();
            (page.Height, page.Width) = (page.Width, page.Height);
            // WIDTH = 842 pt
            // HEIGHT = 595 pt

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XSolidBrush headingColor = new(XColor.FromArgb(5, 105, 115));
            XSolidBrush alternatingGray = new(XColor.FromArgb(245, 245, 245));
            XFont heading1 = new("Arvo", 18, XFontStyle.Bold);
            XFont heading2 = new("Arvo", 14, XFontStyle.Regular);
            XFont footer = new("Arvo", 9, XFontStyle.Regular);

            XFont tableHeading = new("Arvo", 10, XFontStyle.Bold);
            XFont tableContent = new("Arvo", 10, XFontStyle.Regular);

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
            for (int i = 0; i < 10; i++)
            {
                if (i%2==0)
                    gfx.DrawRectangle(alternatingGray, new XRect(26, 190+12*i, 786, 12));
            }

            //PAGE FOOTER
            gfx.DrawString("1 / 1                            " + associationAddress, footer, XBrushes.Black, new XRect(28, page.Height - 18 - 28, 200, 18), XStringFormats.TopLeft);

            PdfPage page2 = document.AddPage();
            (page2.Height, page2.Width) = (page2.Width, page2.Height);

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