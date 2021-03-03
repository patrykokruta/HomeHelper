using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.ZigbeeDevices.Queries.GetAllZigbeeDevices
{
    public class GetAllZigbeeDevicesQuery : IRequest<IEnumerable<ZigbeeDevice>>
    {
    }

    public class GetAllZigbeeDevicesQueryHandler : IRequestHandler<GetAllZigbeeDevicesQuery, IEnumerable<ZigbeeDevice>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllZigbeeDevicesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ZigbeeDevice>> Handle(GetAllZigbeeDevicesQuery request, CancellationToken cancellationToken)
        {
            var devices = await _unitOfWork.ZigbeeDevice.GetAllAsync();
            return devices;
        }
    }
}
