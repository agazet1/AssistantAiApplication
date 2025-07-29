using Microsoft.AspNetCore.Server.HttpSys;
using AssistantApplication.DTOs;

namespace AssistantApplication.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int UserIdFrom { get; set; }
        public int UserIdTo { get; set; }
        public string Text { get; set; } = "";
        public DateTime Date { get; set; }
        public int Rate {  get; set; }
        public bool IsReaded { get; set; }

        public MessageDto ToDto()
        {
            return new MessageDto()
            {   
                Id = Id,
                UserIdFrom = UserIdFrom,
                UserIdTo = UserIdTo,
                Text = Text,
                Date = Date,
                Rate = Rate,
                IsReaded = IsReaded
            };
        }
        public MessageAnswearDto ToAnswearDto()
        {
            return new MessageAnswearDto()
            {
                Id = Id,
                UserIdFrom = UserIdFrom,
                UserIdTo = UserIdTo,
                Text = Text,
                Date = Date,
                Rate = Rate,
                IsReaded = IsReaded,
                QuestionId = 0
            };
        }
    }
}
