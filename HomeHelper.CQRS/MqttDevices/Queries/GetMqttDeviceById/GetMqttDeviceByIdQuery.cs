using HomeHelper.Common.Wrappers;
using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.MqttDevices.Queries.GetMqttDeviceById
{
    public class GetMqttDeviceByIdQuery : IRequest<MqttDevice>
    {
        public string Id { get; set; }
    }
    public class GetMqttDeviceByIdQueryHandler : IRequestHandler<GetMqttDeviceByIdQuery, MqttDevice>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMqttDeviceByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<MqttDevice> Handle(GetMqttDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var mqttDevice = await _unitOfWork.MqttDevice.GetAsync(request.Id);

            return mqttDevice;
        }
    }
}
