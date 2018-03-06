using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheWorld.Service;

namespace TheWorld
{
	public class Startup
    {
		private IHostingEnvironment _env;
		private IConfigurationRoot _config;

		public Startup(IHostingEnvironment env)
		{
			_env = env;
			var builder = new ConfigurationBuilder()
				.SetBasePath(_env.ContentRootPath)
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();
			_config = builder.Build();
		}

        public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(_config);

			if (!(_env.IsProduction()))
			{
				services.AddScoped<IMailService, DebugMailService>();
			}
			services.AddMvc();
		}
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (!env.IsProduction())
			{
				app.UseDeveloperExceptionPage();
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
        }
    }
}