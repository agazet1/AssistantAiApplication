using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AssistantApplication.Data;
using AssistantApplication.Data.Models;
using AssistantApplication.Repositories.Interfaces;

namespace AssistantApplication.Repositories.Implementations
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _dbContext;

        public ChatRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Message>> GetMessageHistory(int? lastMessageId, int takeNo, CancellationToken cancellationToken)
        {
            Expression<Func<Message, bool>> predicate = lastMessageId is null ? (x => true) : (x => x.Id < lastMessageId);

            var msgList = await _dbContext.Messages
                .Where(predicate)
                .OrderByDescending(x => x.Id)
                .Take(takeNo)
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);
            return msgList;
        }

        public async Task<bool> RateMessage(int messageId, int rate, CancellationToken cancellationToken)
        {
            var msg = _dbContext.Messages.FirstOrDefault(x => x.Id == messageId);
            if (msg is null)
            {
                throw new Exception("No message");
            }

            msg.Rate = rate;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<int> SaveMessage(Message message, CancellationToken cancellationToken)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return message.Id;
        }

        public async Task<int> SetMessageReaded(Message message, CancellationToken cancellationToken)
        {
            var msg = _dbContext.Messages.FirstOrDefault(x => x.Id == message.Id);
            if (msg is not null)
            {
                msg.IsReaded = true;
                msg.Text = message.Text;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                message.IsReaded = true;
                message.Date = DateTime.Now;
                message.Text = message.Text;
                _dbContext.Messages.Add(message);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return message.Id;
        }
    }
}
