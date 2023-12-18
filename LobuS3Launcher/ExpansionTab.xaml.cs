using Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LobuS3Launcher.Tabs
{
	/// <summary>
	/// Interaction logic for ExpansionTab.xaml
	/// </summary>
	public partial class ExpansionTab : UserControl
	{
		private List<ExpansionControl> ExpansionControls { get; }

		public TabItem? TabItemActions { get; set; } = null;

		public ExpansionTab()
		{
			InitializeComponent();

			ExpansionControls = new List<ExpansionControl>();

			// Expansion Packs.
			ExpansionControl[] expansions = new[]
			{
				CreateExpansionControl("World Adventures",          "The Sims 3 World Adventures"),
				CreateExpansionControl("Ambitions",                 "The Sims 3 Ambitions"),
				CreateExpansionControl("Late Night",                "The Sims 3 Late Night"),
				CreateExpansionControl("Titanfall",                 "The Sims 3 Titanfall"),
				CreateExpansionControl("Generations",               "The Sims 3 Generations"),
				CreateExpansionControl("Pets",                      "The Sims 3 Pets"),
				CreateExpansionControl("Showtime",                  "The Sims 3 Showtime"),
				CreateExpansionControl("Supernatural",              "The Sims 3 Supernatural"),
				CreateExpansionControl("Seasons",                   "The Sims 3 Seasons"),
				CreateExpansionControl("University Life",           "The Sims 3 University Life"),
				CreateExpansionControl("Island Paradise",           "The Sims 3 Island Paradise"),
				CreateExpansionControl("Into the Future",           "The Sims 3 Into The Future"),
			};

			// Stuff Packs.
			ExpansionControl[] stuffs = new[]
			{
				CreateExpansionControl("High-End Loft Stuff",       "The Sims 3 High-End Loft Stuff"),
				CreateExpansionControl("Fast Lane Stuff",           "The Sims 3 Fast Lane Stuff"),
				CreateExpansionControl("Outdoor Living Stuff",      "The Sims 3 Outdoor Living Stuff"),
				CreateExpansionControl("Town Life Stuff",           "The Sims 3 Town Life Stuff"),
				CreateExpansionControl("Master Suite Stuff",        "The Sims 3 Master Suite Stuff"),
				CreateExpansionControl("Katy Perry's Sweet Treats", "The Sims 3 Katy Perry Sweet Treats"),
				CreateExpansionControl("Diesel Stuff",              "The Sims 3 Diesel Stuff"),
				CreateExpansionControl("70s, 80s, &amp; 90s Stuff", "The Sims 3 70s 80s & 90s Stuff"),
				CreateExpansionControl("Movie Stuff",               "The Sims 3 Movie Stuff"),
			};

			ExpansionControls.AddRange(expansions);
			ExpansionControls.AddRange(stuffs);

			// Add expansions to the window.
			foreach (var expansion in expansions)
				EPPanel.Children.Add(expansion);

			foreach (var stuff in stuffs)
				SPPanel.Children.Add(stuff);

			Loaded += ExpansionTab_Loaded;
		}

		private static ExpansionControl CreateExpansionControl(string title, string gameKey)
		{
			return new ExpansionControl(new Expansion(gameKey))
			{
				Title = title,
				Margin = new Thickness(0, 10, 0, 0),
			};
		}

		private void ExpansionTab_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateCheckBoxes();
		}

		private void UpdateCheckBoxes()
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
}
