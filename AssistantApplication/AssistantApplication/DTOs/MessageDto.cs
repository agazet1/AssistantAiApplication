namespace AssistantApplication.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int UserIdFrom { get; set; }
        public int UserIdTo { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; } = "";
        public int Rate { get; set; }
        public bool IsReaded { get; set; }
    }
}
