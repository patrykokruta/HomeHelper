using HomeHelper.Common.Enums;
using HomeHelper.Common.Mqtt;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Readings.Queries
{
    public class GetAllReadingsQuery : IRequest<IEnumerable<MqttResponseWithLog>>
    {
        public string ConnectedDeviceId { get; set; }
        public SubscribeType Type { get; set; }
    }
    public class GetAllReadingsQueryHandler : IRequestHandler<GetAllReadingsQuery, IEnumerable<MqttResponseWithLog>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllReadingsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MqttResponseWithLog>> Handle(GetAllReadingsQuery request, CancellationToken cancellationToken)
        {
            List<MqttResponseWithLog> response = null;
            switch (request.Type)
            {
                case SubscribeType.Temperature:
                    var tempReadings = await _unitOfWork.TemperatureReading.GetAllAsync(t => t.ConnectedDeviceId == request.ConnectedDeviceId);
                    if (tempReadings != null)
                    {
                        response = new List<MqttResponseWithLog>();
                        foreach (var tempReading in tempReadings)
                        {
                            response.Add(new MqttResponseWithLog() { Temperature = tempReading.Value, CreatedTime = tempReading.CreatedDate });
                        }
                    }
                    break;
                case SubscribeType.Humidity:
                    var humReadings = await _unitOfWork.HumidityReading.GetAllAsync(t => t.ConnectedDeviceId == request.ConnectedDeviceId);
                    if (humReadings != null)
                    {
                        response = new List<MqttResponseWithLog>();
                        foreach (var humReading in humReadings)
                        {
                            response.Add(new MqttResponseWithLog() { Humidity = humReading.Value, CreatedTime = humReading.CreatedDate });
                        }
                    }
                    break;
                case SubscribeType.Battery:
                    var batteryReadings = await _unitOfWork.BatteryReading.GetAllAsync(t => t.ConnectedDeviceId == request.ConnectedDeviceId);
                    if (batteryReadings != null)
                    {
                        response = new List<MqttResponseWithLog>();
                        foreach (var batteryReading in batteryReadings)
                        {
                            response.Add(new MqttResponseWithLog() { Battery = batteryReading.Value, CreatedTime = batteryReading.CreatedDate });
                        }
                    }
                    break;
                case SubscribeType.Motion:
                    var motionReadings = await _unitOfWork.MotionReading.GetAllAsync(t => t.ConnectedDeviceId == request.ConnectedDeviceId);
                    if (motionReadings != null)
                    {
                        response = new List<MqttResponseWithLog>();
                        foreach (var motionReading in motionReadings)
                        {
                            response.Add(new MqttResponseWithLog() { Motion = motionReading.Value, CreatedTime = motionReading.CreatedDate });
                        }
                    }
                    break;
                case SubscribeType.Contact:
                    var contactReadings = await _unitOfWork.ContactReading.GetAllAsync(t => t.ConnectedDeviceId == request.ConnectedDeviceId);
                    if (contactReadings != null)
                    {
                        response = new List<MqttResponseWithLog>();
                        foreach (var contactReading in contactReadings)
                        {
                            response.Add(new MqttResponseWithLog() { Contact = contactReading.Value, CreatedTime = contactReading.CreatedDate });
                        }
                    }
                    break;
                default:
                    break;
            }
            return response;
        }
    }
}
