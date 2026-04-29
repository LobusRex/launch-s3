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

	public async Task RunAsync(Process process)
	{
		await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);

		process.Kill();
		logProcessKilled();
	}

	[LoggerMessage(
		Level = LogLevel.Information,
		Message = $"The game was killed by {nameof(QuitAfter10)}")]
	private partial void logProcessKilled();
}
