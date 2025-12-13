using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;

namespace PdfToolkit.Services
{
    public sealed class PdfBuilder
    {
        private readonly Document _document;
        private Section _currentSection;

        public PdfBuilder()
        {
            _document = new Document();
            _currentSection = _document.AddSection();

            _currentSection.PageSetup.PageFormat = PageFormat.Letter;
            _currentSection.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            _currentSection.PageSetup.PageWidth = Unit.FromInch(12);
            _currentSection.PageSetup.PageHeight = Unit.FromInch(9);
        }

        public Document Build()
        {
            return _document;
        }

        public Style AddStyle(string styleName, string? baseStyleName = null)
        {
            return _document.Styles.AddStyle(styleName, baseStyleName ?? "Normal");
        }

        public void AddHeading(string text)
        {
            var p = _currentSection.AddParagraph(text, "Title");
            p.Format.SpaceBefore = Unit.FromCentimeter(0.2);
        }

        public Paragraph AddParagraph(string text, string? style = null)
        {
            return _currentSection.AddParagraph(text, style ?? "Normal");
        }

        public Paragraph AddCenteredParagraph(string text, double? fontSize = null, bool bold = false)
        {
            var p = _currentSection.AddParagraph(text);
            p.Format.Alignment = ParagraphAlignment.Center;
            if (fontSize.HasValue) p.Format.Font.Size = fontSize.Value;
            p.Format.Font.Bold = bold;
            return p;
        }

        public Paragraph AddParagraphSized(string text, double size, bool bold = false, string? style = null)
        {
            var p = _currentSection.AddParagraph(text, style ?? "Normal");
            p.Format.Font.Size = size;
            p.Format.Font.Bold = bold;
            return p;
        }

        public void SetFooter(string text)
        {
            var footer = _currentSection.Footers.Primary;
            var paragraph = footer.AddParagraph(text);
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.Font.Size = 8;
        }

        public void AddTable(Table table)
        {
            _currentSection.Add(table);
        }

        public void AddNewSection()
        {
            _currentSection = _document.AddSection();
            _currentSection.PageSetup.PageFormat = PageFormat.Letter;
            _currentSection.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            _currentSection.PageSetup.PageWidth = Unit.FromInch(12);
            _currentSection.PageSetup.PageHeight = Unit.FromInch(9);
        }
    }
}