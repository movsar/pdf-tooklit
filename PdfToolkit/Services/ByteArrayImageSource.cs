using System.Drawing;
using System.Drawing.Imaging;
using static MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource;

namespace PdfToolkit.Services
{
    public class ByteArrayImageSource : IImageSource
    {
        private readonly byte[] _bytes;
        private readonly Lazy<Bitmap> _bitmap;

        public ByteArrayImageSource(byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
            _bitmap = new Lazy<Bitmap>(() =>
            {
                using var ms = new MemoryStream(_bytes);
                var bmp = new Bitmap(ms);
                return new Bitmap(bmp);
            });
        }

        public string Name { get; }
        public int Width => _bitmap.Value.Width;
        public int Height => _bitmap.Value.Height;

        public bool Transparent => ImageIsTransparent();
        private bool ImageIsTransparent()
            {
            var bmp = _bitmap.Value;
            return bmp.PixelFormat == PixelFormat.Format64bppArgb
                   || bmp.PixelFormat == PixelFormat.Format64bppArgb;
        }
        public void SaveAsJpeg(MemoryStream ms)
        {
            _bitmap.Value.Save(ms, ImageFormat.Jpeg);
        }

        public void SaveAsPdfBitmap(MemoryStream ms)
        {
            _bitmap.Value.Save(ms, ImageFormat.Png);
        }
    }
}
