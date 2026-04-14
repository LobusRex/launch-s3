using Common;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LobuS3Launcher.Tabs
{
	/// <summary>
	/// Interaction logic for ExpansionControl.xaml
	/// </summary>
	public partial class ExpansionControl : UserControl
	{
		private Expansion Expansion { get; }

		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ExpansionControl));

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public ExpansionControl(Expansion expansion)
		{
			InitializeComponent();

			DataContext = this;

			Expansion = expansion;

			checkBox.Checked += CheckBox_Checked;
			checkBox.Unchecked += CheckBox_Unchecked;
			discCombo.Selected += Disc_Selected;
			steamCombo.Selected += Steam_Selected;
		}

		public void UpdateControls()
		{
			Expansion.Update();

			List<ExpansionSource> sources = Expansion.Sources.ToList();

			steamCombo.IsEnabled = sources.Contains(ExpansionSource.Steam);
			discCombo.IsEnabled = sources.Contains(ExpansionSource.Disc);
			IsEnabled = Expansion.IsInstalled;

			SilentCheckUpdate();
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

		private void SilentCheckUpdate()
		{
			// Update the CheckBox.
			checkBox.Checked -= CheckBox_Checked;
			checkBox.Unchecked -= CheckBox_Unchecked;
			checkBox.IsChecked = Expansion.IsSelected;
			checkBox.Checked += CheckBox_Checked;
			checkBox.Unchecked += CheckBox_Unchecked;

			// Update the Disc ComboBoxItem.
			discCombo.Selected -= Disc_Selected;
			discCombo.IsSelected = Expansion.PreferredSource == ExpansionSource.Disc;
			discCombo.Selected += Disc_Selected;

			// Update the Steam ComboBoxItem.
			steamCombo.Selected -= Steam_Selected;
			steamCombo.IsSelected = Expansion.PreferredSource == ExpansionSource.Steam;
			steamCombo.Selected += Steam_Selected;
		}
	}
}
