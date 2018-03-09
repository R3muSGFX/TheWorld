using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Service;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
	[Authorize]
	[Route("/api/trips/{tripName}/stops")]
	public class StopsController : Controller
    {
		
		public StopsController(
			IWorldRepository repository, 
			ILogger<StopsController> logger,
			GeoCoordsService geoCoordsService)
		{
			_repository = repository;
			_logger = logger;
			_geoCoordsService = geoCoordsService;
		}

		#region Methods

		[HttpGet("")]
		public IActionResult Get(string tripName)
		{
			try
			{
				var trip = _repository.GetTripName(tripName, User.Identity.Name);

				return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get stops: {ex}");
			}
			return BadRequest("Failed to get stops");
		}

		[HttpPost("")]
		public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel stopViewModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var newStop = Mapper.Map<Stop>(stopViewModel);

					var result = await _geoCoordsService.GetCoordsAsync(newStop.Name);
					if (!result.Succes)
					{
						_logger.LogError($"Failed to get coordinates: {result.Message}");
					}
					else
					{
						newStop.Latitude = result.Latitude;
						newStop.Longitude = result.Longitude;

						_repository.AddStop(tripName, User.Identity.Name, newStop);

						if (await _repository.SaveChangesAsync())
						{
							return Created($"/api/trips/{tripName}/stops/{newStop.Name}", 
								Mapper.Map<StopViewModel>(newStop));
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to save new stop: {ex.Message}");
			}

			return BadRequest("Failed to save new stop");
		}

		#endregion Methods

		#region Fields

		private IWorldRepository _repository;
		private ILogger<StopsController> _logger;
		private GeoCoordsService _geoCoordsService;

		#endregion Fields

	}
}