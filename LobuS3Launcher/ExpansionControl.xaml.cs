using System.Windows;
using System.Windows.Controls;

namespace LobuS3Launcher.Tabs
{
	/// <summary>
	/// Interaction logic for ExpansionControl.xaml
	/// </summary>
	public partial class ExpansionControl : UserControl
	{
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ExpansionControl));

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public ExpansionControl()
		{
			InitializeComponent();

			DataContext = this;
		}
	}
}
