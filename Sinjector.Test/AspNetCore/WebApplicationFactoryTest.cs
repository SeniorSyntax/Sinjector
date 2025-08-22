using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test.AspNetCore;

[SinjectorFixture]
[WebApplicationFactoryTest]
public class WebApplicationFactoryTest : IIsolateSystem
{
	public AutofacWebApplicationFactory<TestStartup> Web;
	public IEnumerable<FakeService> Service;
	public IEnumerable<IHttpContextAccessor> HttpContextAccessor;

	public void Isolate(IIsolate isolate)
	{
		isolate.UseTestDouble<FakeService>().For<ITestService>();
	}

	public class FakeService : ITestService
	{
		public string Value() => "Fake service";
	}

	[Test]
	public void ShouldGetResponseFromController()
	{
		var client = Web.CreateClient();

		var result = client.GetAsync("/controller/value").Result;

		result.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = result.Content.ReadAsStringAsync().Result;
		content.Should().Contain("controller");
	}

	[Test]
	public void ShouldGetResponseFromFakeService()
	{
		var client = Web.CreateClient();

		var result = client.GetAsync("/service/value").Result;

		result.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = result.Content.ReadAsStringAsync().Result;
		content.Should().Contain("Fake service");
	}

	[Test]
	public void ShouldInjectFakeService()
	{
		Service.Should().Have.Count.EqualTo(1);
	}

	[Test]
	public void ShouldInjectHttpContextAccessor()
	{
		HttpContextAccessor.Should().Have.Count.EqualTo(1);
	}

	public class WebApplicationFactoryTestAttribute : Attribute, IContainerBuild, IContainerSetup
	{
		public ITheContainer ContainerBuild(Action<ITheContainerBuilder> registrations)
		{
			AutofacWebApplicationFactory<TestStartup> factory = null;

			factory = new AutofacWebApplicationFactory<TestStartup>(builder =>
			{
				registrations.Invoke(new AutofacBuilder(builder));
				builder.RegisterInstance(factory).As<AutofacWebApplicationFactory<TestStartup>>().SingleInstance();
			});
			factory.CreateClient();

			return new AutofacContainer(factory.Container);
		}

		public void ContainerSetup(IContainerSetupContext context)
		{
			context.AddModule(new TestSystemModule());
		}
	}


	/// <summary>
	/// Based upon https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/test/integration-tests/samples/3.x/IntegrationTestsSample
	/// </summary>
	/// <typeparam name="TStartup"></typeparam>
	public class AutofacWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
	{
		private readonly Action<ContainerBuilder> _registrations;
		public Container Container;

		public AutofacWebApplicationFactory(Action<ContainerBuilder> registrations)
		{
			_registrations = registrations;
			// to fool WebApplicationFactory to not look for .sln file for content root
			var assemblyFullName = typeof(TStartup).Assembly.FullName;
			File.WriteAllText("MvcTestingAppManifest.json", $"{{\"{assemblyFullName}\": \".\"}}");
		}

		protected override IHostBuilder CreateHostBuilder()
		{
			return Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(x =>
				{
					x.UseStartup<TStartup>()
						.UseTestServer();
				});
		}

		protected override IHost CreateHost(IHostBuilder builder)
		{
			var factory = new CustomServiceProviderFactory(_registrations);
			builder.UseServiceProviderFactory(factory);
			var host = base.CreateHost(builder);
			Container = factory.Container;
			return host;
		}
	}

	/// <summary>
	/// Based upon https://github.com/dotnet/aspnetcore/issues/14907#issuecomment-620750841 - only necessary because of an issue in ASP.NET Core
	/// </summary>
	public class CustomServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
	{
		private readonly Action<ContainerBuilder> _registrations;
		private readonly AutofacServiceProviderFactory _wrapped;
		private IServiceCollection _services;

		public Container Container;

		public CustomServiceProviderFactory(Action<ContainerBuilder> registrations)
		{
			_registrations = registrations;
			_wrapped = new AutofacServiceProviderFactory();
		}

		public ContainerBuilder CreateBuilder(IServiceCollection services)
		{
			// Store the services for later.
			_services = services;

			return _wrapped.CreateBuilder(services);
		}

		public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
		{
			var sp = _services.BuildServiceProvider();
#pragma warning disable CS0612 // Type or member is obsolete
			var filters = sp.GetRequiredService<IEnumerable<IStartupConfigureContainerFilter<ContainerBuilder>>>();
#pragma warning restore CS0612 // Type or member is obsolete

			foreach (var filter in filters)
				filter.ConfigureContainer(b => { })(containerBuilder);

			_registrations.Invoke(containerBuilder);

			var serviceProvider = (AutofacServiceProvider) _wrapped.CreateServiceProvider(containerBuilder);
			Container = (Container) serviceProvider.LifetimeScope;

			return serviceProvider;
		}
	}
}