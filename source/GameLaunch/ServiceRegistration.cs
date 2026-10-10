using Microsoft.Extensions.DependencyInjection;

namespace GameLaunch;

public static class ServiceRegistration
{
	public static IServiceCollection AddGameLaunch(this IServiceCollection services)
	{
		services.AddSingleton<GameLauncher>();

		// TODO: Feature toggle IPostLaunchJobs from appsettings.
		services.AddSingleton<IPostLaunchJob, CoreLimitingPostLaunchJob>();
#if DEBUG
		services.AddSingleton<IPostLaunchJob, QuitAfter10>();
#endif

		return services;
	}
}
