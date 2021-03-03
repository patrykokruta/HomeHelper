using HomeHelper.Common.Wrappers;
using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.MqttDevices.Queries.GetAllMqttDevices
{
    public class GetAllMqttDevicesQuery : IRequest<IEnumerable<MqttDevice>>
    {
    }
    public class GetAllMqttDevicesQueryHandler : IRequestHandler<GetAllMqttDevicesQuery, IEnumerable<MqttDevice>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMqttDevicesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MqttDevice>> Handle(GetAllMqttDevicesQuery request, CancellationToken cancellationToken)
        {
            var mqttDevices = await _unitOfWork.MqttDevice.GetAllAsync();
            return mqttDevices;
        }
    }
}
