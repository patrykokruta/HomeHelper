using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.HttpDevices.Queries
{
    public class GetHttpDeviceByIdQuery : IRequest<HttpDevice>
    {
        public string Id { get; set; }
    }
    public class GetHttpDeviceByIdQueryHandler : IRequestHandler<GetHttpDeviceByIdQuery, HttpDevice>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetHttpDeviceByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<HttpDevice> Handle(GetHttpDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var httpDevice = await _unitOfWork.HttpDevice.GetAsync(request.Id);
            return httpDevice;
        }
    }
}
