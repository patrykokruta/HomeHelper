using HomeHelper.Common.Enums;
using HomeHelper.CQRS.Devices.Queries.GetAllDevices;
using HomeHelper.CQRS.HttpDevices.Queries;
using HomeHelper.CQRS.MqttMessages.PublishingMessages.Queries;
using HomeHelper.CQRS.Switches.Queries;
using HomeHelper.SDK;
using HomeHelper.SDK.HttpControls;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SwitchController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEnumerable<ISwitch> _switches;
        private readonly IEnumerable<IMqtt> _mqttDevices;

        public SwitchController(IMediator mediator, IEnumerable<ISwitch> switches, IEnumerable<IMqtt> mqttDevices)
        {
            _mediator = mediator;
            _switches = switches;
            _mqttDevices = mqttDevices;
        }
        public async Task<IActionResult> Index()
        {
            var devices = await _mediator.Send(new GetAllSwitchesQuery());
            return View(devices);
        }

        [HttpPost]
        public async Task<IActionResult> TurnOn(string id)
        {
            var httpDevice = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
            var modelName = (await _mediator.Send(new GetAllDevicesQuery())).FirstOrDefault(d => d.ConnectedDeviceId == id).ModelName;
            var sw = _switches.FirstOrDefault(s => s.Name == modelName);
            var result = await sw.TurnOn(httpDevice.IpAddress, httpDevice.PortNumber);
            if (result) return Ok();
            else return BadRequest();

        }
        [HttpPost]
        public async Task<IActionResult> TurnOff(string id)
        {
            var httpDevice = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
            var modelName = (await _mediator.Send(new GetAllDevicesQuery())).FirstOrDefault(d => d.ConnectedDeviceId == id).ModelName;
            var sw = _switches.FirstOrDefault(s => s.Name == modelName);
            var result = await sw.TurnOff(httpDevice.IpAddress, httpDevice.PortNumber);
            if (result) return Ok();
            else return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> State(string id)
        {
            var httpDevice = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
            var modelName = (await _mediator.Send(new GetAllDevicesQuery())).FirstOrDefault(d => d.ConnectedDeviceId == id).ModelName;
            var sw = _switches.FirstOrDefault(s => s.Name == modelName);
            var result = await sw.IsOn(httpDevice.IpAddress, httpDevice.PortNumber);
            if (result == null) return BadRequest();
            if (result == true) return Json(new { state = "on" });
            else return Json(new { state = "off" });
        }

        [HttpGet]
        public async Task<IActionResult> Linkquality(string id)
        {
            var httpDevice = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
            var modelName = (await _mediator.Send(new GetAllDevicesQuery())).FirstOrDefault(d => d.ConnectedDeviceId == id).ModelName;
            var sw = _switches.FirstOrDefault(s => s.Name == modelName);
            var result = await sw.SignalStrength(httpDevice.IpAddress, httpDevice.PortNumber);
            if (result == null) return BadRequest();
            else
            {
                return Json(new { linkquality = result });
            }

        }

        [HttpGet]
        public async Task<IActionResult> MqttMessage(string id, string publishType)
        {
            var messages = await _mediator.Send(new GetAllMqttPublishingMessagesQuery());
            var message = messages.FirstOrDefault(m => m.ClientId == id && Enum.GetName(typeof(PublishType), m.PublishType) == publishType);
            var device = _mqttDevices.FirstOrDefault(d => d.Name == message.ModelName);
            var publishMessage = device.PublishMessages.FirstOrDefault(
                m => m.GetTopic(id) == message.Topic && m.PublishType == message.PublishType);

            return Json(new { topic = message.Topic, payload = publishMessage.GetPayload() });
        }
    }
}