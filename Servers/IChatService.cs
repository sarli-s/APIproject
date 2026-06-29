using DTOs;

namespace Servers
{
    public interface IChatService
    {
        Task<ChatResponse> SendMessageAsync(ChatRequest req);
    }
}
