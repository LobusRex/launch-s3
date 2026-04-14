namespace Common
{
	public class SavesFolder : Folder
	{
		public SavesFolder(string path) : base(path)
		{ }

		public void BackupTo(Folder target)
		{
			// Make sure the documents directories exist.
			Documents.CreateFolders();

			string backupName = "Backup " + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");

			RecursiveSaveBackup(Location, Path.Combine(target.Location, backupName));
		}

		// Inspired by a question and answer from Stack Overflow.
		// https://stackoverflow.com/q/58744/13798212
		// Asked by Keith https://stackoverflow.com/users/905/keith
		// Answered by Konrad Rudolph https://stackoverflow.com/users/1968/konrad-rudolph
		public static void RecursiveSaveBackup(string sourcePath, string targetPath)
		{
			DirectoryInfo source = new DirectoryInfo(sourcePath);
			DirectoryInfo target = new DirectoryInfo(targetPath);

			// Copy all non backup save directories.
			foreach (DirectoryInfo dir in source.GetDirectories())
			{
				if (dir.Extension == ".backup")
					continue;

				RecursiveSaveBackup(dir.FullName, target.CreateSubdirectory(dir.Name).FullName);
			}

			// Copy all files within the save directory.
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target.FullName, file.Name));
		}

		internal void CreateFolders()
		{
			// Create this directory.
			Directory.CreateDirectory(Location);
		}
	}
}
