using Common;
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

			Loaded += ActionsTab_Loaded;
		}

		private void ActionsTab_Loaded(object sender, RoutedEventArgs e)
		{
			activeSavesButton.IsEnabled = Directory.Exists(Documents.Game.Location);
			backupSavesButton.IsEnabled = Directory.Exists(Documents.Launcher.Location);
		}

		private void EnableEPButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				EPSelectionManager.SetSelectionEnabled(true, true, true);
			}
			catch (RegistryKeyNotFoundException)
			{
				ErrorBox.Show("Unable to get the game location from the Windows Registry.");
			}
		}

		private void DisableEPButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				EPSelectionManager.SetSelectionEnabled(false, true, true);
			}
			catch (RegistryKeyNotFoundException)
			{
				ErrorBox.Show("Unable to get the game location from the Windows Registry.");
			}
		}

		private void BackupButton_Click(object sender, RoutedEventArgs e)
		{
			// Ask the user if they are sure.
			string message = "Do you want to make a backup of all saves? Make sure that the disk has enough space to fit the backup.";
			string caption = "Create backup";
			MessageBoxImage icon = MessageBoxImage.Question;
			MessageBoxButton button = MessageBoxButton.YesNoCancel;
			bool doBackup = MessageBox.Show(message, caption, button, icon).Equals(MessageBoxResult.Yes);

			if (!doBackup)
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

		private void EnableModsButton_Click(object sender, RoutedEventArgs e)
		{
			// Ask the user if they are sure.
			string message = "Do you want to set up modding? Doing so requires an internet connection to download FrameworkSetup.zip from modthesims.info.";
			string caption = "Set up modding";
			MessageBoxImage icon = MessageBoxImage.Question;
			MessageBoxButton button = MessageBoxButton.YesNoCancel;
			bool doSetup = MessageBox.Show(message, caption, button, icon).Equals(MessageBoxResult.Yes);

			if (!doSetup)
				return;

			// Download and extract FrameworkSetup.zip.
			Task<ModManager.DownloadExtractResult> download = ModManager.EnableSelection();

			// Print error messages if anything fails.
			Task temp = download.ContinueWith(t =>
			{
				// The download failed.
				if (t.Result == ModManager.DownloadExtractResult.DownloadFailed)
					ErrorBox.Show($"Unable to download FrameworkSetup.zip from {ModManager.FrameworkSetupUrl}.");

				// The extraction failed.
				if (t.Result == ModManager.DownloadExtractResult.ExtractionFailed)
					ErrorBox.Show($"Unable to extract FrameworkSetup.zip to {Documents.Game.Location}.");
			});
		}
	}
}
