using MediatR;
using AssistantApplication.DTOs;

namespace AssistantApplication.Features.Chat.Command
{
    public record AskQuestion(MessageDto QuestionMsg) : IRequest<MessageAnswearDto>
    {
    }
}
