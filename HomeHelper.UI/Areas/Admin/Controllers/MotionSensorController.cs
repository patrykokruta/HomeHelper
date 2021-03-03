using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeHelper.Common.Enums;
using HomeHelper.CQRS.Devices.Queries;
using HomeHelper.CQRS.HttpDevices.Queries;
using HomeHelper.CQRS.MotionSensors.Queries;
using HomeHelper.CQRS.Readings.Queries;
using HomeHelper.SDK.HttpSensors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MotionSensorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEnumerable<IMotionSensor> _motionSensors;

        public MotionSensorController(IMediator mediator, IEnumerable<IMotionSensor> motionSensors)
        {
            _mediator = mediator;
            _motionSensors = motionSensors;
        }
        public async Task<IActionResult> Index()
        {
            var motionSensors = await _mediator.Send(new GetAllMotionSensorsQuery());
            return View(motionSensors);
        }
        public async Task<IActionResult> IsMotion(string id)
        {
            var device = await _mediator.Send(new GetDeviceByConnectedDeviceIdQuery() { Id = id });
            if (device != null)
            {
                if (device.Protocol == ProtocolType.Http)
                {
                    var sensor = await _mediator.Send(new GetHttpDeviceByIdQuery() { Id = id });
                    var modelName = device.ModelName;
                    var motionSensor = _motionSensors.Where(s => s.Name == modelName).FirstOrDefault();
                    var motionValue = await motionSensor.IsMotion(sensor.IpAddress, sensor.PortNumber);
                    return Json(new { motion = motionValue, date = DateTime.Now });
                }
                else
                {
                    var lastReading = await _mediator.Send(new GetLastReadingQuery() { ConnectedDeviceId = id, Type = SubscribeType.Motion });
                    return Json(new { motion = lastReading.Motion, date = lastReading.CreatedTime });
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
                    var motionSensor = _motionSensors.Where(s => s.Name == modelName).FirstOrDefault();
                    var batteryValue = await motionSensor.GetBatteryValue(sensor.IpAddress, sensor.PortNumber);
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