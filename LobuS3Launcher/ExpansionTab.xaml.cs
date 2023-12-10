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
		private List<ExpansionPack> ExpansionPacks { get; }

		public TabItem? TabItemActions { get; set; } = null;

		public ExpansionTab()
		{
			InitializeComponent();

			ExpansionPacks = new List<ExpansionPack>()
			{
				// Expansion Packs
				new(EP01Expansion, "The Sims 3 World Adventures"),
				new(EP02Expansion, "The Sims 3 Ambitions"),
				new(EP03Expansion, "The Sims 3 Late Night"),
				new(EP04Expansion, "The Sims 3 Generations"),
				new(EP05Expansion, "The Sims 3 Pets"),
				new(EP06Expansion, "The Sims 3 Showtime"),
				new(EP07Expansion, "The Sims 3 Supernatural"),
				new(EP08Expansion, "The Sims 3 Seasons"),
				new(EP09Expansion, "The Sims 3 University Life"),
				new(EP10Expansion, "The Sims 3 Island Paradise"),
				new(EP11Expansion, "The Sims 3 Into The Future"),

				// Stuff Packs
				new(SP01Expansion, "The Sims 3 High-End Loft Stuff"),
				new(SP02Expansion, "The Sims 3 Fast Lane Stuff"),
				new(SP03Expansion, "The Sims 3 Outdoor Living Stuff"),
				new(SP04Expansion, "The Sims 3 Town Life Stuff"),
				new(SP05Expansion, "The Sims 3 Master Suite Stuff"),
				new(SP06Expansion, "The Sims 3 Katy Perry Sweet Treats"),
				new(SP07Expansion, "The Sims 3 Diesel Stuff"),
				new(SP08Expansion, "The Sims 3 70s 80s & 90s Stuff"),
				new(SP09Expansion, "The Sims 3 Movie Stuff"),
			};

			Loaded += ExpansionTab_Loaded;
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

			foreach (var expansion in ExpansionPacks)
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
