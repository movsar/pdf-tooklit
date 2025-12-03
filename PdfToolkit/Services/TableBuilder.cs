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

            _table.Format.Alignment = ParagraphAlignment.Center;

            _table.Borders.Width = 0.25;
            _table.Borders.Color = Colors.Black;

            foreach (var width in columnWidths)
                _table.AddColumn(Unit.FromCentimeter(width));
        }

        public TableBuilder AddHeader(params string[] headers)
        {
            var row = _table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 12;
            row.Shading.Color = Colors.White;
            row.VerticalAlignment = VerticalAlignment.Center;

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = row.Cells[i];
                var paragraph = cell.AddParagraph(headers[i]);
                paragraph.Format.Font.Bold = true;
                paragraph.Format.Font.Size = 10;
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.Format.Alignment = ParagraphAlignment.Left;

                cell.Borders.Width = 0.25;
                cell.Borders.Color = Colors.Black;
            }

            return this;
        }

        public TableBuilder AddRow(params string[] values)
        {
            var row = _table.AddRow();
            row.Format.Font.Size = 9;
            row.VerticalAlignment = VerticalAlignment.Center;
            

            for (int i = 0; i < values.Length; i++)
            {
                var cell = row.Cells[i];
                var paragraph = cell.AddParagraph(values[i] ?? string.Empty);
                paragraph.Format.Font.Size = 9;
                cell.VerticalAlignment = VerticalAlignment.Top;
                cell.Format.Alignment = ParagraphAlignment.Left;

                cell.Borders.Width = 0.25;
                cell.Borders.Color = Colors.Black;
            }
            return this;
        }

        public TableBuilder AddMergedRow(int span, string text, params string[] remainingValues)
        {
            var row = _table.AddRow();

            var cell = row.Cells[0];
            cell.MergeRight = span - 1;
            var p = cell.AddParagraph(text);
            p.Format.Alignment = ParagraphAlignment.Right;
            p.Format.Font.Bold = true;

            for (int i = 0; i < remainingValues.Length; i++)
            {
                row.Cells[span + i].AddParagraph(remainingValues[i]);
                row.Cells[span + i].Format.Alignment = ParagraphAlignment.Left;
                row.Format.Font.Bold = true;
            }

            return this;
        }
    }
}
