using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.CQRS.MqttDevices.Queries.GetAllMqttDevices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MqttDeviceController : Controller
    {
        private readonly IMediator _mediator;
        public MqttDeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> GetNotConfirmed()
        {
            var mqttDevices = await _mediator.Send(new GetAllMqttDevicesQuery());
            var notConfirmedDevices = mqttDevices.
                Where(device => device.IsConfirmed == false && device.IsAvailable == true).Select(device => device.Id);
            return Json(MapToSelectOptions(notConfirmedDevices));
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