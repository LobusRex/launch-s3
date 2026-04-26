using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GameLaunch;

public partial class CoreLimitingPostLaunchJob : IPostLaunchJob
{
	private readonly ILogger<CoreLimitingPostLaunchJob> _logger;

	// The processor affinity to use when the game starts.
	// It is a bitmask where every bit corresponds to one (logical?) core on the processor.
	// https://en.wikipedia.org/wiki/Processor_affinity
	// For solving the issue with Intel processors it does not seem to matter which core is used,
	// only that the game is limited to a single core.
	private readonly nint _limitedProcessorAffinity = 0b1;

	private readonly TimeSpan _duration = TimeSpan.FromSeconds(5);

	public CoreLimitingPostLaunchJob(
		ILogger<CoreLimitingPostLaunchJob> logger)
	{
		_logger = logger;
	}

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
	private void limitProcessCores(Process process)
	{
		logRunningLimiter();

		try
		{
			var originalProcessorAffinity = process.ProcessorAffinity;

			process.ProcessorAffinity = _limitedProcessorAffinity;

			// Delay long enough for the fix to have an effect.
			Thread.Sleep(_duration);

			if (!process.HasExited)
			{
				logStillRunning();
				process.ProcessorAffinity = originalProcessorAffinity;
			}
			else
			{
				logNoLongerRunning();
			}
		}
		catch { }
	}

	[LoggerMessage(
		Level = LogLevel.Information,
		Message = $"Temporarily limiting the number of cores the game is running on.")]
	private partial void logRunningLimiter();

	[LoggerMessage(
		Level = LogLevel.Debug,
		Message = "")]
	private partial void logStillRunning();

	[LoggerMessage(
		Level = LogLevel.Debug,
		Message = "The game is no longer running.")]
	private partial void logNoLongerRunning();

	[LoggerMessage(
		Level = LogLevel.Debug,
		Message = "Restoring the game's processor affinity.")]
	private partial void logRestoringProcessAffinity();
}
