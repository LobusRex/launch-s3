using LaunchS3.Expansions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LaunchS3.Expansions;

public static class ServiceRegistration
{
	public static IServiceCollection AddExpansions(this IServiceCollection services)
	{
		services
			.AddOptions<ExpansionsSection>()
			.BindConfiguration(ExpansionsSection.SectionName);

		services.AddSingleton<IExpansionService, ExpansionService>();

		return services;
	}
}
