using Common;
using LobuS3Launcher.Composition;
using LobuS3Launcher.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LobuS3Launcher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

		var tabSelector = ServiceLocator
			.Instance
			.Services
			.GetRequiredService<TabSelector>();

		tabSelector.TabControl = tabControl;
	}

	private async void launchButton_Click(object sender, RoutedEventArgs e)
	{
		// Get the path to the base game installation.
		string? baseGamePath = GameDirectory.BaseGamePath;
		if (baseGamePath == null)
		{
			await ErrorDialog.Show("Unable to get the game location from the Windows Registry.");
			return;
		}

		Launcher.Launch(baseGamePath, true);
	}
}
