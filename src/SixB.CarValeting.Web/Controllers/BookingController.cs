using MediatR;
using Microsoft.AspNetCore.Mvc;
using SixB.CarValeting.Application.Commands.CreateBooking;

namespace SixB.CarValeting.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookingController> _logger;

        public BookingController(
            IMediator mediator,
            ILogger<BookingController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateBookingCommand command)
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

            return View("Success");
        }
    }
}