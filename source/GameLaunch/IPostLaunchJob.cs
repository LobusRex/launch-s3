using System.Diagnostics;

namespace GameLaunch;

public interface IPostLaunchJob
{
	public Task RunAsync(Process process);
}
