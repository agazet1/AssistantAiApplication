using AssistantApplication.Data.Models;
using AssistantApplication.DTOs;
using AssistantApplication.Features.Chat.Command;
using AssistantApplication.Features.Chat.CommandHandlers;
using AssistantApplication.Repositories.Implementations;
using AssistantApplication.Repositories.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace AssistantApplication.Tests.CommandHandlers
{
    public class AskQuestionHandlerTest
    {
        private readonly Mock<IChatRepository> _chatRepository;
        private readonly Mock<IAnswearGenerator> _answearGenerator;
        private MessageDto _messageDto;

        public AskQuestionHandlerTest()
        {
            _chatRepository = new Mock<IChatRepository>();
            _answearGenerator = new Mock<IAnswearGenerator>();

            Setup();
        }

        [Fact]
        public async Task Handle_Should_Save_Question_And_Message()
        {
            var handler = GetHandler();

            _chatRepository.Setup(x => x.SaveMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Random().Next(1, 100));
            _answearGenerator.Setup(x => x.GetAnswear(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("Answear");

            var request = new AskQuestion(_messageDto);

            var result = await handler.Handle(request, default);

            _chatRepository.Verify(x => x.SaveMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_Should_Get_Answear()
        {
            var handler = GetHandler();

            _chatRepository.Setup(x => x.SaveMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Random().Next(1, 100));
            _answearGenerator.Setup(x => x.GetAnswear(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("Answear");

            var request = new AskQuestion(_messageDto);
            var result = await handler.Handle(request, default);

            _answearGenerator.Verify(x => x.GetAnswear(It.IsAny<string>(), It.IsAny<CancellationToken>()));
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Text));
        }

        [Fact]
        public async Task Handle_Should_Return_Generated_Id()
        {
            var handler = GetHandler();

            _chatRepository.Setup(x => x.SaveMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Random().Next(1, 100));
            _answearGenerator.Setup(x => x.GetAnswear(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("Answear");

            var request = new AskQuestion(_messageDto);

            var result = await handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.True(result.QuestionId > 0);
        }


        private AskQuestionHandler GetHandler()
        {
            return new AskQuestionHandler(_chatRepository.Object, _answearGenerator.Object);
        }

        private void Setup()
        {
            _messageDto = new MessageDto()
            {
                Id = 0,
                UserIdTo = 1,
                UserIdFrom = 2,
                Text = "My question",
                Date = DateTime.Now,
                IsReaded = false,
                Rate = 0
            };
        }
    }
}
