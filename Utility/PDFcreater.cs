﻿using System;
using System.Collections.Generic;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using Timotheus.Schedule;

namespace Timotheus.Utility
{
    /// <summary>
    /// Creates the invoice form.
    /// </summary>
    public class PDFCreater
    {
        /// <summary>
        /// White 
        /// </summary>
        private readonly static Color White = new Color(255, 255, 255);

        /// <summary>
        ///  Title Color
        /// </summary>
        private readonly static Color TitleColor = new Color(5, 105, 115);

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

            // Create a new style called Reference based on style Normal.
            style = document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);

            return style;
        }

        /// <summary>
        /// Creates the static parts of the invoice.
        /// </summary>
        private static Table CreatePage(Document document, string logoPath, string associationName, string associationAddress, string periodName)
        {
            string welcome = "Welcome to";
            string schedule = "Schedule for";
            string date = "Date";
            string start = "Start";
            string activity = "Activity";
            string leader = "Leader";
            string musician = "Musician";
            string coffee = "Coffee";

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture.Name);

            welcome = locale.GetLocalization("PDF_Welcome", welcome);
            schedule = locale.GetLocalization("PDF_Schedule", schedule);
            date = locale.GetLocalization("PDF_Date", date);
            start = locale.GetLocalization("PDF_Start", start);
            activity = locale.GetLocalization("PDF_Activity", activity);
            leader = locale.GetLocalization("PDF_Leader", leader);
            musician = locale.GetLocalization("PDF_Musician", musician);
            coffee = locale.GetLocalization("PDF_Coffee", coffee);

            // Each MigraDoc document needs at least one section.
            Section section = document.AddSection();

            // Define the page setup. We use an image in the header, therefore the
            // default top margin is too small for our invoice.
            section.PageSetup = document.DefaultPageSetup.Clone();
            // We increase the TopMargin to prevent the document body from overlapping the page header.
            // We have an image of 3.5 cm height in the header.
            // The default position for the header is 1.25 cm.
            // We add 0.5 cm spacing between header image and body and get 5.25 cm.
            // Default value is 2.5 cm.
            section.PageSetup.TopMargin = "1cm";
            section.PageSetup.LeftMargin = "1cm";
            section.PageSetup.RightMargin = "1cm";

            if (logoPath != string.Empty)
            {
                Image image = section.AddImage(logoPath);
                image.Height = "3cm";
                image.LockAspectRatio = true;
            }

            section.AddParagraph();

            // add title
            Paragraph paragraph = section.AddParagraph(welcome + " " + associationName);
            paragraph.Format.Font.Size = 18;
            paragraph.Format.Font.Color = TitleColor;
            paragraph.Format.Font.Bold = true;

            paragraph = section.AddParagraph(schedule + " " + periodName.ToLower());
            paragraph.Format.Font.Size = 14;
            paragraph.Format.Font.Color = TitleColor;

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

            // Before you can add a row, you must define the columns.
            Column column = table.AddColumn("3.3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn("2.2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn("12.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            // Create the header of the table.
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

            // Create a paragraph with centered page number. See definition of style "Footer".
            paragraph = new Paragraph();
            paragraph.AddPageField();
            paragraph.AddText(" / ");
            paragraph.AddNumPagesField();
            paragraph.AddTab();
            paragraph.AddTab();
            paragraph.AddText(associationAddress);
            paragraph.Format.Font.Size = 9;

            // Add paragraph to footer for pages.
            section.Footers.Primary.Add(paragraph);
            section.Footers.EvenPage.Add(paragraph.Clone());

            return table;
        }

        public static void ExportCalendar(string filePath, string title, List<Event> events, string associationName, string associationAddress, string logoPath, string periodName)
        {
            string fileName = $"{filePath}\\{title}";

            // Create the document using MigraDoc.
            Document document = new Document();
            document.Info.Title = fileName;
            document.Info.Author = associationName;
            document.DefaultPageSetup.Orientation = Orientation.Landscape;
            document.UseCmykColor = false;

            DefineStyles(document);

            Table table = CreatePage(document, logoPath, associationName, associationAddress, periodName);
            
            // Creates the dynamic parts of the PDF.
            for (int i = 0; i < events.Count; i++)
            {
                if (!events[i].Deleted)
                {
                    string name = events[i].Name;
                    string description = events[i].Description;
                    DateTime time = events[i].StartTime;
                    string leader = "";
                    string musician = "";
                    string coffee = "";

                    Row row = table.AddRow();

                    row.Cells[0].AddParagraph(time.ToString("ddd. d. MMM.", Program.culture));

                    if (time.Minute == 0 && time.Hour == 0)
                        row.Cells[1].AddParagraph("");
                    else
                        row.Cells[1].AddParagraph(time.ToString("t", Program.culture));

                    row.Cells[2].AddParagraph(name);
                    row.Cells[3].AddParagraph(leader);
                    row.Cells[4].AddParagraph(musician);
                    row.Cells[5].AddParagraph(coffee);
                }
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