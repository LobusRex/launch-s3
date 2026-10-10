using GameLaunch;
using LaunchS3.Expansions;
using LaunchS3.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LaunchS3.Composition;

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

		builder.Services.AddExpansions();

		builder.Services.AddSingleton<TabSelector>();

		builder.Services.AddGameLaunch();

		var host = builder.Build();

		Services = host.Services;
	}
}
