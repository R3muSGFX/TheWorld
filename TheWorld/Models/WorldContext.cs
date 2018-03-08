using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TheWorld.Models
{
	public class WorldContext : IdentityDbContext<WorldUser>
    {

		public WorldContext(IConfigurationRoot config, DbContextOptions options)
			: base (options)
		{
			_config = config;
		}

		#region Methods

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseSqlServer(_config["ConnectionStrings:WorldContextConnection"]);
			
		}

		public void Close() => this.Database.CloseConnection();

		#endregion Methods

		#region Fields

		private IConfigurationRoot _config;

		#endregion Fields

		#region Properties

		public DbSet<Trip> Trips { get; set; }

		public DbSet<Stop> Stops { get; set; }

		#endregion Properties

	}
}