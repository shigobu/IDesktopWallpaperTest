using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace ChangeWallpaper
{
	static class Program
	{
        /// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
            try
            {
			    string wallpaperDirectory = GetWallpaperDirectory();
                if (string.IsNullOrWhiteSpace(wallpaperDirectory))
                {
                    throw new Exception("壁紙格納フォルダの取得に失敗しました。");
                }
                MessageBox.Show(wallpaperDirectory);

                //正常終了したらログファイルを削除する。
                if (File.Exists(LogFilePath))
                {
                    File.Delete(LogFilePath);
                }
            }
            catch (Exception ex)
            {
                List<string> logMessage = new List<string>();
                logMessage.Add(DateTime.Now.ToLongDateString());
                logMessage.Add(DateTime.Now.ToLongTimeString());
                logMessage.Add(ex.Message);
                File.WriteAllLines(LogFilePath, logMessage.ToArray());                
            }
		}

        #region プロパティ
        static string _appPath = null;
        /// <summary>
        /// 自分のexeフルパス
        /// </summary>
        static string AppPath
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
        static string AppDirectory
        {
            get
            {
                return Path.GetDirectoryName(AppPath);
            }
        }

        /// <summary>
        /// 設定ファイルのパス
        /// </summary>
        static string SettingFilePath
        {
            get
            {
                return Path.Combine(AppDirectory, "setting.txt");
            }
        }

        /// <summary>
        /// ログファイルパス
        /// </summary>
        static string LogFilePath
        {
            get
            {
                return Path.Combine(AppDirectory, "log.txt");
            }
        }
        #endregion

        /// <summary>
        /// 壁紙を格納しているディレクトリを返します。
        /// 設定されていない場合は、場所の指定をします。
        /// </summary>
        /// <returns></returns>
        static string GetWallpaperDirectory()
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
		static string CreateSettigFile(string settingFilePath)
		{
            var dialog = new CommonOpenFileDialog("フォルダーの選択")
            {
                // 選択形式をフォルダースタイルにする IsFolderPicker プロパティを設定
                IsFolderPicker = true,
                InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}"
            };

            string wallpaperDirectory = null;
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				wallpaperDirectory = dialog.FileName;
				using (StreamWriter writer = new StreamWriter(settingFilePath, false))
				{
					writer.WriteLine(wallpaperDirectory);
				}
			}

			return wallpaperDirectory;
		}
	}
}
