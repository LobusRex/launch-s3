using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LobuS3Launcher.Tabs;
using Common;

namespace LobuS3Launcher
{
    public class ExpansionPack
	{
		private ExpansionControl Control { get; }
		private Expansion Expansion { get; }

		public ExpansionPack(ExpansionControl control, string gameKey)
		{
			Control = control;
			Expansion = new Expansion(gameKey);

			Control.checkBox.Checked += CheckBox_Checked;
			Control.checkBox.Unchecked += CheckBox_Unchecked;
			Control.discCombo.Selected += Disc_Selected;
			Control.steamCombo.Selected += Steam_Selected;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			Expansion.Select();

			UpdateControls();
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			Expansion.Deselect();

			UpdateControls();
		}

		private void Disc_Selected(object sender, RoutedEventArgs e)
		{
			Expansion.SetPreferredSource(ExpansionSource.Disc);

			UpdateControls();
		}

		private void Steam_Selected(object sender, RoutedEventArgs e)
		{
			Expansion.SetPreferredSource(ExpansionSource.Steam);

			UpdateControls();
		}

		public void UpdateControls()
		{
			Expansion.Update();

			List<ExpansionSource> sources = Expansion.Sources.ToList();

			Control.steamCombo.IsEnabled = sources.Contains(ExpansionSource.Steam);
			Control.discCombo.IsEnabled = sources.Contains(ExpansionSource.Disc);
			Control.IsEnabled = Expansion.IsInstalled;

			SilentCheckUpdate();
		}

		private void SilentCheckUpdate()
		{
			// Update the CheckBox.
			Control.checkBox.Checked -= CheckBox_Checked;
			Control.checkBox.Unchecked -= CheckBox_Unchecked;
			Control.checkBox.IsChecked = Expansion.IsSelected;
			Control.checkBox.Checked += CheckBox_Checked;
			Control.checkBox.Unchecked += CheckBox_Unchecked;

			// Update the Disc ComboBoxItem.
			Control.discCombo.Selected -= Disc_Selected;
			Control.discCombo.IsSelected = Expansion.PreferredSource == ExpansionSource.Disc;
			Control.discCombo.Selected += Disc_Selected;

			// Update the Steam ComboBoxItem.
			Control.steamCombo.Selected -= Steam_Selected;
			Control.steamCombo.IsSelected = Expansion.PreferredSource == ExpansionSource.Steam;
			Control.steamCombo.Selected += Steam_Selected;
		}
	}
}
