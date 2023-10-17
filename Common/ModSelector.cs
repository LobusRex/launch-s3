namespace Common
{
	public class ModSelector
	{
		public Folder Game { get; }
		public Folder Launcher { get; }

		private static string Extension { get; } = ".package";

		public ModSelector(Folder gameFolder, Folder launcherFolder)
		{
			Game = gameFolder;
			Launcher = launcherFolder;
		}

		public Dictionary<string, bool> GetAvailableAndSelected()
		{
			Dictionary<string, bool> mods = new Dictionary<string, bool>();

			// Get the mods from the games folder.
			DirectoryInfo gameModsFolder = new DirectoryInfo(Game.Location);
			foreach (FileInfo file in gameModsFolder.GetFiles("*.package"))
			{
				string name = Path.GetFileNameWithoutExtension(file.Name);

				// Add the mod to the dictionary.
				// This mod is selected.
				mods.Add(name, true);
			}

			// Get additional mods from the launcher folder.
			DirectoryInfo launcherModsFolder = new DirectoryInfo(Launcher.Location);
			foreach (FileInfo file in launcherModsFolder.GetFiles("*.package"))
			{
				string name = Path.GetFileNameWithoutExtension(file.Name);

				// Add the mod if it is not already in the dictionary.
				// This mod is not selected.
				mods.TryAdd(name, false);
			}

			return mods;
		}

		public void Select(string name)
		{
			string packageName = name + Extension;

			// Move the mod to the game mods folder.
			Launcher.MoveFile(Game, packageName);
		}

		public void Deselect(string name)
		{
			string packageName = name + Extension;

			// Move the mod to the launcher mods folder.
			Game.MoveFile(Launcher, packageName);
		}

		public void Add(string packagePath)
		{
			FileInfo file = new FileInfo(packagePath);

			if (file.DirectoryName == null)
				return;

			// Copy the mod to the game mods folder.
			Game.CopyFileFrom(file.DirectoryName, file.Name);
		}

		public void Delete(string name)
		{
			string packageName = name + Extension;

			// Delete the mod from both/either mod folder.
			Launcher.DeleteFile(packageName);
			Game.DeleteFile(packageName);
		}
	}
}
