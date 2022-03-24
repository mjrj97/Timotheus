using System;
using System.Collections.Generic;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using Timotheus.Schedule;
using Timotheus.IO;
using System.Linq;

namespace Timotheus.Utility
{
    /// <summary>
    /// Class containing methods to export data suchs as the calendar, memberlist and accounts.
    /// </summary>
    public class PDF
    {
        //Primary colors
        private readonly static Color White = new(255, 255, 255);
        private readonly static Color HeadingColor = new(5, 105, 115);

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        private static Style DefineStyles(Document document)
        {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Arvo";

            // Create a new style called Table based on style Normal.
            style = document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Arvo";
            style.Font.Size = 10;

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            // Define Headings
            style = document.Styles["Heading1"];
            style.Font.Name = "Arvo";
            style.Font.Size = 18;
            style.Font.Color = HeadingColor;
            style.Font.Bold = true;

            style = document.Styles["Heading2"];
            style.Font.Name = "Arvo";
            style.Font.Size = 14;
            style.Font.Color = HeadingColor;
            style.Font.Bold = false;

            // Create a new style called Reference based on style Normal.
            style = document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);

            return style;
        }
        
        /// <summary>
        /// Exports a calendar as a PDF.
        /// </summary>
        /// <param name="filePath">Path the PDF should be saved at.</param>
        /// <param name="title">Name of the PDF file.</param>
        /// <param name="associationName">Name of the association.</param>
        /// <param name="associationAddress">Address of the association.</param>
        /// <param name="logoPath">Path to the associations logo.</param>
        /// <param name="periodName">Name of the time period. i.e. fall 2021</param>
        public static void ExportCalendar(List<Event> events, string filePath, string title, string associationName, string associationAddress, string logoPath, string periodName)
        {
            List<Event> SortedList = events.OrderBy(o => o.Start).ToList();
            string fileName = $"{filePath}\\{title}";

            // Create the document using MigraDoc.
            Document document = new();
            document.Info.Title = title;
            document.Info.Author = associationName;
            document.DefaultPageSetup.Orientation = Orientation.Landscape;
            document.UseCmykColor = false;

            DefineStyles(document);

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

            // Each MigraDoc document needs at least one section.
            Section section = document.AddSection();

            section.PageSetup = document.DefaultPageSetup.Clone();
            section.PageSetup.TopMargin = "1cm";
            section.PageSetup.LeftMargin = "1cm";
            section.PageSetup.RightMargin = "1cm";

            //Add logo if defined.
            if (logoPath != string.Empty && logoPath != null)
            {
                Image image = section.AddImage(logoPath);
                image.Height = "3cm";
                image.LockAspectRatio = true;
            }

            section.AddParagraph();

            // add headings
            section.AddParagraph(welcome + " " + associationName, "Heading1");
            section.AddParagraph(schedule + " " + periodName.ToLower(), "Heading2");

            // extra Paragraph to add space
            section.AddParagraph();

            // Create the item table.
            Table table = section.AddTable();
            table.Style = "Table";
            table.Rows.LeftIndent = 0;

            // Define the columns
            table.AddColumn("3cm");
            table.AddColumn("2cm");
            table.AddColumn("10.5cm");
            table.AddColumn("2.8cm");
            table.AddColumn("2.8cm");
            table.AddColumn("6.5cm");

            // Add the header text of the columns.
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.Bold = true;
            row.Shading.Color = White;
            row.Cells[0].AddParagraph(date);
            row.Cells[1].AddParagraph(start);
            row.Cells[2].AddParagraph(activity);
            row.Cells[3].AddParagraph(leader);
            row.Cells[4].AddParagraph(musician);
            row.Cells[5].AddParagraph(coffee);

            // Create a the footer containing the page number and assocaition address.
            Paragraph paragraph = new();
            paragraph.AddPageField();
            paragraph.AddText(" / ");
            paragraph.AddNumPagesField();
            paragraph.AddTab();
            paragraph.AddTab();
            paragraph.AddText(associationAddress);
            paragraph.Format.Font.Size = 9;

            // Add footer to pages.
            section.Footers.Primary.Add(paragraph);
            section.Footers.EvenPage.Add(paragraph.Clone());

            // Creates the dynamic parts of the PDF.
            for (int i = 0; i < SortedList.Count; i++)
            {
                string name = SortedList[i].Name;
                Register values = new(':', SortedList[i].Description);
                DateTime time = SortedList[i].Start;

                string eventLeader = values.Retrieve(leader);
                string eventMusician = values.Retrieve(musician);
                string eventCoffee = values.Retrieve(coffee);

                row = table.AddRow();
                row.Shading.Color = (i % 2 == 1) ? Color.FromRgb(255, 255, 255) : Color.FromRgb(245, 245, 245);

                row.Cells[0].AddParagraph(time.ToString("ddd. d. MMM", Timotheus.Culture));

                if (time.Minute == 0 && time.Hour == 0)
                    row.Cells[1].AddParagraph("");
                else
                    row.Cells[1].AddParagraph(time.ToString("t", Timotheus.Culture));

                row.Cells[2].AddParagraph(name);
                row.Cells[3].AddParagraph(eventLeader);
                row.Cells[4].AddParagraph(eventMusician);
                row.Cells[5].AddParagraph(eventCoffee);
            }

            // Create a renderer for PDF that uses Unicode font encoding.
            PdfDocumentRenderer pdfRenderer = new(true)
            {
                Document = document
            };

            // Create the PDF document.
            pdfRenderer.RenderDocument();
            pdfRenderer.Save(fileName);
        }
    }
}