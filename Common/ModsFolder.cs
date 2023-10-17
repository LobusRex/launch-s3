namespace Common
{
	public class ModsFolder : Folder
	{
		public Folder Overrides { get; }
		public Folder Packages { get; }

		public ModsFolder(string path) : base(path)
		{
			Overrides = new Folder(Path.Combine(path, "Overrides"));
			Packages = new Folder(Path.Combine(path, "Packages"));
		}
	}
}
