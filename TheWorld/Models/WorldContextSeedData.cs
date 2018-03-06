using System;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
	public class WorldContextSeedData
    {
		private WorldContext _context;

		public WorldContextSeedData(WorldContext context)
		{
			_context = context;
		}

		public async Task EnsureSeedData()
		{
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
    }
}