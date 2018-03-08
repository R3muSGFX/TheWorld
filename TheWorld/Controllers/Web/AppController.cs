using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using TheWorld.Models;
using TheWorld.Service;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
	public class AppController : Controller
    {
		
		public AppController(
			IMailService mailService, 
			IConfigurationRoot config, 
			IWorldRepository repository,
			ILogger<AppController> logger)
		{
			_mailService = mailService;
			_config = config;
			_repository = repository;
			_logger = logger;
		}

		#region Methods

		public IActionResult Index()
		{
			try
			{
				var data = _repository.GetAllTrips();

				return View(data);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get the fking trips: {ex.Message}");
				return Redirect("/error");
			}
		}

		[Authorize]
		public IActionResult Trips()
		{
			var trips = _repository.GetAllTrips();
			return View(trips);
		}
		
		public IActionResult Contact()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Contact(ContactViewModel model)
		{
			if (ModelState.IsValid)
			{
				_mailService.SendMail(
					_config["MailSettings:ToAddress"],
					model.Mail,
					$"Contact mail from {model.Name}",
					model.Message);
				ModelState.Clear();
				ViewBag.UserMessage = "Message sent";
			}

			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		#endregion Methods

		#region Fields

		private IMailService _mailService;
		private IConfigurationRoot _config;
		private IWorldRepository _repository;
		private ILogger<AppController> _logger;

		#endregion Fields

	}
}