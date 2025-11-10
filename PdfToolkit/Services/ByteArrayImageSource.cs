using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _bitmap = new Lazy<Bitmap>(() => new Bitmap(new MemoryStream(_bytes)));
        }

        /// <summary>
        /// Returns a MemoryStream of the image bytes.
        /// </summary>
        public Stream GetStream()
        {
            return new MemoryStream(_bytes);
        }

        /// <summary>
        /// Name of the image source.
        /// </summary>
        public string Name => "ByteArrayImage";

        /// <summary>
        /// Width in pixels.
        /// </summary>
        public int Width => _bitmap.Value.Width;

        /// <summary>
        /// Height in pixels.
        /// </summary>
        public int Height => _bitmap.Value.Height;

        /// <summary>
        /// Whether the image has transparency.
        /// </summary>
        public bool Transparent => ImageIsTransparent();

        private bool ImageIsTransparent()
        {
            var bmp = _bitmap.Value;
            return bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb
                   || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
        }

        /// <summary>
        /// Not used by MigraDocCore; provided for interface compatibility.
        /// </summary>
        public void SaveAsJpeg(MemoryStream ms)
        {
            _bitmap.Value.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public void SaveAsPdfBitmap(MemoryStream ms)
        {
            _bitmap.Value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
