using HomeHelper.Domain;
using HomeHelper.Repositories;
using HomeHelper.Common.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.Rooms.Commands.CreateRoom
{
    public class CreateRoomCommand : IRequest<string>
    {
        [Display(Name = "Room name:")]
        [Required]
        public string Name { get; set; }
    }

    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room() { Name = request.Name, LastModifiedDate = DateTime.Now };
            await _unitOfWork.Room.AddAsync(room);
            await _unitOfWork.SaveAsync();
            return room.Id;
        }
    }
}
