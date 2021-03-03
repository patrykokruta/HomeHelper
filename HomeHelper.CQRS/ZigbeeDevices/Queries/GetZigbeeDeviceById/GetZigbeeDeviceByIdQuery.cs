using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.ZigbeeDevices.Queries.GetZigbeeDeviceById
{
    public class GetZigbeeDeviceByIdQuery : IRequest<ZigbeeDevice>
    {
        public string Id { get; set; }
    }

    public class GetZigbeeDeviceByIdQueryHandler : IRequestHandler<GetZigbeeDeviceByIdQuery, ZigbeeDevice>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetZigbeeDeviceByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ZigbeeDevice> Handle(GetZigbeeDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var device = await _unitOfWork.ZigbeeDevice.GetAsync(request.Id);
            await _unitOfWork.SaveAsync();
            return device;
        }
    }
}
