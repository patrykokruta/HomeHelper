using HomeHelper.Common.Wrappers;
using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.MqttDevices.Commands.UpdateMqttDevice
{
    public class UpdateMqttDeviceCommand : IRequest<string>
    {
        public string Id { get; set; }
        public bool isConfirmed { get; set; }
        public bool isAvailable { get; set; }
    }

    public class UpdateMqttDeviceCommandHandler : IRequestHandler<UpdateMqttDeviceCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMqttDeviceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(UpdateMqttDeviceCommand request, CancellationToken cancellationToken)
        {
            var mqttDevice = new MqttDevice() { Id = request.Id, IsAvailable = request.isAvailable, IsConfirmed = request.isConfirmed };
            _unitOfWork.MqttDevice.Update(mqttDevice);
            await _unitOfWork.SaveAsync();

            return mqttDevice.Id;
        }
    }
}
