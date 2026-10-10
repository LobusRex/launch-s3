using Microsoft.Win32;

namespace LaunchS3.Expansions;

public class ExpansionService : IExpansionService
{
	public void Deselect(ExpansionKey expansionKey)
	{
		var key = Path.Combine(@"SOFTWARE\WOW6432Node", "SimL", expansionKey.Value);

		Registry.LocalMachine.DeleteSubKeyTree(
			subkey: key,
			throwOnMissingSubKey: false);
	}
}
