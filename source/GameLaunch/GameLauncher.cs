using System.Diagnostics;

namespace GameLaunch;

public class GameLauncher
{
	private readonly bool _startSingleCore;

	public GameLauncher()
	{
		// TODO: Move this to an options object.
		_startSingleCore = true;
	}

	public void Launch(string path)
	{
		Trace.WriteLine("Launching game at " + path);

		var process = new Process();
		process.StartInfo.FileName = path;
		try
		{
			bool started = process.Start();

			Trace.WriteLine(started ? "Started" : "Not started");
		}
		catch
		{
			return;
		}

		if (_startSingleCore)
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
