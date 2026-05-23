using Common;
using LaunchS3.Expansions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LobuS3Launcher.Tabs;

/// <summary>
/// Interaction logic for ExpansionControl.xaml
/// </summary>
public partial class ExpansionControl : UserControl
{
	private Expansion Expansion { get; }

	public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ExpansionControl));
	
	private readonly IExpansionService _expansionService;
	private readonly ExpansionKey _expansionKey;

	public string Title
	{
		get { return (string)GetValue(TitleProperty); }
		set { SetValue(TitleProperty, value); }
	}

	public ExpansionControl(Expansion expansion, IExpansionService expansionService)
	{
		InitializeComponent();

		DataContext = this;

		Expansion = expansion;
		_expansionService = expansionService;
		_expansionKey = new ExpansionKey(expansion.GameKey);

		checkBox.Checked += CheckBox_Checked;
		checkBox.Unchecked += checkBox_Unchecked;
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

	private void checkBox_Unchecked(object sender, RoutedEventArgs e)
	{
		_expansionService.Deselect(_expansionKey);

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
		checkBox.Unchecked -= checkBox_Unchecked;
		checkBox.IsChecked = Expansion.IsSelected;
		checkBox.Checked += CheckBox_Checked;
		checkBox.Unchecked += checkBox_Unchecked;

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
