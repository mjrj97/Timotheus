using System;
using System.Globalization;
using System.Collections.Generic;
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
        /// The MigraDoc document that represents the invoice.
        /// </summary>
        private Document _document;

        /// <summary>
        /// The table of the MigraDoc document that contains the invoice items.
        /// </summary>
        private Table _table;

        /// <summary>
        /// filename
        /// </summary>
        private readonly string _filename;

        /// <summary>
        /// list of events
        /// </summary>
        private readonly List<Event> _Events;

        /// <summary>
        /// associationAddress
        /// </summary>
        private readonly string _associationAddress;

        /// <summary>
        /// associationName
        /// </summary>
        private readonly string _associationName;

        /// <summary>
        /// logoPath
        /// </summary>
        private readonly string _logoPath;

        readonly static Color White = new Color(255, 255, 255);

        /// <summary>
        /// Initializes a new instance of the class PDFcreater and opens the specified XML document.
        /// </summary>
        public PDFCreater(string filename, List<Event> Events, String associationName, string associationAddress, string logoPath)
        {
            _filename = filename;
            _Events = Events;
            _associationAddress = associationAddress;
            _associationName = associationName;
            _logoPath = logoPath;
        }

        /// <summary>
        /// Creates the invoice document.
        /// </summary>
        public Document CreateDocument()
        {
            // Create a new MigraDoc document.
            _document = new Document();
            _document.Info.Title = _filename;
            _document.Info.Author = _associationName;

            DefineStyles();

            CreatePage();

            FillContent();

            return _document;
        }

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        void DefineStyles()
        {
            // Get the predefined style Normal.
            var style = _document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Arvo";


            // Create a new style called Table based on style Normal.
            style = _document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Arvo";
            style.Font.Size = 12;

            style = _document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            // Create a new style called Reference based on style Normal.
            style = _document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);

            // Set landscape mode
            _document.DefaultPageSetup.Orientation = Orientation.Landscape;
        }

        /// <summary>
        /// Creates the static parts of the invoice.
        /// </summary>
        void CreatePage()
        {
            // Each MigraDoc document needs at least one section.
            var section = _document.AddSection();

            // Define the page setup. We use an image in the header, therefore the
            // default top margin is too small for our invoice.
            section.PageSetup = _document.DefaultPageSetup.Clone();
            // We increase the TopMargin to prevent the document body from overlapping the page header.
            // We have an image of 3.5 cm height in the header.
            // The default position for the header is 1.25 cm.
            // We add 0.5 cm spacing between header image and body and get 5.25 cm.
            // Default value is 2.5 cm.
            section.PageSetup.TopMargin = "1cm";
            section.PageSetup.LeftMargin = "1cm";
            section.PageSetup.RightMargin = "1cm";

            if (_logoPath != "")
            {
                Image image = section.AddImage(_logoPath);
                image.Height = "3cm";
                image.LockAspectRatio = true;
            }
           

            // add title
            var paragraph = section.AddParagraph("Program");
            paragraph.Format.Font.Size = 40;

            paragraph = section.AddParagraph($"Velkomemen i {_associationName}");
            paragraph.Format.Font.Size = 20;

       


            // extra Paragraph to add space
            section.AddParagraph();

            // Create the item table.
            _table = section.AddTable();
            _table.Style = "Table";
            _table.Borders.Color = White;
            _table.Borders.Width = 0.25;
            _table.Borders.Left.Width = 0.5;
            _table.Borders.Right.Width = 0.5;
            _table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns.
            var column = _table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _table.AddColumn("11cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _table.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            // Create the header of the table.
            var row = _table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.Bold = true;
            row.Shading.Color = White;
            row.Cells[0].AddParagraph("Dato");
            row.Cells[1].AddParagraph("Tidspunkt");  
            row.Cells[2].AddParagraph("Handling");
            row.Cells[3].AddParagraph("Mødeleder ");
            row.Cells[4].AddParagraph("Musiker");
            row.Cells[5].AddParagraph("Kaffehold ");

            // Creater footer
            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText(_associationAddress);
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
        }

        /// <summary>
        /// Creates the dynamic parts of the PDF.
        /// </summary>
        void FillContent()
        {
            _Events.Sort(delegate (Event x, Event y)
            {
                return x.StartTime.CompareTo(y.StartTime);
            });


            for (int i = 0; i < _Events.Count; i++)
            {
                if (_Events[i].Deleted)
                {
                    continue;
                }
                string name = _Events[i].Name;
                string description  = _Events[i].Description;
                DateTime time = _Events[i].StartTime;
                string leader = "";
                string musician = "";
                string coffeeTeam = "";

                var row = this._table.AddRow();

                row.Cells[0].AddParagraph(time.ToString("ddd, MMM d", CultureInfo.CreateSpecificCulture("da-DK")));
                
                if (time.Minute == 0 && time.Hour == 0)
                {
                    row.Cells[1].AddParagraph("Hele dagen");
                }
                else
                {
                    row.Cells[1].AddParagraph(time.ToString("t", CultureInfo.CreateSpecificCulture("da-DK")));
                }

                row.Cells[2].AddParagraph(name);
                row.Cells[3].AddParagraph(leader);
                row.Cells[4].AddParagraph(musician);
                row.Cells[5].AddParagraph(coffeeTeam);
            }   
        }
    }
}