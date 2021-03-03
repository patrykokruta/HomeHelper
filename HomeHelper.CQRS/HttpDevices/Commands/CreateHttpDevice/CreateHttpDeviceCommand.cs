using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.HttpDevices.Commands.CreateHttpDevice
{
    public class CreateHttpDeviceCommand : IRequest<string>
    {
        public string IpAddress { get; set; }
        public int PortNumber { get; set; }
    }

    public class CreateHttpDeviceCommandHandler : IRequestHandler<CreateHttpDeviceCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateHttpDeviceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(CreateHttpDeviceCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.HttpDevice.AddAsync(new HttpDevice()
            {
                IpAddress = request.IpAddress,
                PortNumber = request.PortNumber,
                LastModifiedDate = DateTime.Now
            });
            await _unitOfWork.SaveAsync();
            var device = await _unitOfWork.HttpDevice.GetFirstOrDefaultAsync(d => d.IpAddress == request.IpAddress);
            return device.Id;
        }
    }
}
