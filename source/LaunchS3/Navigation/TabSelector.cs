using System.Linq;
using System.Windows.Controls;

namespace LaunchS3.Navigation;

internal class TabSelector
{
	public TabControl? TabControl { get; set; }

	public void NavigateTo<TTab>() where TTab : UserControl
	{
		if (TabControl is null)
			return;

		var tab = TabControl
			.Items
			.OfType<TabItem>()
			.Select(i => new { Content=i.Content, Item=i })
			.FirstOrDefault(i => i.Content is TTab);

		if (tab is not null)
			TabControl?.SelectedItem = tab.Item;
	}
}
