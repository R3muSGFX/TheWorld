using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
	[Authorize]
	[Route("api/trips")]
	public class TripsController : Controller
    {

		public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		#region Methods

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				var trips = _repository.GetUserTripsWithStops(User.Identity.Name);

				return Ok(Mapper.Map<IEnumerable<TripViewModel>>(trips));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to to get alltrips: {ex}");
				return BadRequest("Error ocurred");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Result([FromBody]TripViewModel trip)
		{
			if (ModelState.IsValid)
			{
				var newtrip = Mapper.Map<Trip>(trip);
				newtrip.Username = User.Identity.Name;

				_repository.AddTrip(newtrip);

				if (await _repository.SaveChangesAsync())
				{
					return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(newtrip));
				}
				else
				{
					return BadRequest("Failed to save changes");
				}
			}
			return BadRequest("Failed to save");
		}

		#endregion Methods

		#region Fields

		private IWorldRepository _repository;
		private ILogger<TripsController> _logger;

		#endregion Fields

	}
}