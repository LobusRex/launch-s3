using LobuS3Launcher.ExpansionConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LobuS3Launcher.Composition;

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

		var host = builder.Build();

		Services = host.Services;
	}
}
