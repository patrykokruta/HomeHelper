using HomeHelper.Common.Enums;
using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.MqttMessages.PublishingMessages.Commands
{
    public class CreateMqttPublishingMessageCommand : IRequest<string>
    {
        public string Topic { get; set; }
        public string ClientId { get; set; }
        public string ModelName { get; set; }
        public PublishType PublishType { get; set; }
    }
    public class CreateMqttPublishingMessageCommandHandler : IRequestHandler<CreateMqttPublishingMessageCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMqttPublishingMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(CreateMqttPublishingMessageCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid().ToString();
            await _unitOfWork.PublishMessage.AddAsync(new MqttPublishingMessage()
            {
                Id = id,
                ClientId = request.ClientId,
                Topic = request.Topic,
                ModelName = request.ModelName,
                PublishType = request.PublishType
            });
            await _unitOfWork.SaveAsync();
            var publishMessage = await _unitOfWork.PublishMessage.GetAsync(id);
            return publishMessage.Id;
        }
    }
}
