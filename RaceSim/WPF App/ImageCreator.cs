using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace WPF_App
{
    public static class ImageCreator
    {
        private static Dictionary<string, Bitmap> _bitmapDic = new Dictionary<string, Bitmap>();

        public static Bitmap GetBitmap(string str)
        {
            if(_bitmapDic.ContainsKey(str))
            {
                return _bitmapDic[str];
            }

            _bitmapDic.Add(str, new Bitmap(str));

            return _bitmapDic[str];
        }
        
        public static void ClearBitDictionary()
        {
            _bitmapDic.Clear();
        }

        public static Bitmap GetBitmap(int x, int y)
        {
            if (_bitmapDic.ContainsKey("emptyMap"))
            {
                return _bitmapDic["emptyMap"];
            }
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 195, 77));
            Bitmap map = new Bitmap(x, y);
            Graphics graph = Graphics.FromImage(map);
            graph.FillRectangle(brush, 0, 0, x, y);

            _bitmapDic.Add("emptyMap", map);

            return _bitmapDic["emptyMap"];
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
