using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers
{
	public class AuthController : Controller
    {
		
		public AuthController(SignInManager<WorldUser> manager)
		{
			_manager = manager;
		}

		#region Methods

		[Route("/Account/Login")]
        public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Trips", "App");
			}

			return View();
		}

		[HttpPost("/Account/Login")]
		public async Task<IActionResult> Login(LoginViewModel loginVm, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var result = await _manager.PasswordSignInAsync(
					loginVm.Username, 
					loginVm.Password, 
					true, false);
				if (result.Succeeded)
				{
					if (string.IsNullOrWhiteSpace(returnUrl))
					{
						return RedirectToAction("Trips, App");
					}
					else
					{
						return Redirect(returnUrl);
					}
				}
				else
				{
					ModelState.AddModelError("", "Username or password incorrect");
				}
			}
			

			return View();
		}

		[Route("/App/Logout")]
		public async Task<IActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				await _manager.SignOutAsync();
			}
			return RedirectToAction("Index","App");
		}

		#endregion Methods

		#region Fields

		private SignInManager<WorldUser> _manager;

		#endregion Fields

	}
}