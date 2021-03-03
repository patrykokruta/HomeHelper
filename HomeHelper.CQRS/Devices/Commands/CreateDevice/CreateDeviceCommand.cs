using HomeHelper.Common.Enums;
using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Devices.Commands.CreateDevice
{
    public class CreateDeviceCommand : IRequest<string>
    {
        #region Common properties
        [Required]
        [Display(Name = "Device name:")]
        public string Name { get; set; }

        [Display(Name = "Room name:")]
        public string RoomId { get; set; }

        public IEnumerable<Room> Rooms { get; set; }

        [Required]
        [Display(Name = "Device model:")]
        public string ModelName { get; set; }

        [Required]
        [Display(Name = "Communication protocol:")]
        public ProtocolType Protocol { get; set; }

        [Required]
        [Display(Name = "Device type:")]
        public DeviceType Type { get; set; }

        [Display(Name = "Choose connected device:")]
        public string ConnectedDeviceId { get; set; }
        #endregion

        #region Http only properties
        [Display(Name = "IP address:")]
        public string IpAddress { get; set; }

        [Display(Name = "Port number:")]
        public int PortNumber { get; set; }
        #endregion
    }
    public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDeviceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Device.AddAsync(
                new Device()
                {
                    Name = request.Name,
                    ModelName = request.ModelName,
                    Protocol = request.Protocol,
                    Type = request.Type,
                    ConnectedDeviceId = request.ConnectedDeviceId,
                    RoomId = request.RoomId,
                    LastModifiedDate = DateTime.Now
                });

            if (request.Protocol == ProtocolType.Mqtt)
            {
                var mqttDevice = await _unitOfWork.MqttDevice.GetAsync(request.ConnectedDeviceId);
                mqttDevice.IsConfirmed = true;
                mqttDevice.LastModifiedDate = DateTime.Now;
                _unitOfWork.MqttDevice.Update(mqttDevice);
            }
            if (request.Protocol == ProtocolType.Zigbee)
            {
                var zigbeeDevice = await _unitOfWork.ZigbeeDevice.GetAsync(request.ConnectedDeviceId);
                zigbeeDevice.IsConfirmed = true;
                zigbeeDevice.LastModifiedDate = DateTime.Now;
                _unitOfWork.ZigbeeDevice.Update(zigbeeDevice);
            }
            await _unitOfWork.SaveAsync();
            var device = await _unitOfWork.Device.GetFirstOrDefaultAsync(d => d.Name == request.Name);


            return device.Id;
        }
    }
}
