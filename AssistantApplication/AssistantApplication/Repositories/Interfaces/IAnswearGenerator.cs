using Microsoft.VisualBasic;

namespace AssistantApplication.Repositories.Interfaces
{
    public interface IAnswearGenerator
    {
        public Task<string> GetAnswear(string questionText, CancellationToken cancellationToken);
    }
}
