using System.IO.Compression;

namespace Common
{
	public static class ModManager
	{
		public static string FrameworkSetupUrl { get; set; } = "https://chii.modthesims.info/FrameworkSetup.zip";

		public static ModSelector Overrides { get; }
		public static ModSelector Packages { get; }

		static ModManager()
		{
			Folder gameOverrides = Documents.Game.Mods.Overrides;
			Folder launcherOverrides = Documents.Launcher.Mods.Overrides;
			Overrides = new ModSelector(gameOverrides, launcherOverrides);

			Folder gamePackages = Documents.Game.Mods.Packages;
			Folder launcherPackages = Documents.Launcher.Mods.Packages;
			Packages = new ModSelector(gamePackages, launcherPackages);
		}

		public enum DownloadExtractResult
		{
			Succeeded,
			DownloadFailed,
			ExtractionFailed,
		}

		public static Task<DownloadExtractResult> EnableSelection()
		{
			return DownloadFrameworkSetup();
		}

		private static async Task<DownloadExtractResult> DownloadFrameworkSetup()
		{
			try
			{
				// Download FrameworkSetup.zip.
				using HttpClient HttpClient = new HttpClient();
				using Stream data = await HttpClient.GetStreamAsync(FrameworkSetupUrl);

				// Extract the archive to the game documents folder.
				ZipArchive archive = new ZipArchive(data, ZipArchiveMode.Read);
				archive.ExtractToDirectory(Documents.Game.Location, true);
			}
			catch (Exception e)
			{
				if (e is InvalidOperationException ||
					e is HttpRequestException ||
					e is TaskCanceledException)
				{
					// The Download failed.
					return DownloadExtractResult.DownloadFailed;
				}
				else
				{
					// The extraction failed.
					return DownloadExtractResult.ExtractionFailed;
				}
			}

			// The set up was successfull.
			return DownloadExtractResult.Succeeded;
		}
	}
}
