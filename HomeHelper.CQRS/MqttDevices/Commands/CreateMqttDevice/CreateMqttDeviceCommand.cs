using HomeHelper.Common.Wrappers;
using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.MqttDevices.Commands.CreateMqttDevice
{
    public class CreateMqttDeviceCommand : IRequest<string>
    {
        public string Id { get; set; }
        public bool isConfirmed { get; set; }
        public bool isAvailable { get; set; }
    }
    public class CreateMqttDeviceCommandHandler : IRequestHandler<CreateMqttDeviceCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMqttDeviceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(CreateMqttDeviceCommand request, CancellationToken cancellationToken)
        {

            await _unitOfWork.MqttDevice.AddAsync(new MqttDevice()
            {
                Id = request.Id,
                IsConfirmed = request.isConfirmed,
                IsAvailable = request.isAvailable
            });
            await _unitOfWork.SaveAsync();

            return request.Id;
        }
    }
}
