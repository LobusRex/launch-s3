namespace Common
{
	public class DocumentsFolder : Folder
	{
		public ModsFolder Mods { get; }
		public SavesFolder Saves { get; }

		public DocumentsFolder(string path) : base(path)
		{
			Mods = new ModsFolder(Path.Combine(Location, "Mods"));
			Saves = new SavesFolder(Path.Combine(Location, "Saves"));
		}

		internal void CreateFolders()
		{
			// Create this directory.
			Directory.CreateDirectory(Location);

			// Create subdirectories.
			Mods.CreateFolders();
			Saves.CreateFolders();
		}
	}
}
