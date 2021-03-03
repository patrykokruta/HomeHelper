using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Devices.Queries
{
    public class GetDeviceByConnectedDeviceIdQuery : IRequest<Device>
    {
        public string Id { get; set; }
    }
    public class GetDeviceByConnectedDeviceIdQueryHandler : IRequestHandler<GetDeviceByConnectedDeviceIdQuery, Device>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDeviceByConnectedDeviceIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Device> Handle(GetDeviceByConnectedDeviceIdQuery request, CancellationToken cancellationToken)
        {
            var device = await _unitOfWork.Device.GetAllAsync(d => d.ConnectedDeviceId == request.Id);
            return device.FirstOrDefault();
        }
    }
}
