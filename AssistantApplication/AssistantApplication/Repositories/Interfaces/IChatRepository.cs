using AssistantApplication.Data.Models;

namespace AssistantApplication.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public Task<bool> RateMessage(int messageId, int rate, CancellationToken cancellationToken);
        public Task<int> SaveMessage(Message message, CancellationToken cancellationToken);
        public Task<int> SetMessageReaded(Message message, CancellationToken cancellationToken);
        public Task<List<Message>> GetMessageHistory(int? lastMessageId, int takeNo, CancellationToken cancellationToken);
    }
}
