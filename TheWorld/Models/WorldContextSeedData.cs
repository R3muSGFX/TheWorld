using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
	public class WorldContextSeedData
    {

		#region Fields

		private WorldContext _context;
		private UserManager<WorldUser> _userManager;

		#endregion Fields

		public WorldContextSeedData(
			WorldContext context,
			UserManager<WorldUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task EnsureSeedData()
		{
			var userQ = await _userManager.FindByEmailAsync("go@google.com");
			if (userQ == null)
			{
				var user = new WorldUser()
				{
					UserName = "remus",
					Email = "remus.r3mus@gmail.com"
				};

				await _userManager.CreateAsync(user, "P@ssw0rd!!!");
			}

			if (!_context.Trips.Any())
			{
				var usTrip = new Trip()
				{
					DateCreated = DateTime.Now,
					Name = "us trip",
					Username = "remus",
					Stops = Common.GetUsTripStops()
				};

				_context.Trips.Add(usTrip);
				_context.Stops.AddRange(usTrip.Stops);

				var worldTrip = new Trip()
				{
					DateCreated = DateTime.Now,
					Name = "world Trip",
					Username = "remus",
					Stops = Common.GetWorldTripStops()
				};

				_context.Trips.Add(worldTrip);
				_context.Stops.AddRange(worldTrip.Stops);

				await _context.SaveChangesAsync();
			}
		}

		public void CloseContextConnection() => _context.Close();
    }
}