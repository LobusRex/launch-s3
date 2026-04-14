using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace LobuS3Launcher.Navigation;

internal static class TabSelector
{
	private static readonly Dictionary<Type, TabItem> _tabs = [];
	
	public static TabControl? TabControl { get; set; }

	public static void Register<TTab>(TabItem tabItem) where TTab : UserControl
	{
		_tabs[typeof(TTab)] = tabItem;
	}

	public static void NavigateTo<TTab>() where TTab : UserControl
	{
		if (TabControl is null)
			return;

		var tabItem = _tabs[typeof(TTab)];
		
		if (TabControl.Items.Contains(tabItem))
			changeTabTo(tabItem);
	}

	private static void changeTabTo(TabItem tabItem)
	{
		TabControl?.SelectedItem = tabItem;
	}
}
