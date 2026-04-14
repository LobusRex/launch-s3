using System.Threading.Tasks;
using ModernWpf.Controls;

namespace LobuS3Launcher
{
	public static class ErrorDialog
	{
		/// <summary>
		/// Show a ContentDialog, styled for errors.
		/// </summary>
		/// <param name="message">The error message to display.</param>
		public static async Task Show(string message)
		{
			ContentDialog dialog = new ContentDialog()
			{
				Title = "Error",
				Content = message,
				CloseButtonText = "OK",
			};

			await dialog.ShowAsync();
		}
	}
}
