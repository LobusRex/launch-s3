using GameLaunch;
using LobuS3Launcher.ExpansionConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LobuS3Launcher.Composition;

/// <summary>
/// A way to configure services in a central place.
/// A temporary measure that should eventually be replaced with dependency injection.
/// Get services by using <see cref="Services"/> from the static <see cref="Instance"/>.
/// </summary>
internal class ServiceLocator
{
	public static readonly ServiceLocator Instance = new();

	public IServiceProvider Services { get; }

	public ServiceLocator()
	{
		var builder = Host.CreateApplicationBuilder();

		builder.Services
			.AddOptions<ExpansionsSection>()
			.BindConfiguration(ExpansionsSection.SectionName);

		builder.Services.AddSingleton<GameLauncher>();

		var host = builder.Build();

		Services = host.Services;
	}
}
