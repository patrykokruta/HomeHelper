using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.ZigbeeDevices.Commands.UpdateZigbeeDevice
{
    public class UpdateZigbeeDeviceCommand : IRequest<string>
    {
        public string Id { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class UpdateZigbeeDeviceCommandHandler : IRequestHandler<UpdateZigbeeDeviceCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateZigbeeDeviceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(UpdateZigbeeDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = await _unitOfWork.ZigbeeDevice.GetAsync(request.Id);

            device.Id = request.Id;
            device.IsAvailable = request.IsAvailable;
            device.IsConfirmed = request.IsConfirmed;
            device.LastModifiedDate = DateTime.Now;

            _unitOfWork.ZigbeeDevice.Update(device);
            await _unitOfWork.SaveAsync();
            return request.Id;
        }
    }
}
