using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixB.CarValeting.Application.Commands.CreateBooking;
using SixB.CarValeting.Application.Commands.DeleteBooking;
using SixB.CarValeting.Application.Commands.EditBooking;
using SixB.CarValeting.Application.Commands.UserLogin;
using SixB.CarValeting.Application.Queries.GetAllBookings;
using SixB.CarValeting.Application.Queries.GetBookingById;

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return View(await GetById(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookingCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            try
            {
                await _mediator.Send(command);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                ModelState.AddModelError("", "Failed to edit booking");
                return View(command);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteBookingCommand { Id = id });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeApproval(int id)
        {
            try
            {
                var command = await GetById(id);
                command.IsApproved = !command.IsApproved;

                await _mediator.Send(command);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            try
            {
                await _mediator.Send(command);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                ModelState.AddModelError("", "Failed to create booking");
                return View(command);
            }

            return RedirectToAction("Index");
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

                if (!result.IsLoggedIn)
                {
                    ModelState.AddModelError("", "Failed to login");
                    return View(command);
                }

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
                ModelState.AddModelError("", "Failed to login");
                return View(command);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        private async Task<EditBookingCommand> GetById(int id)
        {
            var result = await _mediator.Send(new GetBookingByIdQuery { Id = id });

            return new EditBookingCommand
            {
                Id = id,
                Name = result.Booking.Name,
                Date = result.Booking.Date,
                VehicleSize = result.Booking.VehicleSize,
                Flexibility = result.Booking.Flexibility,
                Email = result.Booking.Email,
                IsApproved = result.Booking.IsApproved,
                PhoneNumber = result.Booking.PhoneNumber
            };
        }
    }
}