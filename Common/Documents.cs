namespace Common
{
	public static class Documents
	{
		public static DocumentsFolder Launcher { get; }
		public static DocumentsFolder Game { get; }

		static Documents()
		{
			string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			Launcher = new DocumentsFolder(Path.Combine(documents, "Launch S3"));
			Game = new DocumentsFolder(Path.Combine(documents, @"Electronic Arts\The Sims 3"));
		}
	}
}
