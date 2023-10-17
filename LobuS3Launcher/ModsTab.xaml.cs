using Common;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LobuS3Launcher.Tabs
{
    /// <summary>
    /// Interaction logic for ModsTab.xaml
    /// </summary>
    public partial class ModsTab : UserControl
    {
		public TabItem? TabItemActions { get; set; } = null;

		public ModsTab()
        {
            InitializeComponent();

			Loaded += ModsTab_Loaded;
        }

		private void ModsTab_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateCheckBoxes();

			addOverrideButton.Click += (sender, e) => AddModButton_Click(sender, e, ModManager.Overrides);
			addPackageButton.Click += (sender, e) => AddModButton_Click(sender, e, ModManager.Packages);
		}

		private void UpdateCheckBoxes()
		{
			// Get all available mods.
			Dictionary<string, bool> overrides = ModManager.Overrides.GetAvailableAndSelected();
			Dictionary<string, bool> packages = ModManager.Packages.GetAvailableAndSelected();

			List <CheckBox> overrideBoxes = CreateCheckBoxes(overrides, ModManager.Overrides);
			List<CheckBox> packageBoxes = CreateCheckBoxes(packages, ModManager.Packages);

			// Add the new CheckBoxes to the window.
			overridesContainer.Children.Clear();
			foreach (CheckBox checkBox in overrideBoxes)
				overridesContainer.Children.Add(checkBox);

			packagesContainer.Children.Clear();
			foreach (CheckBox checkBox in packageBoxes)
				packagesContainer.Children.Add(checkBox);
		}

		private List<CheckBox> CreateCheckBoxes(Dictionary<string, bool> mods, ModSelector modSelector)
		{
			// Create a CheckBox for each mod.
			List<CheckBox> checkBoxes = new List<CheckBox>();
			foreach (KeyValuePair<string, bool> mod in mods)
			{
				checkBoxes.Add(new CheckBox
				{
					Content = mod.Key,
					IsChecked = mod.Value,
				});
			}

			// Sort the list by name.
			checkBoxes = checkBoxes.OrderBy(x => x.Content).ToList();

			// Add CheckBox events.
			foreach (CheckBox checkBox in checkBoxes)
			{
				checkBox.Checked += (sender, e) => CheckBox_Checked(sender, e, modSelector);
				checkBox.Unchecked += (sender, e) => CheckBox_Unchecked(sender, e, modSelector);
			}

			// Add a context menu to all CheckBoxes.
			foreach (CheckBox checkBox in checkBoxes)
			{
				ContextMenu contextMenu = new ContextMenu();
				checkBox.ContextMenu = contextMenu;

				MenuItem menuDelete = new MenuItem
				{
					Header = "Remove",
					Icon = new SymbolIcon(Symbol.Delete),
					CommandParameter = checkBox,
				};
				menuDelete.Click += (sender, e) => MenuDelete_Click(sender, e, modSelector);
				contextMenu.Items.Add(menuDelete);
			}

			return checkBoxes;
		}

		private void MenuDelete_Click(object sender, RoutedEventArgs e, ModSelector modSelector)
		{
			if (sender is not MenuItem menuItem)
				return;

			if (menuItem.CommandParameter is not CheckBox checkBox)
				return;

			if (checkBox.Content is not string name)
				return;

			// Delete the mod.
			modSelector.Delete(name);

			// Update the tab.
			UpdateCheckBoxes();
		}

		private void AddModButton_Click(object sender, RoutedEventArgs e, ModSelector modSelector)
		{
			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.DefaultExt = ".package";
			dialog.Filter = "Package mods |*.package";

			bool? result = dialog.ShowDialog();

			if (result == true)
			{
				string filename = dialog.FileName;

				modSelector.Add(filename);

				UpdateCheckBoxes();
			}
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e, ModSelector modSelector)
		{
			// Get the mod name.
			CheckBox checkBox = (CheckBox)sender;
			string name = (string)checkBox.Content;

			// Deselect the mod.
			modSelector.Deselect(name);

			// Update the tab.
			UpdateCheckBoxes();
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e, ModSelector mods)
		{
			// Get the mod name.
			CheckBox checkBox = (CheckBox)sender;
			string name = (string)checkBox.Content;

			// Select the mod.
			mods.Select(name);

			// Update the tab.
			UpdateCheckBoxes();
		}

		private void Hyperlink_Click(object sender, RoutedEventArgs e)
		{
			if (TabItemActions == null)
				return;

			try
			{
				TabItem tabItem = (TabItem)Parent;
				TabControl tabControl = (TabControl)tabItem.Parent;

				Dispatcher.BeginInvoke((Action)(() => tabControl.SelectedItem = TabItemActions));
			}
			catch { return; }
		}
	}
}
