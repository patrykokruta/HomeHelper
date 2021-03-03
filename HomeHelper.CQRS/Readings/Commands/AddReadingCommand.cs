using HomeHelper.Common.Enums;
using HomeHelper.Common.Mqtt;
using HomeHelper.Domain.Readings;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Readings.Commands
{
    public class AddReadingCommand : IRequest<string>
    {
        public MqttResponse Data { get; set; }
        public string ConnectedDeviceId { get; set; }
        public SubscribeType Type { get; set; }
    }
    public class AddReadingCommandHandler : IRequestHandler<AddReadingCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddReadingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(AddReadingCommand request, CancellationToken cancellationToken)
        {
            string id = null;
            switch (request.Type)
            {
                case SubscribeType.Temperature:
                    id = Guid.NewGuid().ToString();
                    await _unitOfWork.TemperatureReading.AddAsync(
                        new TemperatureReading() { Value = request.Data.Temperature, ConnectedDeviceId = request.ConnectedDeviceId, Id = id });

                    break;
                case SubscribeType.Humidity:
                    id = Guid.NewGuid().ToString();
                    await _unitOfWork.HumidityReading.AddAsync(
                        new HumidityReading() { Value = request.Data.Humidity, ConnectedDeviceId = request.ConnectedDeviceId, Id = id });
                    break;
                case SubscribeType.Battery:
                    id = Guid.NewGuid().ToString();
                    await _unitOfWork.BatteryReading.AddAsync(
                        new BatteryReading() { Value = request.Data.Battery, ConnectedDeviceId = request.ConnectedDeviceId, Id = id });
                    break;
                case SubscribeType.Motion:
                    id = Guid.NewGuid().ToString();
                    await _unitOfWork.MotionReading.AddAsync(
                        new MotionReading() { Value = request.Data.Motion, ConnectedDeviceId = request.ConnectedDeviceId, Id = id });
                    break;
                case SubscribeType.Contact:
                    id = Guid.NewGuid().ToString();
                    await _unitOfWork.ContactReading.AddAsync(
                        new ContactReading() { Value = request.Data.Contact, ConnectedDeviceId = request.ConnectedDeviceId, Id = id });
                    break;
                default:
                    break;
            }
            await _unitOfWork.SaveAsync();
            return id;
        }
    }
}
