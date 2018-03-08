using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
	public class WorldRepository : IWorldRepository
	{
		
		public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
		{
			_context = context;
			_logger = logger;
		}

		#region Methods for Trip class

		public IEnumerable<Trip> GetAllTrips()
		{
			return _context.Trips.ToList();
		}

		public void AddTrip(Trip trip)
		{
			_context.Add(trip);
		}

		public async Task<bool> SaveChangesAsync()
		{
			return (await _context.SaveChangesAsync()) > 0;
		}

		public void UpdateTrip(Trip trip)
		{
			_context.Update(trip);
		}

		public void DeleteTrip(int tripId) { }

		public Trip GetTripName(string tripName)
		{
			return _context.Trips
				.Include(t => t.Stops)
				.Where(t => t.Name == tripName)
				.FirstOrDefault();
		}

		public Trip GetTripById(int id)
		{
			return _context.Trips
				.Include(t => t.Stops)
				.Where(t => t.Id == id)
				.FirstOrDefault();
		}

		public IEnumerable<Trip> GetUserTripsWithStops(string name)
		{
			try
			{
				return _context.Trips
					.Include(t => t.Stops)
					.OrderBy(t => t.Name)
					.Where(t => t.Username == name)
					.ToList();
			}
			catch (System.Exception ex)
			{
				_logger.LogError("Could not get trips with stops from database", ex);
				return null;
			}
		}

		public Trip GetTripName(string tripName, string name)
		{
			return _context.Trips
				.Include(t => t.Stops)
				.Where(t => t.Name == tripName && t.Username == name)
				.FirstOrDefault();
		}


		#endregion Methods for Trip class

		#region Methods for Stop class

		public void AddStop(string tripName, string username, Stop newStop)
		{
			var trip = GetTripName(tripName, username);
			if (trip != null)
			{
				trip.Stops.Add(newStop);
				_context.Stops.Add(newStop);
			}
		}
		
		#endregion Methods for Stop class

		#region Fields

		private WorldContext _context;
		private ILogger<WorldRepository> _logger;

		#endregion Fields

	}
}