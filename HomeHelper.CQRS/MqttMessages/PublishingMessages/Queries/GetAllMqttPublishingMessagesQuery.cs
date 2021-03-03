using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.MqttMessages.PublishingMessages.Queries
{
    public class GetAllMqttPublishingMessagesQuery : IRequest<IEnumerable<MqttPublishingMessage>>
    {
    }
    public class GetAllMqttPublishingMessagesQueryHandler : IRequestHandler<GetAllMqttPublishingMessagesQuery, IEnumerable<MqttPublishingMessage>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMqttPublishingMessagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MqttPublishingMessage>> Handle(GetAllMqttPublishingMessagesQuery request, CancellationToken cancellationToken)
        {
            var publishingMessages = await _unitOfWork.PublishMessage.GetAllAsync();
            return publishingMessages;
        }
    }
}
