using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.Tables;
using System;

namespace PdfToolkit.Services
{
    // Small facade to keep using code readable and decoupled from MigraDoc types.
    public sealed class PdfBuilder
    {
        internal Document Document { get; }
        internal Section CurrentSection { get; private set; }

        public PdfBuilder()
        {
            Document = new Document();
            DefineDefaultStyles(Document);
            CurrentSection = Document.AddSection();
        }

        void DefineDefaultStyles(Document doc)
        {
            var normal = doc.Styles["Normal"];
            normal.Font.Name = "Segoe UI";
            normal.Font.Size = 11;
            doc.Styles.AddStyle("Heading1", "Normal").Font.Size = 18;
            doc.Styles["Heading1"].ParagraphFormat.SpaceAfter = "0.25cm";
        }

        public void AddHeading(string text)
        {
            var p = CurrentSection.AddParagraph(text, "Heading1");
            p.Format.SpaceBefore = "0.2cm";
        }

        public Paragraph AddParagraph(string text, string? style = null)
        {
            return CurrentSection.AddParagraph(text, style ?? "Normal");
        }

        public void AddPageBreak()
        {
            CurrentSection.AddPageBreak();
            CurrentSection = Document.AddSection();
        }

        public void AddImage(ImageSource.IImageSource imageSource, Unit? width = null)
        {
            var image = CurrentSection.AddImage(imageSource);
            if (width != null) image.Width = width.Value;
        }

        public Table AddTable(params Unit[] columnWidths)
        {
            var table = CurrentSection.AddTable();
            foreach (var w in columnWidths)
            {
                var col = table.AddColumn(w);
                col.Format.Alignment = ParagraphAlignment.Left;
            }
            return table;
        }
    }
}
