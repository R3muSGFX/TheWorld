using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
	public interface IWorldRepository
	{
		IEnumerable<Trip> GetAllTrips();

		void AddTrip(Trip trip);

		Task<bool> SaveChangesAsync();

		void UpdateTrip(Trip trip);

		void DeleteTrip(int tripId);

		Trip GetTripName(string tripName);

		Trip GetTripById(int id);

		void AddStop(string tripName, string username, Stop newStop);

		IEnumerable<Trip> GetUserTripsWithStops(string name);

		Trip GetTripName(string tripName, string name);
	}
}