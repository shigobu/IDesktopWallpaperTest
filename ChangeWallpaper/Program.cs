using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Wallpaper;

namespace ChangeWallpaper
{
	static class Program
	{
        /// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            try
            {
			    string wallpaperDirectory = GetWallpaperDirectory();
                if (string.IsNullOrWhiteSpace(wallpaperDirectory))
                {
                    throw new Exception("壁紙格納フォルダの取得に失敗しました。");
                }

                string[] fileNames = Directory.GetFiles(wallpaperDirectory);
                string[] imageFileNames = ExcludeFileUnusableInWallpaper(fileNames);
                if (imageFileNames.Length == 0)
                {
                    throw new Exception("壁紙に使えるファイルが見つかりませんでした。");
                }

                //現在設定されている壁紙を取得する。
                DesktopWallpaper wallpaper = new DesktopWallpaper();
                uint monitorCount = wallpaper.GetMonitorDevicePathCount();
                string lastMonitorId = wallpaper.GetMonitorDevicePathAt(monitorCount - 1);
                string currentWallpaperFilename = wallpaper.GetWallpaper(lastMonitorId);

                //現在の壁紙が何番目か調べる。
                int currentWallpaperNum = Array.IndexOf(imageFileNames, currentWallpaperFilename);

                //壁紙を変更する
                int wallpaperFileIndex = currentWallpaperNum;
                for (int i = 0; i < monitorCount; i++)
                {
                    wallpaperFileIndex = GetNextIndex(wallpaperFileIndex, imageFileNames.Length);
                    string monitorId = wallpaper.GetMonitorDevicePathAt((uint)i);
                    wallpaper.SetWallpaper(monitorId, imageFileNames[wallpaperFileIndex]);
                }

                //正常終了したらログファイルを削除する。
                File.Delete(LogFilePath);
            }
            catch (Exception ex)
            {
                //ログ出力
                List<string> logMessage = new List<string>();
                logMessage.Add(DateTime.Now.ToLongDateString());
                logMessage.Add(DateTime.Now.ToLongTimeString());
                logMessage.Add(ex.ToString());
                File.WriteAllLines(LogFilePath, logMessage.ToArray());

                //設定ファイル削除
                File.Delete(SettingFilePath);
            }
        }

        #region プロパティ
        static string _appPath = null;

        /// <summary>
        /// 自分のexeフルパス
        /// </summary>
        private static string AppPath
        {
            get
            {
                if (_appPath == null)
                {
                    _appPath = Assembly.GetExecutingAssembly().Location;
                }
                return _appPath;
            }
        }

        /// <summary>
        /// 自分のexeのあるフォルダ
        /// </summary>
        private static string AppDirectory
        {
            get
            {
                return Path.GetDirectoryName(AppPath);
            }
        }

        /// <summary>
        /// 設定ファイルのパス
        /// </summary>
        private static string SettingFilePath
        {
            get
            {
                return Path.Combine(AppDirectory, "setting.txt");
            }
        }

        /// <summary>
        /// ログファイルパス
        /// </summary>
        private static string LogFilePath
        {
            get
            {
                return Path.Combine(AppDirectory, "log.txt");
            }
        }

        /// <summary>
        /// 壁紙に使える拡張子一覧
        /// </summary>
        private static string[] FileExtensionsUsableInWallpaper
        {
            get
            {
                return new string[] { ".jpg", ".jpeg", ".bmp", ".dib", ".png", ".jfif", ".jpe", ".gif", ".tif", ".tiff", ".wdp" };
            }
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 壁紙を格納しているディレクトリを返します。
        /// 設定されていない場合は、場所の指定をします。
        /// </summary>
        /// <returns></returns>
        private static string GetWallpaperDirectory()
		{
			if (File.Exists(SettingFilePath))
			{
				string[] settingFileData = File.ReadAllLines(SettingFilePath);
				if (settingFileData.Length == 0)
				{					
					//設定ファイルが異常な場合、改めて作成。
					return CreateSettigFile(SettingFilePath);
				}
				else if (!Directory.Exists(settingFileData[0]))
				{
					return CreateSettigFile(SettingFilePath);
				}
				else
				{
					return settingFileData[0];
				}
			}
			else
			{
				return CreateSettigFile(SettingFilePath);
			}
		}

        /// <summary>
        /// フォルダ選択ダイアログを表示し、フォルダの入力を促す。入力されたフォルダパスを設定ファイルに保存する。
        /// </summary>
        /// <param name="settingFilePath"></param>
        /// <returns></returns>
        private static string CreateSettigFile(string settingFilePath)
		{
            var dialog = new CommonOpenFileDialog("フォルダーの選択")
            {
                // 選択形式をフォルダースタイルにする IsFolderPicker プロパティを設定
                IsFolderPicker = true,
                InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}"
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
                string wallpaperDirectory = dialog.FileName;
                using (StreamWriter writer = new StreamWriter(settingFilePath, false))
				{
					writer.WriteLine(wallpaperDirectory);
				}
			    return wallpaperDirectory;
			}
            else
            {
                throw new Exception("フォルダ選択がキャンセルされました。");
            }
		}

        /// <summary>
        /// 壁紙に使えないファイルを除外します。
        /// </summary>
        /// <param name="fileNames">ファイル名を格納している配列</param>
        /// <returns></returns>
        private static string[] ExcludeFileUnusableInWallpaper(string[] fileNames)
        {
            List<string> imageFiles = new List<string>();

            foreach (string item in fileNames)
            {
                bool isUsable = Array.IndexOf(FileExtensionsUsableInWallpaper, Path.GetExtension(item).ToLower()) >= 0;
                if (isUsable)
                {
                    imageFiles.Add(item);
                }
            }

            return imageFiles.ToArray();
        }

        /// <summary>
        /// インデックスを進めます。
        /// 最後のインデックスが指定された場合、0を返します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="length">配列長さ</param>
        /// <returns></returns>
        private static int GetNextIndex(int index, int length)
        {
            index++;
            if (index >= length)
            {
                index = 0;
            }
            return index;
        }
        #endregion
    }
}
