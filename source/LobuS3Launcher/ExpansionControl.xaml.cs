using Common;
using LaunchS3.Expansions;
using LaunchS3.Expansions.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LobuS3Launcher.Tabs;

/// <summary>
/// Interaction logic for ExpansionControl.xaml
/// </summary>
public partial class ExpansionControl : UserControl
{
	private readonly Expansion _expansion;
	private readonly IExpansionService _expansionService;
	private readonly ExpansionKey _expansionKey;

	private static readonly DependencyProperty _titleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ExpansionControl));

	private string Title
	{
		get { return (string)GetValue(_titleProperty); }
		set { SetValue(_titleProperty, value); }
	}

	public ExpansionControl(ExpansionItem expansionItem, IExpansionService expansionService)
	{
		InitializeComponent();

		DataContext = this;

		_expansion = new Expansion(expansionItem.Key);
		
		Title = expansionItem.Name;
		_expansionKey = new ExpansionKey(expansionItem.Key);
		_expansionService = expansionService;

		checkBox.Checked += checkBox_Checked;
		checkBox.Unchecked += checkBox_Unchecked;
		discCombo.Selected += disc_Selected;
		steamCombo.Selected += steam_Selected;
	}

	public void UpdateControls()
	{
		_expansion.Update();

		var sources = _expansion.Sources.ToList();

		steamCombo.IsEnabled = sources.Contains(ExpansionSource.Steam);
		discCombo.IsEnabled = sources.Contains(ExpansionSource.Disc);
		IsEnabled = _expansion.IsInstalled;

		silentCheckUpdate();
	}

	private void checkBox_Checked(object sender, RoutedEventArgs e)
	{
		_expansion.Select();

		UpdateControls();
	}

	private void checkBox_Unchecked(object sender, RoutedEventArgs e)
	{
		_expansionService.Deselect(_expansionKey);

		UpdateControls();
	}

	private void disc_Selected(object sender, RoutedEventArgs e)
	{
		_expansion.SetPreferredSource(ExpansionSource.Disc);

		UpdateControls();
	}

	private void steam_Selected(object sender, RoutedEventArgs e)
	{
		_expansion.SetPreferredSource(ExpansionSource.Steam);

		UpdateControls();
	}

	private void silentCheckUpdate()
	{
		// Update the CheckBox.
		checkBox.Checked -= checkBox_Checked;
		checkBox.Unchecked -= checkBox_Unchecked;
		checkBox.IsChecked = _expansion.IsSelected;
		checkBox.Checked += checkBox_Checked;
		checkBox.Unchecked += checkBox_Unchecked;

		// Update the Disc ComboBoxItem.
		discCombo.Selected -= disc_Selected;
		discCombo.IsSelected = _expansion.PreferredSource == ExpansionSource.Disc;
		discCombo.Selected += disc_Selected;

		// Update the Steam ComboBoxItem.
		steamCombo.Selected -= steam_Selected;
		steamCombo.IsSelected = _expansion.PreferredSource == ExpansionSource.Steam;
		steamCombo.Selected += steam_Selected;
	}
}
