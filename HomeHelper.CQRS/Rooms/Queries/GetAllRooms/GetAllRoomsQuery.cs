using HomeHelper.Domain;
using HomeHelper.Repositories;
using HomeHelper.Common.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Rooms.Queries.GetAllRooms
{
    public class GetAllRoomsQuery : IRequest<IEnumerable<Room>> { }

    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, IEnumerable<Room>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllRoomsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Room>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Room.GetAllAsync();
            return result;
        }
    }
}
