using HomeHelper.Common.Enums;
using HomeHelper.Common.Mqtt;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Readings.Queries
{
    public class GetLastReadingQuery : IRequest<MqttResponseWithLog>
    {
        public string ConnectedDeviceId { get; set; }
        public SubscribeType Type { get; set; }
    }
    public class GetLastReadingQueryHandler : IRequestHandler<GetLastReadingQuery, MqttResponseWithLog>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLastReadingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<MqttResponseWithLog> Handle(GetLastReadingQuery request, CancellationToken cancellationToken)
        {
            MqttResponseWithLog response = new MqttResponseWithLog();

            switch (request.Type)
            {
                case SubscribeType.Temperature:
                    var tempReading = (await _unitOfWork.TemperatureReading.GetAllAsync(
                        t => t.ConnectedDeviceId == request.ConnectedDeviceId)).OrderBy(t => t.CreatedDate).LastOrDefault();
                    response.Temperature = tempReading.Value;
                    response.CreatedTime = tempReading.CreatedDate;
                    break;

                case SubscribeType.Humidity:
                    var humReading = (await _unitOfWork.HumidityReading.GetAllAsync(
                        t => t.ConnectedDeviceId == request.ConnectedDeviceId)).OrderBy(t => t.CreatedDate).LastOrDefault();
                    response.Humidity = humReading.Value;
                    response.CreatedTime = humReading.CreatedDate;
                    break;
                case SubscribeType.Battery:
                    var batteryReading = (await _unitOfWork.BatteryReading.GetAllAsync(
                        t => t.ConnectedDeviceId == request.ConnectedDeviceId)).OrderBy(t => t.CreatedDate).LastOrDefault();
                    response.Battery = batteryReading.Value;
                    response.CreatedTime = batteryReading.CreatedDate;
                    break;
                case SubscribeType.Motion:
                    var motionReading = (await _unitOfWork.MotionReading.GetAllAsync(
                        t => t.ConnectedDeviceId == request.ConnectedDeviceId)).OrderBy(t => t.CreatedDate).LastOrDefault();
                    response.Motion = motionReading.Value;
                    response.CreatedTime = motionReading.CreatedDate;
                    break;
                case SubscribeType.Contact:
                    var contactReading = (await _unitOfWork.ContactReading.GetAllAsync(
                        t => t.ConnectedDeviceId == request.ConnectedDeviceId)).OrderBy(t => t.CreatedDate).LastOrDefault();
                    response.Contact = contactReading.Value;
                    response.CreatedTime = contactReading.CreatedDate;
                    break;
                default:
                    break;
            }
            return response;
        }
    }
}
