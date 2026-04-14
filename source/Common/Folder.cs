using System.Diagnostics;

namespace Common
{
	public class Folder
	{
		public string Location { get; }

		public Folder(string path)
		{
			Location = path;
		}

		public void MoveFile(Folder to, string name)
		{
			// Make sure the documents directories exist.
			Documents.CreateFolders();

			string fromPath = Path.Combine(Location, name);
			string toPath = Path.Combine(to.Location, name);

			try
			{
				File.Move(fromPath, toPath, true);
			}
			catch { }
		}

		public void CopyFileFrom(string from, string name)
		{
			// Make sure the documents directories exist.
			Documents.CreateFolders();

			string fromPath = Path.Combine(from, name);
			string toPath = Path.Combine(Location, name);

			try
			{
				File.Copy(fromPath, toPath, true);
			}
			catch { }
		}

		public void DeleteFile(string name)
		{
			// Make sure the documents directories exist.
			// This is probably not needed.
			Documents.CreateFolders();

			string path = Path.Combine(Location, name);

			try
			{
				File.Delete(path);
			}
			catch { }
		}

		public void OpenWithExplorer()
		{
			// Make sure the documents directories exist.
			Documents.CreateFolders();

			// Create an Explorer process.
			Process process = new Process();
			process.StartInfo.FileName = "explorer.exe";
			process.StartInfo.ArgumentList.Add(Location);

			// Start the process.
			try
			{
				process.Start();
			}
			catch { }
		}
	}
}
