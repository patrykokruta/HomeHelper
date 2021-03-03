using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeHelper.Common.Enums;
using HomeHelper.CQRS.Devices.Queries;
using HomeHelper.CQRS.HttpDevices.Queries;
using HomeHelper.CQRS.Humidity.Queries;
using HomeHelper.CQRS.Readings.Queries;
using HomeHelper.SDK.HttpSensors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HumiditySensorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEnumerable<IHumiditySensor> _humiditySensors;

        public HumiditySensorController(IMediator mediator, IEnumerable<IHumiditySensor> humiditySensors)
        {
            _mediator = mediator;
            _humiditySensors = humiditySensors;
        }
        public async Task<IActionResult> Index()
        {
            var humiditySensors = await _mediator.Send(new GetAllHumiditySensorsQuery());
            return View(humiditySensors);
        }
        public async Task<IActionResult> Humidity(string id)
        {
            var device = await _mediator.Send(new GetDeviceByConnectedDeviceIdQuery() { Id = id });
            if (device != null)
            {
                if (device.Protocol == ProtocolType.Http)
                {
                    var sensor = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
                    var modelName = device.ModelName;
                    var humiditySensor = _humiditySensors.Where(s => s.Name == modelName).FirstOrDefault();
                    var humidityValue = await humiditySensor.GetHumidityValue(sensor.IpAddress, sensor.PortNumber);
                    return Json(new { humidity = humidityValue, date = DateTime.Now });
                }
                else
                {
                    var lastReading = await _mediator.Send(new GetLastReadingQuery() { ConnectedDeviceId = id, Type = SubscribeType.Humidity });
                    return Json(new { humidity = lastReading.Humidity, date = lastReading.CreatedTime });
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
                    var humiditySensor = _humiditySensors.Where(s => s.Name == modelName).FirstOrDefault();
                    var batteryValue = await humiditySensor.GetBatteryValue(sensor.IpAddress, sensor.PortNumber);
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