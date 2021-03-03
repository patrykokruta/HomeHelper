using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Devices.Queries.GetAllDevices
{
    public class GetAllDevicesQuery : IRequest<IEnumerable<Device>>
    {
    }
    public class GetAllDevicesQueryHandler : IRequestHandler<GetAllDevicesQuery, IEnumerable<Device>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDevicesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Device>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
        {
            var devices = await _unitOfWork.Device.GetAllAsync();
            return devices;
        }
    }
}
