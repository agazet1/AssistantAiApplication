using MediatR;
using AssistantApplication.DTOs;

namespace AssistantApplication.Features.Chat.Query
{
    public record GetHistory(int lastId, int takeNo) : IRequest<List<MessageDto>>
    {
    }
}
