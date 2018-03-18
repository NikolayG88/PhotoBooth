using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoBooth
{
    public class PrintSizing
    {
        public enum PrintDensity
        {
            DPI300 = 300
        }

        private PrintDensity _selectedDensity = PrintDensity.DPI300;
        public PrintDensity SelectedDensity
        {
            get
            {
                return _selectedDensity;
            }
            set
            {
                _selectedDensity = value;
            }
        }

        /// <summary>
        /// Returns the print density scaled as int. It's so because depending on the scale the enum value may not match the output.
        /// </summary>
        public int SelectedDensityScaled
        {
            get
            {
                return Scale < 0 ? (int)_selectedDensity / Math.Abs(Scale) : (int)_selectedDensity;
            }
        }

        public enum PrintPaperSize
        {
            FourBySix = 1
        }

        PrintPaperSize _selectedSize = PrintPaperSize.FourBySix;
        public PrintPaperSize SelectedSize
        {
            get
            {
                return SelectedSize;
            }
            set
            {
                _selectedSize = value;
            }
        }

        /// <summary>
        /// The size in pixels.
        /// </summary>
        Size RenderSize
        {
            get
            {
                Size size;
                switch (_selectedSize)
                {
                    case PrintPaperSize.FourBySix:
                        size = new Size(4 * (int)_selectedDensity, 6 * (int)_selectedDensity);
                        break;

                    default:
                        size = new Size(100, 200);
                        break;
                }

                return size;
            }
        }

        private int _scale = 0;
        public int Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }


        public Size RenderSizeScaled
        {
            get
            {
                var size = RenderSize;
                if (Scale > 0)
                {
                    return new Size(size.Width * Scale, size.Height * Scale);
                }
                else if (Scale < 0)
                {
                    var scale = Scale * -1;
                    return new Size(size.Width / scale, size.Height / scale);
                }

                return size;
            }
        }
    }
}
