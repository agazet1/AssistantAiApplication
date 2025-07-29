using MediatR;
using AssistantApplication.DTOs;

namespace AssistantApplication.Features.Chat.Command
{
    public record RateMessage(RateMessageDto Rate): IRequest<bool>
    {
    }
}
