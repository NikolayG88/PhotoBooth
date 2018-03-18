using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoBooth
{
    public class ImageFactory
    {
        /// <summary>
        /// Render a given visual to a bitmap frame
        /// </summary>
        /// <returns></returns>
        public BitmapFrame RenderVisualToBitmap(Visual element, Size renderSize, int dpiX, int dpiY)
        {
            var rect = new Rect(0, 0, renderSize.Width, renderSize.Height);

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Width, (int)rect.Height,
               dpiX, dpiY, PixelFormats.Pbgra32);

            rtb.Render(element);
            var list = new List<string>();

            return BitmapFrame.Create(rtb);
        }

    }
}
