using MediatR;
using AssistantApplication.DTOs;
using AssistantApplication.Repositories.Interfaces;
using AssistantApplication.Features.Chat.Query;

namespace AssistantApplication.Features.Chat.QueryHandlers
{
    internal class GetHistoryHandler : IRequestHandler<GetHistory, List<MessageDto>>
    {
        private readonly IChatRepository _chatRepository;

        public GetHistoryHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<List<MessageDto>> Handle(GetHistory request, CancellationToken cancellationToken)
        {
            List<MessageDto> msgDtoList = new List<MessageDto>();
            int? lastId = request.lastId < 0 ? null : request.lastId;
            
            var result = await _chatRepository.GetMessageHistory(lastId, request.takeNo, cancellationToken);
            result.ForEach(message => msgDtoList.Add(message.ToDto()));

            return msgDtoList;
        }
    }
}
