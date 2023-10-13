using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixB.CarValeting.Application.Commands.UserLogin;
using SixB.CarValeting.Application.Queries.GetAllBookings;

namespace SixB.CarValeting.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IMediator mediator,
            ILogger<AdminController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _mediator.Send(new GetAllBookingsQuery());

                return View(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            try
            {
                var result = await _mediator.Send(command);
                if (!result.IsLoggedIn) return View(command);

                var claimsIdentity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.Username)
                    },
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return View(command);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}