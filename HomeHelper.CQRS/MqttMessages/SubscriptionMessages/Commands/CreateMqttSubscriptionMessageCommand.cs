using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.MqttMessages.SubscriptionMessages.Commands
{
    public class CreateMqttSubscriptionMessageCommand : IRequest<string>
    {
        public string Topic { get; set; }
        public string ClientId { get; set; }
        public string ModelName { get; set; }
    }
    public class CreateMqttSubscriptionMessageCommandHandler : IRequestHandler<CreateMqttSubscriptionMessageCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMqttSubscriptionMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(CreateMqttSubscriptionMessageCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid().ToString();
            await _unitOfWork.SubscribeMessage.AddAsync(
                new MqttSubscriptionMessage() { Id = id, ClientId = request.ClientId, ModelName = request.ModelName, Topic = request.Topic });
            await _unitOfWork.SaveAsync();
            var subscribeMessage = await _unitOfWork.SubscribeMessage.GetAsync(id);
            return subscribeMessage.Id;
        }
    }
}
