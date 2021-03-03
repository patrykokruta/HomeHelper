using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.ZigbeeDevices.Commands.CreateZigbeeDevice
{
    public class CreateZigbeeDeviceCommand : IRequest<string>
    {
        public string Id { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class CreateZigbeeDeviceCommandHandler : IRequestHandler<CreateZigbeeDeviceCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateZigbeeDeviceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(CreateZigbeeDeviceCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ZigbeeDevice.AddAsync(
                new ZigbeeDevice()
                {
                    Id = request.Id,
                    IsAvailable = request.IsAvailable,
                    IsConfirmed = request.IsConfirmed,
                    LastModifiedDate = DateTime.Now
                });
            await _unitOfWork.SaveAsync();
            return request.Id;
        }
    }
}
