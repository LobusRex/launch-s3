using System.Diagnostics;

namespace GameLaunch;

public interface IPostLaunchJob
{
	public void Run(Process process);
}
