using Common;
using LobuS3Launcher.Composition;
using LobuS3Launcher.ExpansionConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using LobuS3Launcher.Navigation;
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
	private IEnumerable<ExpansionControl> ExpansionControls => [
		.. EPPanel.Children.OfType<ExpansionControl>(),
		.. SPPanel.Children.OfType<ExpansionControl>()
		];

	private readonly TabSelector _tabSelector;

	public ExpansionTab()
	{
		InitializeComponent();

		var serviceProvider = ServiceLocator.Instance.Services;

		var expansions = serviceProvider
			.GetRequiredService<IOptions<ExpansionsSection>>()
			.Value;

		_tabSelector = serviceProvider
			.GetRequiredService<TabSelector>();

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

	private static ExpansionControl createExpansionControl(string title, string gameKey)
	{
		return new ExpansionControl(new Expansion(gameKey))
		{
			Title = title,
			Margin = new Thickness(0, 10, 0, 0),
		};
	}

	private ExpansionControl createExpansionControl(ExpansionItem expansion)
	{
		return createExpansionControl(
			title: expansion.Name,
			gameKey: expansion.Key);
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

	private void hyperlink_Click(object sender, RoutedEventArgs e)
	{
		_tabSelector.NavigateTo<ActionsTab>();
	}
}
