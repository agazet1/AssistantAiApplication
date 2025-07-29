using MediatR;
using AssistantApplication.DTOs;

namespace AssistantApplication.Features.Chat.Command
{
    public record SaveAnswear(MessageDto AnswearMsg) : IRequest<MessageDto>
    {
    }
}
