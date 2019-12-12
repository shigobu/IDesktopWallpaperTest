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
			string wallpaperDirectory = GetWallpaperDirectory();
			MessageBox.Show(wallpaperDirectory);
		}

		/// <summary>
		/// 壁紙を格納しているディレクトリを返します。
		/// 設定されていない場合は、場所の指定をします。
		/// </summary>
		/// <returns></returns>
		static string GetWallpaperDirectory()
		{
			string appPath = Assembly.GetExecutingAssembly().Location;
			string appDirectory = Path.GetDirectoryName(appPath);
			string settingFilePath = Path.Combine(appDirectory, "setting.txt");

			if (File.Exists(settingFilePath))
			{
				string[] settingFileData = File.ReadAllLines(settingFilePath);
				if (settingFileData.Length == 0)
				{					
					//設定ファイルが異常な場合、改めて作成。
					return CreateSettigFile(settingFilePath);
				}
				else if (!Directory.Exists(settingFileData[0]))
				{
					return CreateSettigFile(settingFilePath);
				}
				else
				{
					return settingFileData[0];
				}
			}
			else
			{
				return CreateSettigFile(settingFilePath);
			}
		}

		/// <summary>
		/// フォルダ選択ダイアログを表示し、フォルダの入力を促す。入力されたフォルダパスを設定ファイルに保存する。
		/// </summary>
		/// <param name="settingFilePath"></param>
		/// <returns></returns>
		static string CreateSettigFile(string settingFilePath)
		{
			var dialog = new CommonOpenFileDialog("フォルダーの選択");

			// 選択形式をフォルダースタイルにする IsFolderPicker プロパティを設定
			dialog.IsFolderPicker = true;

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
