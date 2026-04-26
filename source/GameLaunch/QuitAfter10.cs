using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GameLaunch;

public partial class QuitAfter10 : IPostLaunchJob
{
	private readonly ILogger<QuitAfter10> _logger;

	public QuitAfter10(ILogger<QuitAfter10> logger)
	{
		_logger = logger;
	}

	public void Run(Process process)
	{
		// I don't know how thread-bad this is...
		new Thread(() => kill(process)).Start();
	}

	private void kill(Process process)
	{
		Thread.Sleep(TimeSpan.FromSeconds(10));

		process.Kill();
		logProcessKilled();
	}

	[LoggerMessage(
		Level = LogLevel.Information,
		Message = $"The game was killed by {nameof(QuitAfter10)}")]
	private partial void logProcessKilled();
}
