using HomeHelper.Domain;
using HomeHelper.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.CQRS.MqttMessages.SubscribeMessages.Queries
{
    public class GetAllMqttSubscriptionMessagesQuery : IRequest<IEnumerable<MqttSubscriptionMessage>>
    {
    }
    public class GetAllMqttSubscriptionMessagesQueryHandler : IRequestHandler<GetAllMqttSubscriptionMessagesQuery, IEnumerable<MqttSubscriptionMessage>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMqttSubscriptionMessagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MqttSubscriptionMessage>> Handle(GetAllMqttSubscriptionMessagesQuery request, CancellationToken cancellationToken)
        {
            var subscribeMessages = await _unitOfWork.SubscribeMessage.GetAllAsync();
            return subscribeMessages;
        }
    }
}
