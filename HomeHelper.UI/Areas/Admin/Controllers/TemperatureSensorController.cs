using HomeHelper.Common.Enums;
using HomeHelper.CQRS.Devices.Queries;
using HomeHelper.CQRS.HttpDevices.Queries;
using HomeHelper.CQRS.Readings.Queries;
using HomeHelper.CQRS.Temperature.Queries;
using HomeHelper.SDK.HttpSensors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TemperatureSensorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEnumerable<ITemperatureSensor> _temperatureSensors;

        public TemperatureSensorController(IMediator mediator, IEnumerable<ITemperatureSensor> temperatureSensors)
        {
            _mediator = mediator;
            _temperatureSensors = temperatureSensors;
        }
        public async Task<IActionResult> Index()
        {
            var temperatureSensors = await _mediator.Send(new GetAllTemperatureSensorsQuery());
            return View(temperatureSensors);
        }
        public async Task<IActionResult> Charts()
        {
            var temperatureSensors = (await _mediator.Send(new GetAllTemperatureSensorsQuery())).Where(t => t.Protocol != ProtocolType.Http);
            return View(temperatureSensors);
        }

        public async Task<IActionResult> Temperature(string id)
        {
            var device = await _mediator.Send(new GetDeviceByConnectedDeviceIdQuery() { Id = id });
            if (device != null)
            {
                if (device.Protocol == ProtocolType.Http)
                {
                    var sensor = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
                    var modelName = device.ModelName;
                    var tempSensor = _temperatureSensors.Where(s => s.Name == modelName).FirstOrDefault();
                    var tempValue = await tempSensor.GetTemperatureValue(sensor.IpAddress, sensor.PortNumber);
                    return Json(new { temperature = tempValue, date = DateTime.Now });
                }
                else
                {
                    var lastReading = await _mediator.Send(new GetLastReadingQuery() { ConnectedDeviceId = id, Type = SubscribeType.Temperature });
                    return Json(new { temperature = lastReading.Temperature, date = lastReading.CreatedTime });
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
                    var tempSensor = _temperatureSensors.Where(s => s.Name == modelName).FirstOrDefault();
                    var batteryValue = await tempSensor.GetBatteryValue(sensor.IpAddress, sensor.PortNumber);
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
        //public async Task<IActionResult> Availability(string id)
        //{
        //    var sensor = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
        //    var modelName = (await _mediator.Send(new GetDeviceByConnectedDeviceIdQuery() { Id = id })).ModelName;
        //    var tempSensor = _temperatureSensors.Where(s => s.Name == modelName).FirstOrDefault();
        //    var availability = await tempSensor.CheckAvailability(sensor.IpAddress, sensor.PortNumber);
        //    return Json(availability);
        //}
    }
}