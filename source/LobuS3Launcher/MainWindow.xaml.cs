using Common;
using GameLaunch;
using LobuS3Launcher.Composition;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace LobuS3Launcher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private readonly GameLauncher _gameLauncher;

	public MainWindow()
	{
		InitializeComponent();

		var serviceProvider = ServiceLocator.Instance.Services;

		_gameLauncher = serviceProvider.GetRequiredService<GameLauncher>();

		Loaded += mainWindow_Loaded;
	}

	private void mainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		expansionTab.TabItemActions = tabItemActions;
		modsTab.TabItemActions = tabItemActions;
	}

	private async void launchButton_Click(object sender, RoutedEventArgs e)
	{
		// Get the path to the base game installation.
		string? binPath = GameDirectory.BaseGamePath;
		if (binPath == null)
		{
			await ErrorDialog.Show("Unable to get the game location from the Windows Registry.");
			return;
		}

		var gamePath = Path.Combine(binPath, getExeName());

		// We don't await the Task because there is no reason to block the UI.
		_ = _gameLauncher.LaunchAsync(path: gamePath);
	}

	private string getExeName()
	{
		if (ExpansionManager.GetSelectionEnabled())
			return GameDirectory.NewGame;
		else
			return GameDirectory.OldGame;
	}
}
