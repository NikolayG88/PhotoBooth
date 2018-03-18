using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class SystemPoint
    {
        public static bool IsEmpty(System.Windows.Point point)
        {
            return point == new System.Windows.Point(Double.MinValue, Double.MinValue);
        }

        public static System.Windows.Point Empty
        {
            get
            {
                return new System.Windows.Point(double.MinValue, double.MinValue);
            }
        }
    }
}
