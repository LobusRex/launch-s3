using Common;
using LaunchS3.Expansions;
using LaunchS3.Expansions.Configuration;
using LobuS3Launcher.Composition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LobuS3Launcher.Tabs;

/// <summary>
/// Interaction logic for ExpansionTab.xaml
/// </summary>
public partial class ExpansionTab : UserControl
{
	private readonly IExpansionService _expansionService;

	private IEnumerable<ExpansionControl> ExpansionControls => [
		.. EPPanel.Children.OfType<ExpansionControl>(),
		.. SPPanel.Children.OfType<ExpansionControl>()
		];

	public TabItem? TabItemActions { get; set; } = null;

	public ExpansionTab()
	{
		InitializeComponent();

		var expansions = ServiceLocator
			.Instance
			.Services
			.GetRequiredService<IOptions<ExpansionsSection>>()
			.Value;

		_expansionService = ServiceLocator
			.Instance
			.Services
			.GetRequiredService<IExpansionService>();

		// This should probably be in ExpansionTab_Loaded.
		// For now, it is left here because it caused a noticable delay when switching tabs.
		addExpansions(expansions);

		Loaded += ExpansionTab_Loaded;
	}

	private void addExpansions(ExpansionsSection expansions)
	{
		var expansionControls = expansions
			.ExpansionPacks
			.Select(createExpansionControl);

		foreach (var expansion in expansionControls)
			EPPanel.Children.Add(expansion);

		var stuffControls = expansions
			.StuffPacks
			.Select(createExpansionControl);

		foreach (var stuff in stuffControls)
			SPPanel.Children.Add(stuff);
	}

	private ExpansionControl createExpansionControl(ExpansionItem expansion)
	{
		return new ExpansionControl(expansion, _expansionService)
		{
			Margin = new Thickness(0, 10, 0, 0),
		};
	}

	private void ExpansionTab_Loaded(object sender, RoutedEventArgs e)
	{
		updateCheckBoxes();
	}

	private void updateCheckBoxes()
	{
		bool selectionEnabled = ExpansionManager.GetSelectionEnabled();

		// Hide expansions if selection is disabled.
		if (!selectionEnabled)
		{
			EPBox.Visibility = Visibility.Collapsed;
			SPBox.Visibility = Visibility.Collapsed;
			return;
		}

		EPBox.Visibility = Visibility.Visible;
		SPBox.Visibility = Visibility.Visible;

		foreach (var expansion in ExpansionControls)
			expansion.UpdateControls();
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
