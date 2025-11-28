using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.Tables;
using System;
using System.IO;
using static MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource;

namespace PdfToolkit.Services
{
    public sealed class PdfBuilder
    {
        internal Document Document { get; }
        internal Section CurrentSection { get; private set; }

        public PdfBuilder()
        {
            Document = new Document();
            DefineDefaultStyles(Document);
            CurrentSection = Document.AddSection();
            CurrentSection.PageSetup.PageFormat = PageFormat.Letter;
            CurrentSection.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            CurrentSection.PageSetup.PageWidth = Unit.FromInch(12);
            CurrentSection.PageSetup.PageHeight = Unit.FromInch(9);

        }

        void DefineDefaultStyles(Document doc)
        {
            var normal = doc.Styles["Normal"];
            normal.Font.Name = "Segoe UI";
            normal.Font.Size = 12;

            var title = doc.Styles.AddStyle("Title", "Normal");
            title.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            title.Font.Size = 18;
            title.Font.Bold = true;
            title.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.15);

            var subtitle = doc.Styles.AddStyle("Subtitle", "Normal");
            subtitle.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            subtitle.Font.Size = 12;
            subtitle.Font.Bold = false;
            subtitle.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.1);

            var smallRight = doc.Styles.AddStyle("SmallRight", "Normal");
            smallRight.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            smallRight.Font.Size = 9;
            smallRight.Font.Color = Colors.DarkGray;

            var th = doc.Styles.AddStyle("TableHeader", "Normal");
            th.Font.Size = 10;
            th.Font.Bold = true;
            th.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            th.ParagraphFormat.SpaceBefore = Unit.FromPoint(2);
            th.ParagraphFormat.SpaceAfter = Unit.FromPoint(2);
        }

        // Existing methods
        public void AddHeading(string text)
        {
            var p = CurrentSection.AddParagraph(text, "Title");
            p.Format.SpaceBefore = Unit.FromCentimeter(0.2);
        }

        public Paragraph AddParagraph(string text, string? style = null)
        {
            return CurrentSection.AddParagraph(text, style ?? "Normal");
        }

        public void AddSpacer(double cm)
        {
            var p = CurrentSection.AddParagraph();
            p.Format.SpaceBefore = Unit.FromCentimeter(cm);
        }

      
        public void AddFooter(string text)
        {
            var p = CurrentSection.AddParagraph(text);
            p.Format.Alignment = ParagraphAlignment.Center;
            p.Format.Font.Size = 8;
            p.Format.Font.Color = Colors.DarkGray;
            p.Format.SpaceBefore = Unit.FromCentimeter(0.5);
        }

        public Paragraph AddCenteredParagraph(string text, double? fontSize = null, bool bold = false)
        {
            var p = CurrentSection.AddParagraph(text);
            p.Format.Alignment = ParagraphAlignment.Center;
            if (fontSize.HasValue) p.Format.Font.Size = fontSize.Value;
            p.Format.Font.Bold = bold;
            return p;
        }

        public Paragraph AddParagraphSized(string text, double size, bool bold = false, string? style = null)
        {
            var p = CurrentSection.AddParagraph(text, style ?? "Normal");
            p.Format.Font.Size = size;
            p.Format.Font.Bold = bold;
            return p;
        }

        public void AddImage(byte[] bytes, Unit? width = null)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("Image bytes cannot be null or empty.", nameof(bytes));

            var imageSource = new ByteArrayImageSource(bytes);
            var image = CurrentSection.AddImage(imageSource);

            if (width.HasValue)
                image.Width = width.Value;
        }
        //public Table AddTable(params Unit[] columnWidths)
        //{
        //    var table = CurrentSection.AddTable();
        //    foreach (var w in columnWidths)
        //    {
        //        var col = table.AddColumn(w);
        //        col.Format.Alignment = ParagraphAlignment.Left;
        //    }
        //    return table;
        //}

        public TableBuilder AddTable(params double[] columnWidthsCm)
            => new TableBuilder(CurrentSection.Document, CurrentSection, columnWidthsCm);
    }
}
