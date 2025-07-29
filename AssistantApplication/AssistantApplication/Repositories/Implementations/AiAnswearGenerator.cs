using AssistantApplication.Repositories.Interfaces;

namespace AssistantApplication.Repositories.Implementations
{
    public class AiAnswearGenerator : IAnswearGenerator
    {
        public Task<string> GetAnswear(string questionText, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
