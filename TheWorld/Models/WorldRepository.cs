using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace TheWorld.Models
{
	public class WorldRepository : IWorldRepository
	{

		#region Methods

		public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IEnumerable<Trip> GetAllTrips()
		{
			return _context.Trips.ToList();
		}

		#endregion Methods

		#region Fields

		private WorldContext _context;
		private ILogger<WorldRepository> _logger;

		#endregion Fields

	}
}