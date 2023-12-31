﻿using Common;
using System.Windows;

namespace LobuS3Launcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			expansionTab.TabItemActions = tabItemActions;
			modsTab.TabItemActions = tabItemActions;
		}

		private async void LaunchButton_Click(object sender, RoutedEventArgs e)
		{
			// Get the path to the base game installation.
			string? baseGamePath = GameDirectory.BaseGamePath;
			if (baseGamePath == null)
			{
				await ErrorDialog.Show("Unable to get the game location from the Windows Registry.");
				return;
			}

			// Launch the game.
			Launcher.Launch(baseGamePath, true);
		}
    }
}
