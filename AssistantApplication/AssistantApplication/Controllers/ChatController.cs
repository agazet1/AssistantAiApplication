using MediatR;
using Microsoft.AspNetCore.Mvc;
using AssistantApplication.DTOs;
using AssistantApplication.Features.Chat.Command;
using AssistantApplication.Features.Chat.Query;

namespace AssistantApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<List<MessageDto>> GetHistory([FromQuery] int lastId = -1, [FromQuery] int takeNo = 10, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _mediator.Send(new GetHistory(lastId, takeNo), cancellationToken);
        }

        [HttpPost]
        public Task<MessageAnswearDto> AskQuestion([FromBody] MessageDto request, CancellationToken cancellationToken)
        {
            return _mediator.Send(new AskQuestion(request), cancellationToken);
        }

        [HttpPost]
        public Task<MessageDto> SaveAnswear([FromBody] MessageDto request, CancellationToken cancellationToken)
        {
            return _mediator.Send(new SaveAnswear(request), cancellationToken);
        }

        [HttpPost]
        public Task<bool> RateMessage([FromBody] RateMessageDto request, CancellationToken cancellationToken)
        {
            return _mediator.Send(new RateMessage(request), cancellationToken);
        }
    }
}
