using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDesktopWallpaperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DesktopWallpaper wallpaper = new DesktopWallpaper();

            string imageLocation = @"C:\Windows\Web\Wallpaper\Theme2\img11.jpg";
            string monitorDevicePath = wallpaper.GetMonitorDevicePathAt(0);

            wallpaper.SetWallpaper(monitorDevicePath, imageLocation);

            Console.Read();
        }
    }
}
