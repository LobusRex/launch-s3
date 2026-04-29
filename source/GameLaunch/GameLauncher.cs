using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GameLaunch;

public partial class GameLauncher
{
	private readonly ILogger<GameLauncher> _logger;
	private readonly IEnumerable<IPostLaunchJob> _postLaunchJobs;

	public GameLauncher(
		ILogger<GameLauncher> logger,
		IEnumerable<IPostLaunchJob> postLaunchJobs)
	{
		_logger = logger;
		_postLaunchJobs = postLaunchJobs;
	}

	public async Task LaunchAsync(string path)
	{
		logLaunching(path);

		using var process = new Process();
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

		var jobTasks = _postLaunchJobs.Select(j => j.RunAsync(process));

		await Task.WhenAll(jobTasks).ConfigureAwait(false);
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
