using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            DesktopWallpaper wallpaper = new DesktopWallpaper();

            string imageLocation = @"C:\Windows\Web\Wallpaper\Theme2\img11.jpg";
            uint monitorCount = wallpaper.GetMonitorDevicePathCount();
            string monitorDevicePath = wallpaper.GetMonitorDevicePathAt(1);

            wallpaper.SetWallpaper(monitorDevicePath, imageLocation);

            Console.Read();
        }
    }
}
