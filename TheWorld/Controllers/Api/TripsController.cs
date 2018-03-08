using AutoMapper;
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
	[Route("api/trips")]
	public class TripsController : Controller
    {
		private IWorldRepository _repository;
		private ILogger<TripsController> _logger;

		public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		[HttpGet]
        public IActionResult Get()
		{
			try
			{
				var results = _repository.GetAllTrips().ToList();

				return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
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
    }
}