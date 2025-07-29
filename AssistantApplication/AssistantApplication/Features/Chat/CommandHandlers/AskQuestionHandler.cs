using MediatR;
using AssistantApplication.Data.Models;
using AssistantApplication.DTOs;
using AssistantApplication.Features.Chat.Command;
using AssistantApplication.Repositories.Interfaces;

namespace AssistantApplication.Features.Chat.CommandHandlers
{
    internal class AskQuestionHandler : IRequestHandler<AskQuestion, MessageAnswearDto>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IAnswearGenerator _answearGenerator;

        public AskQuestionHandler(
            IChatRepository chatRepository,
            IAnswearGenerator answearGenerator)
        {
            _chatRepository = chatRepository;
            _answearGenerator = answearGenerator;
        }

        public async Task<MessageAnswearDto> Handle(AskQuestion request, CancellationToken cancellationToken)
        {
            var userAskId = request.QuestionMsg.UserIdFrom > 0 ? request.QuestionMsg.UserIdFrom : User.USER_USER_ID;
            var userAnswearId = request.QuestionMsg.UserIdTo > 0 ? request.QuestionMsg.UserIdTo : User.USER_BOT_ID;

            var questionMsg = new Message()
            {
                Date = request.QuestionMsg.Date,
                UserIdFrom = userAskId,
                UserIdTo = userAnswearId,
                Text = request.QuestionMsg.Text,
                IsReaded = true
            };

            var questionId = await _chatRepository.SaveMessage(questionMsg, cancellationToken);
            if (questionId < 1)
            {
                throw new Exception($"Failed to save a question: {questionMsg.Text}");
            }

            var answearTxt = await _answearGenerator.GetAnswear(questionMsg.Text, cancellationToken);
            var answear = new Message()
            {
                Date = DateTime.Now,
                UserIdFrom = userAnswearId,
                UserIdTo = userAskId,
                Text = answearTxt,
                IsReaded = false
            };

            var answearId = await _chatRepository.SaveMessage(answear, cancellationToken);
            if (answearId < 1)
            {
                throw new Exception($"Failed to save a full answear: {answear.Text}");
            }
            answear.Id = answearId;

            MessageAnswearDto answearDto = answear.ToAnswearDto();
            answearDto.QuestionId = questionId;
            return answearDto;
        }
    }
}
