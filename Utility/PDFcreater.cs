using System;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using Timotheus.Schedule;

namespace Timotheus.Utility
{
    /// <summary>
    /// Class containing methods to export data suchs as the calendar, memberlist and accounts.
    /// </summary>
    public class PDFCreater
    {
        //Primary colors
        private readonly static Color White = new Color(255, 255, 255);
        private readonly static Color HeadingColor = new Color(5, 105, 115);

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
            style.Font.Size = 11;

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
        public static void ExportCalendar(SortableBindingList<Event> events, string filePath, string title, string associationName, string associationAddress, string logoPath, string periodName)
        {
            string fileName = $"{filePath}\\{title}";

            // Create the document using MigraDoc.
            Document document = new Document();
            document.Info.Title = fileName;
            document.Info.Author = associationName;
            document.DefaultPageSetup.Orientation = Orientation.Landscape;
            document.UseCmykColor = false;

            DefineStyles(document);

            #region Localization
            string welcome = "Welcome to";
            string schedule = "Schedule for";
            string date = "Date";
            string start = "Start";
            string activity = "Activity";
            string leader = "Leader";
            string musician = "Musician";
            string coffee = "Coffee";

            welcome = Localization.Get("PDF_Welcome", welcome);
            schedule = Localization.Get("PDF_Schedule", schedule);
            date = Localization.Get("PDF_Date", date);
            start = Localization.Get("PDF_Start", start);
            activity = Localization.Get("PDF_Activity", activity);
            leader = Localization.Get("PDF_Leader", leader);
            musician = Localization.Get("PDF_Musician", musician);
            coffee = Localization.Get("PDF_Coffee", coffee);
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
            table.Borders.Color = White;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Define the columns
            table.AddColumn("3.3cm");
            table.AddColumn("2.2cm");
            table.AddColumn("12.5cm");
            table.AddColumn("3cm");
            table.AddColumn("3cm");
            table.AddColumn("7cm");

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
            Paragraph paragraph = new Paragraph();
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
            for (int i = 0; i < events.Count; i++)
            {
                string name = events[i].Name;
                //string description = events[i].Description;
                DateTime time = events[i].StartTime;

                row = table.AddRow();

                row.Cells[0].AddParagraph(time.ToString("ddd. d. MMM.", Program.culture));

                if (time.Minute == 0 && time.Hour == 0)
                    row.Cells[1].AddParagraph("");
                else
                    row.Cells[1].AddParagraph(time.ToString("t", Program.culture));

                row.Cells[2].AddParagraph(name);
                row.Cells[3].AddParagraph("");
                row.Cells[4].AddParagraph("");
                row.Cells[5].AddParagraph("");
            }

            // Create a renderer for PDF that uses Unicode font encoding.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };

            // Create the PDF document.
            pdfRenderer.RenderDocument();
            pdfRenderer.Save(fileName);
        }
    }
}