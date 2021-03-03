using HomeHelper.Common;
using HomeHelper.Common.Enums;
using HomeHelper.CQRS.Devices.Commands.CreateDevice;
using HomeHelper.CQRS.HttpDevices.Commands.CreateHttpDevice;
using HomeHelper.CQRS.Rooms.Queries.GetAllRooms;
using HomeHelper.SDK;
using HomeHelper.Services.MqttMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeHelper.Domain;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DeviceController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEnumerable<IDevice> _devices;
        private readonly IMqttMessageService _mqttMessageService;

        public DeviceController(IMediator mediator, IEnumerable<IDevice> devices, IMqttMessageService mqttMessageService)
        {
            _mediator = mediator;
            _devices = devices;
            _mqttMessageService = mqttMessageService;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var rooms = await _mediator.Send(new GetAllRoomsQuery());

            return View(new CreateDeviceCommand() { Rooms = rooms });
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateDeviceCommand command)
        {
            if (command.Protocol == ProtocolType.Http)
            {
                var id = await _mediator.Send(new CreateHttpDeviceCommand() { IpAddress = command.IpAddress, PortNumber = command.PortNumber });
                command.ConnectedDeviceId = id;
            }
            else
            {
                await _mqttMessageService.RegisterPublishMessages(command.ConnectedDeviceId, command.ModelName);
                await _mqttMessageService.RegisterSubscribeMessages(command.ConnectedDeviceId, command.ModelName);
            }
            await _mediator.Send(command);
            return Ok();
        }


        [HttpGet]
        public IActionResult GetFilteredDevices(string protocolType, string deviceType)
        {
            var filteredDevices = _devices.
                Where(device => device.Protocol.ToString() == protocolType && device.Type.ToString() == deviceType).
                Select(device => device.Name).
                ToList();

            return Json(MapToSelectOptions(filteredDevices));
        }
        private IEnumerable<SelectOptions> MapToSelectOptions(IEnumerable<string> textArray)
        {
            var selectOptions = new List<SelectOptions>();
            foreach (var text in textArray)
            {
                selectOptions.Add(new SelectOptions() { id = text, text = text });
            }
            return selectOptions;
        }
    }
}