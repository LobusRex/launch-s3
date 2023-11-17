using Common;
using ModernWpf.Controls;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LobuS3Launcher.Tabs
{
	/// <summary>
	/// Interaction logic for ActionsTab.xaml
	/// </summary>
	public partial class ActionsTab : UserControl
	{
		public ActionsTab()
		{
			InitializeComponent();
		}

		private async void EnableEPButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				EPSelectionManager.SetSelectionEnabled(true, true, true);
			}
			catch (RegistryKeyNotFoundException)
			{
				await ErrorDialog.Show("Unable to get the game location from the Windows Registry.");
			}
		}

		private async void DisableEPButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				EPSelectionManager.SetSelectionEnabled(false, true, true);
			}
			catch (RegistryKeyNotFoundException)
			{
				await ErrorDialog.Show("Unable to get the game location from the Windows Registry.");
			}
		}

		private async void BackupButton_Click(object sender, RoutedEventArgs e)
		{
			// Ask the user if they are sure.
			ContentDialog dialog = new ContentDialog()
			{
				Title = "Create backup",
				Content = "Do you want to make a backup of all saves? Make sure that the disk has enough space to fit the backup.",
				CloseButtonText = "No",
				PrimaryButtonText = "Yes",
			};
			ContentDialogResult dialogResult = await dialog.ShowAsync();
			
			if (dialogResult != ContentDialogResult.Primary)
				return;

			// Make the backup.
			Documents.Game.Saves.BackupTo(Documents.Launcher.Saves);
		}

		private void BackupSavesButton_Click(object sender, RoutedEventArgs e)
		{
			Documents.Launcher.Saves.OpenWithExplorer();
		}

		private void ActiveSavesButton_Click(object sender, RoutedEventArgs e)
		{
			Documents.Game.Saves.OpenWithExplorer();
		}

		private async void EnableModsButton_Click(object sender, RoutedEventArgs e)
		{
			// Ask the user if they are sure.
			ContentDialog dialog = new ContentDialog()
			{
				Title = "Set up modding",
				Content = "Do you want to set up modding? Doing so requires an internet connection to download FrameworkSetup.zip from modthesims.info.",
				PrimaryButtonText = "Yes",
				CloseButtonText = "No",
			};
			ContentDialogResult dialogResult = await dialog.ShowAsync();

			if (dialogResult != ContentDialogResult.Primary)
				return;

			// Download and extract FrameworkSetup.zip.
			ModManager.DownloadExtractResult result = await ModManager.EnableSelection();

			// Print error message if the download failed.
			if (result == ModManager.DownloadExtractResult.DownloadFailed)
				await ErrorDialog.Show($"Unable to download FrameworkSetup.zip from {ModManager.FrameworkSetupUrl}.");

			// Print error message if the extraction failed.
			if (result == ModManager.DownloadExtractResult.ExtractionFailed)
				await ErrorDialog.Show($"Unable to extract FrameworkSetup.zip to {Documents.Game.Location}.");
		}
	}
}
