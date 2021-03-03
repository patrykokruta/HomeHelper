using HomeHelper.Common.Enums;
using HomeHelper.DTO.Devices;
using HomeHelper.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.ContactSensors.Queries
{
    public class GetAllContactSensorsQuery : IRequest<IEnumerable<ContactSensor>>
    {
    }
    public class GetAllContactSensorsQueryHandler : IRequestHandler<GetAllContactSensorsQuery, IEnumerable<ContactSensor>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllContactSensorsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ContactSensor>> Handle(GetAllContactSensorsQuery request, CancellationToken cancellationToken)
        {
            var devices = await _unitOfWork.Device.GetAllAsync(
                d => d.Type == DeviceType.ContactSensor);
            var http = await _unitOfWork.HttpDevice.GetAllAsync();
            var mqtt = await _unitOfWork.MqttDevice.GetAllAsync(d => d.IsConfirmed == true);
            var zigbee = await _unitOfWork.ZigbeeDevice.GetAllAsync(d => d.IsConfirmed == true);
            var rooms = await _unitOfWork.Room.GetAllAsync();

            var contactSensors = (from d in devices
                                  join r in rooms on d.RoomId equals r.Id
                                  join z in zigbee on d.ConnectedDeviceId equals z.Id into mz
                                  from c in mz.DefaultIfEmpty()
                                  join h in http on d.ConnectedDeviceId equals h.Id into rh
                                  from a in rh.DefaultIfEmpty()
                                  join m in mqtt on d.ConnectedDeviceId equals m.Id into hm
                                  from b in hm.DefaultIfEmpty()

                                  select new ContactSensor()
                                  {
                                      Id = d.ConnectedDeviceId,
                                      Name = d.Name,
                                      RoomName = r.Name,
                                      Protocol = d.Protocol
                                  });
            return contactSensors;
        }
    }
}
