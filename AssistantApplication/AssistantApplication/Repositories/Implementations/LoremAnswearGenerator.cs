using AssistantApplication.Repositories.Interfaces;
using LoremNET;

namespace AssistantApplication.Repositories.Implementations
{
    public class LoremAnswearGenerator : IAnswearGenerator
    {
        public Task<string> GetAnswear(string questionText, CancellationToken cancellationToken)
        {
            Random rnd = new Random();

            int paragraphNo = rnd.Next(0, 5);
            int sentenceNo = rnd.Next(1, 5);

            string msgText = "";
            switch (paragraphNo)
            {
                case 0:
                    {
                        msgText = Lorem.Sentence(4, 10);
                        break;
                    }
                case 1:
                    {
                        msgText = Lorem.Paragraph(4, 10, sentenceNo);
                        break;
                    }
                default:
                    {
                        msgText = String.Join("<br>", Lorem.Paragraphs(4, 10, sentenceNo, paragraphNo));
                        break;
                    }
            }

            return Task.FromResult(msgText);
        }
    }
}
