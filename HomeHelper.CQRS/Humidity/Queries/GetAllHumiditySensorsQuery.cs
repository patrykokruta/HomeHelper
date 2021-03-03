using HomeHelper.Common.Enums;
using HomeHelper.DTO.Devices;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Humidity.Queries
{
    public class GetAllHumiditySensorsQuery : IRequest<IEnumerable<HumiditySensor>>
    {
    }
    public class GetAllHumiditySensorsQueryHandler : IRequestHandler<GetAllHumiditySensorsQuery, IEnumerable<HumiditySensor>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllHumiditySensorsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<HumiditySensor>> Handle(GetAllHumiditySensorsQuery request, CancellationToken cancellationToken)
        {
            var devices = await _unitOfWork.Device.GetAllAsync(
                d => d.Type == DeviceType.HumiditySensor || d.Type == DeviceType.TemperatureAndHumiditySensor);
            var http = await _unitOfWork.HttpDevice.GetAllAsync();
            var mqtt = await _unitOfWork.MqttDevice.GetAllAsync(d => d.IsConfirmed == true);
            var zigbee = await _unitOfWork.ZigbeeDevice.GetAllAsync(d => d.IsConfirmed == true);
            var rooms = await _unitOfWork.Room.GetAllAsync();

            var humiditySensors = (from d in devices
                                   join r in rooms on d.RoomId equals r.Id
                                   join z in zigbee on d.ConnectedDeviceId equals z.Id into mz
                                   from c in mz.DefaultIfEmpty()
                                   join h in http on d.ConnectedDeviceId equals h.Id into rh
                                   from a in rh.DefaultIfEmpty()
                                   join m in mqtt on d.ConnectedDeviceId equals m.Id into hm
                                   from b in hm.DefaultIfEmpty()

                                   select new HumiditySensor()
                                   {
                                       Id = d.ConnectedDeviceId,
                                       Name = d.Name,
                                       RoomName = r.Name,
                                       Protocol = d.Protocol
                                   });
            return humiditySensors;
        }
    }
}
