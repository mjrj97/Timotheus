using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace Timotheus.Utility
{
    public static class PDFCreator
    {
        public static void CreatePDF()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new();
            document.Info.Title = "Hello world!";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new("Verdana", 20, XFontStyle.BoldItalic);


            System.Diagnostics.Debug.WriteLine(page.Width + " x " + page.Height);
            gfx.DrawRectangle(XBrushes.Red, new XRect(0, 0, 200, 50));
            gfx.DrawString("Hello world!", font, XBrushes.Black, new XRect(0,0,200,50), XStringFormat.Center);

            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/HelloWorld.pdf";

            document.Save(filePath);
        }
    }
}