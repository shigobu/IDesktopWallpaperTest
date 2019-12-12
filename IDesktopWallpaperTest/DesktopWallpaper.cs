using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDesktopWallpaperTest.Native;

namespace IDesktopWallpaperTest
{
    class DesktopWallpaper
    {
        private IDesktopWallpaper _desktopWallpaper;

        public DesktopWallpaper()
        {
            _desktopWallpaper = (IDesktopWallpaper)new DesktopWallpaperClass();
        }

        public void SetWallpaper(string monitorID, string wallpaper)
        {
            _desktopWallpaper.SetWallpaper(monitorID, wallpaper);
        }

        public string GetWallpaper(string monitorID)
        {
            return _desktopWallpaper.GetWallpaper(monitorID);
        }

        /// <summary>
        /// Gets the monitor device path.
        /// </summary>
        /// <param name="monitorIndex">Index of the monitor device in the monitor device list.</param>
        /// <returns></returns>
        public string GetMonitorDevicePathAt(uint monitorIndex)
        {
            return _desktopWallpaper.GetMonitorDevicePathAt(monitorIndex);
        }
        /// <summary>
        /// Gets number of monitor device paths.
        /// </summary>
        /// <returns></returns>
        public uint GetMonitorDevicePathCount()
        {
            return _desktopWallpaper.GetMonitorDevicePathCount();
        }

        public Rect GetMonitorRECT(string monitorID)
        {
            return _desktopWallpaper.GetMonitorRECT(monitorID);
        }

        public void SetBackgroundColor(uint color)
        {
            _desktopWallpaper.SetBackgroundColor(color);
        }


        public uint GetBackgroundColor()
        {
            return _desktopWallpaper.GetBackgroundColor();
        }

        public void SetPosition(DesktopWallpaperPosition position)
        {

        }

        public DesktopWallpaperPosition GetPosition()
        {
            return _desktopWallpaper.GetPosition();
        }

        public void SetSlideshow(IntPtr items)
        {
            throw new NotImplementedException("開発者メモ：IShellItemArrayの実装が必要");
            //_desktopWallpaper.SetSlideshow(items);
        }

        public IntPtr GetSlideshow()
        {
            throw new NotImplementedException("開発者メモ：IShellItemArrayの実装が必要");
            //return _desktopWallpaper.GetSlideshow();
        }

        public void SetSlideshowOptions(DesktopSlideshowDirection options, uint slideshowTick)
        {
            _desktopWallpaper.SetSlideshowOptions(options, slideshowTick);
        }

        public uint GetSlideshowOptions(out DesktopSlideshowDirection options, out uint slideshowTick)
        {
            return _desktopWallpaper.GetSlideshowOptions(out options, out slideshowTick);
        }

        public void AdvanceSlideshow(string monitorID, DesktopSlideshowDirection direction)
        {
            _desktopWallpaper.AdvanceSlideshow(monitorID, direction);
        }

        public DesktopSlideshowDirection GetStatus()
        {
            return _desktopWallpaper.GetStatus();
        }

        public bool Enable()
        {
            return _desktopWallpaper.Enable();
        }

    }
}
