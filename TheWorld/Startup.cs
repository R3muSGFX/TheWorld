using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Service;
using TheWorld.ViewModels;

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

			services
				.AddMvc(config =>
				{
					if (_env.IsProduction())
					{
						config.Filters.Add(new RequireHttpsAttribute());
					}
				})
				.AddJsonOptions(config =>
				{
					config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});

			services.ConfigureApplicationCookie(options => 
			{
				options.LoginPath = "/Account/Login";
				options.Events = new CookieAuthenticationEvents()
				{
					OnRedirectToLogin = async ctx =>
					{
						if (ctx.Request.Path.StartsWithSegments("/api") && 
							ctx.Response.StatusCode == 200)
						{
							ctx.Response.StatusCode = 401;
						}
						else
						{
							ctx.Response.Redirect(ctx.RedirectUri);
						}
						await Task.Yield();
					}
				};
			});

			services
				.AddIdentity<WorldUser, IdentityRole>(config =>
				{
					config.User.RequireUniqueEmail = true;
					config.Password.RequiredLength = 8;
				})
				.AddEntityFrameworkStores<WorldContext>();

			if (!(_env.IsProduction()))
			{
				services.AddScoped<IMailService, DebugMailService>();
			}

			services.AddDbContext<WorldContext>();
			services.AddScoped<IWorldRepository, WorldRepository>();
			services.AddTransient<GeoCoordsService>();
			services.AddTransient<WorldContextSeedData>();
			services.AddLogging();
		}
        
        public async void Configure(
			IApplicationBuilder app, 
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

			app.UseAuthentication();

			Mapper.Initialize(config =>
			{
				config.CreateMap<TripViewModel, Trip>().ReverseMap();
				config.CreateMap<StopViewModel, Stop>().ReverseMap();
			});

			app.UseMvc(config =>
			{
				config.MapRoute(
					name: "Default",
					template: "{controller}/{action}/{id?}",
					defaults: new { controller = "App", action = "Index" }
				);
			});
			try
			{
				seeder.EnsureSeedData().Wait();
			}
			catch (Exception ex)
			{
				int retry = 1;
				Debug.WriteLine($"Retrying to connect({retry})... {DateTime.Now}:: => {ex.Message}");
				
				while(seeder.EnsureSeedData().IsFaulted)
				{
					seeder.CloseContextConnection();
					seeder.EnsureSeedData().Wait();
					retry++;
				}
			}
        }

		#endregion Methods

		#region Fields

		private IHostingEnvironment _env;
		private IConfigurationRoot _config;

		#endregion Fields

	}
}