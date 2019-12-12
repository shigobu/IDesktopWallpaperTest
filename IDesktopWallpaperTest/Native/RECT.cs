using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IDesktopWallpaperTest.Native
{
    /// <summary>
    /// The Rect structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
    /// http://www.pinvoke.net/default.aspx/Structures/RECT.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

    }
}
