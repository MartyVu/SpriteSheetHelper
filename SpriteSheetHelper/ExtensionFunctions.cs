using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteSheetHelper
{
    public static class ExtensionFunctions
    {
        public static T Clamp<T>(this T value, T min, T max) where T: IComparable
        {
            if (value.CompareTo(min) < 0) { return min; }
            else if (value.CompareTo(max) > 0) { return max; }

            return value;            
        }
    }
}
