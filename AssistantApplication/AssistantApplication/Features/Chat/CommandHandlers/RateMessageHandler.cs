using MediatR;
using AssistantApplication.Features.Chat.Command;
using AssistantApplication.Repositories.Interfaces;

namespace AssistantApplication.Features.Chat.CommandHandlers
{
    internal class RateMessageHandler : IRequestHandler<RateMessage, bool>
    {
        private readonly IChatRepository _chatRepository;

        public RateMessageHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<bool> Handle(RateMessage request, CancellationToken cancellationToken)
        {
            return await _chatRepository.RateMessage(request.Rate.MessageId, request.Rate.Rate, cancellationToken);
        }
    }
}
