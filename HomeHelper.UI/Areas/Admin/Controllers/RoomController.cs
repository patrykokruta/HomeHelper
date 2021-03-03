using HomeHelper.CQRS.Rooms.Commands.CreateRoom;
using HomeHelper.CQRS.Rooms.Queries.GetAllRooms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateRoomCommand command)
        {
            var result = await _mediator.Send(command);
            ModelState.Clear();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllRoomsQuery());
            return Json(result);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }



    }
}