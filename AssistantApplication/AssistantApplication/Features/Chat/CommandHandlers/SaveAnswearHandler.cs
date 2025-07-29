using MediatR;
using AssistantApplication.Data.Models;
using AssistantApplication.DTOs;
using AssistantApplication.Features.Chat.Command;
using AssistantApplication.Repositories.Interfaces;

namespace AssistantApplication.Features.Chat.CommandHandlers
{
    internal class SaveAnswearHandler : IRequestHandler<SaveAnswear, MessageDto>
    {
        private readonly IChatRepository _chatRepository;

        public SaveAnswearHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<MessageDto> Handle(SaveAnswear request, CancellationToken cancellationToken)
        {
            var answearMsg = new Message()
            {
                Id = request.AnswearMsg.Id,
                Date = request.AnswearMsg.Date,
                UserIdFrom = request.AnswearMsg.UserIdFrom,
                UserIdTo = request.AnswearMsg.UserIdTo,
                Text = request.AnswearMsg.Text
            };

            var answearId = await _chatRepository.SetMessageReaded(answearMsg, cancellationToken);
            if (answearId < 1)
            {
                throw new Exception("Failed to write down the answer");
            }
            request.AnswearMsg.Id = answearId;

            return request.AnswearMsg;
        }
    }
}
