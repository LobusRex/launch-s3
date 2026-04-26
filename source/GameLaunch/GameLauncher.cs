using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GameLaunch;

public interface IPostLaunchJob
{
	public void Run(Process process);
}

public class QuitAfter10 : IPostLaunchJob
{
	public void Run(Process process)
	{
		// I don't know how thread-bad this is...
		new Thread(() => kill(process)).Start();
	}

	private static void kill(Process process)
	{
		Thread.Sleep(TimeSpan.FromSeconds(10));

		process.Kill();
	}
}

public class CoreLimitingPostLaunchJob : IPostLaunchJob
{
	public void Run(Process process)
	{
		new Thread(() => limitProcessCores(process)).Start();
	}

	// Inspired by Miaa245's core limiting script.
	// https://answers.ea.com/t5/Technical-Issues-PC/Sims-3-won-t-open-Alder-Lake-Intel-12th-gen-CPU/td-p/11057820/page/5.
	// Miaa245 https://answers.ea.com/t5/user/viewprofilepage/user-id/7950707
	/// <summary>
	/// Limits the number of cores used by a process for a short amount of time.
	/// </summary>
	/// <param name="process">The process to limit.</param>
	private static void limitProcessCores(Process process)
	{
		// The processor affinity used when the game starts.
		// Each binary digit corresponds to one core.
		int processorAffinity = 0b1;

		var duration = TimeSpan.FromSeconds(5);

		try
		{
			// Is there a reason to use IntPtr instead of nint (or var)?

			// Save core usage and limit to a single core.
			IntPtr affinity = process.ProcessorAffinity;
			process.ProcessorAffinity = new IntPtr(processorAffinity);

			// Wait for the game to start.
			Thread.Sleep(duration);

			Trace.WriteLine(process.HasExited ? "Not running" : "Running");

			// Return if the process is turned off. This is the case if multiple instances were launched.
			if (process.HasExited)
			{
				return;
			}

			// Restore previous core usage.
			process.ProcessorAffinity = affinity;
		}
		catch { }
	}
}

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
