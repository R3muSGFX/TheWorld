using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.Service;

namespace TheWorld
{
	public class Startup
    {

		public Startup(IHostingEnvironment env)
		{
			_env = env;
			var builder = new ConfigurationBuilder()
				.SetBasePath(_env.ContentRootPath)
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();
			_config = builder.Build();
		}

		#region Methods

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(_config);

			if (!(_env.IsProduction()))
			{
				services.AddScoped<IMailService, DebugMailService>();
			}

			services.AddDbContext<WorldContext>();
			services.AddScoped<IWorldRepository, WorldRepository>();
			services.AddTransient<WorldContextSeedData>();
			services.AddLogging();
			services.AddMvc();
		}
        
        public void Configure(IApplicationBuilder app, 
			IHostingEnvironment env,
			WorldContextSeedData seeder,
			ILoggerFactory factory)
        {
			if (!env.IsProduction())
			{
				app.UseDeveloperExceptionPage();
				factory.AddDebug(LogLevel.Information);
			}
			else
			{
				factory.AddDebug(LogLevel.Error);
			}

			app.UseStaticFiles();

			app.UseMvc(config =>
			{
				config.MapRoute(
					name: "Default",
					template: "{controller}/{action}/{id?}",
					defaults: new { controller = "App", action = "Index" }
				);
			});

			seeder.EnsureSeedData().Wait();
        }

		#endregion Methods

		#region Fields

		private IHostingEnvironment _env;
		private IConfigurationRoot _config;

		#endregion Fields

	}
}