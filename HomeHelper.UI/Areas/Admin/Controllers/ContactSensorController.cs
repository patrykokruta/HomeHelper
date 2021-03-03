using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeHelper.Common.Enums;
using HomeHelper.CQRS.ContactSensors.Queries;
using HomeHelper.CQRS.Devices.Queries;
using HomeHelper.CQRS.HttpDevices.Queries;
using HomeHelper.CQRS.Readings.Queries;
using HomeHelper.SDK.HttpSensors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactSensorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEnumerable<IContactSensor> _contactSensors;

        public ContactSensorController(IMediator mediator, IEnumerable<IContactSensor> contactSensors)
        {
            _mediator = mediator;
            _contactSensors = contactSensors;
        }
        public async Task<IActionResult> Index()
        {
            var contactSensors = await _mediator.Send(new GetAllContactSensorsQuery());
            return View(contactSensors);
        }
        public async Task<IActionResult> IsContact(string id)
        {
            var device = await _mediator.Send(new GetDeviceByConnectedDeviceIdQuery() { Id = id });
            if (device != null)
            {
                if (device.Protocol == ProtocolType.Http)
                {
                    var sensor = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
                    var modelName = device.ModelName;
                    var contactSensor = _contactSensors.Where(s => s.Name == modelName).FirstOrDefault();
                    var contactValue = await contactSensor.IsContact(sensor.IpAddress, sensor.PortNumber);
                    return Json(new { contact = contactValue, date = DateTime.Now });
                }
                else
                {
                    var lastReading = await _mediator.Send(new GetLastReadingQuery() { ConnectedDeviceId = id, Type = SubscribeType.Contact });
                    return Json(new { contact = lastReading.Contact, date = lastReading.CreatedTime });
                }
            }
            return BadRequest();
        }
        public async Task<IActionResult> Battery(string id)
        {
            var device = await _mediator.Send(new GetDeviceByConnectedDeviceIdQuery() { Id = id });
            if (device != null)
            {
                if (device.Protocol == ProtocolType.Http)
                {
                    var sensor = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
                    var modelName = device.ModelName;
                    var contactSensor = _contactSensors.Where(s => s.Name == modelName).FirstOrDefault();
                    var batteryValue = await contactSensor.GetBatteryValue(sensor.IpAddress, sensor.PortNumber);
                    return Json(new { battery = batteryValue, date = DateTime.Now });
                }
                else
                {
                    var lastReading = await _mediator.Send(new GetLastReadingQuery() { ConnectedDeviceId = id, Type = SubscribeType.Battery });
                    return Json(new { battery = lastReading.Battery, date = lastReading.CreatedTime });
                }
            }
            return BadRequest();

        }
    }
}