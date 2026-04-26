using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GameLaunch;

public partial class GameLauncher
{
	private readonly ILogger<GameLauncher> _logger;
	private readonly IEnumerable<IPostLaunchJob> _postLaunchJobs; // postLaunchPipeline?

	public GameLauncher(
		ILogger<GameLauncher> logger,
		IEnumerable<IPostLaunchJob> postLaunchJobs)
	{
		_logger = logger;
		_postLaunchJobs = postLaunchJobs;
	}

	public void Launch(string path)
	{
		logLaunching(path);

		// TODO: Treat Process as the IDisposable it is.
		// We cannot dispose of it before all jobs are finished with it.

		var process = new Process();
		process.StartInfo.FileName = path;
		try
		{
			process.Start();
		}
		catch(Exception e)
		{
			logFailedLaunch(e);
			return;
		}

		foreach (var job in _postLaunchJobs)
			job.Run(process);
	}

	[LoggerMessage(
		Level = LogLevel.Information,
		Message = "Launching game at {path}")]
	private partial void logLaunching(string path);

	[LoggerMessage(
		Level = LogLevel.Error,
		Message = "Failed to start the game process.")]
	private partial void logFailedLaunch(Exception exception);
}
