using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    }
}