using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TheWorld.Service;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
	public class AppController : Controller
    {
		private IMailService _mailService;
		private IConfigurationRoot _config;

		public AppController(IMailService mailService, IConfigurationRoot config)
		{
			_mailService = mailService;
			_config = config;
		}

        public IActionResult Index()
		{
			return View();
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
    }
}