using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;

namespace PdfToolkit.Services
{
    public sealed class TableBuilder
    {
        private readonly Table _table;
        private readonly Document _document;

        internal TableBuilder(Document document, Section section, params double[] columnWidths)
        {
            _document = document;
            _table = section.AddTable();
            foreach (var width in columnWidths)
                _table.AddColumn(Unit.FromCentimeter(width));
        }

        public TableBuilder AddHeader(params string[] headers)
        {
            var row = _table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;

            for (int i = 0; i < headers.Length; i++)
                row.Cells[i].AddParagraph(headers[i]);

            return this;
        }

        public TableBuilder AddRow(params string[] values)
        {
            var row = _table.AddRow();
            for (int i = 0; i < values.Length; i++)
                row.Cells[i].AddParagraph(values[i] ?? string.Empty);
            return this;
        }

        public TableBuilder AddMergedRow(int span, string text, ParagraphAlignment alignment = ParagraphAlignment.Right, bool bold = true)
        {
            var row = _table.AddRow();
            var cell = row.Cells[0];
            cell.MergeRight = span - 1;
            var p = cell.AddParagraph(text);
            p.Format.Alignment = alignment;
            if (bold) p.Format.Font.Bold = true;
            return this;
        }

        internal Table Build() => _table;
    }
}
