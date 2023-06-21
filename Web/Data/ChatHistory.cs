using Azure.AI.OpenAI;

namespace Miaocrosoft.GPT.Data;

public class ChatHistory
{
    public List<List<ChatMessageDTO>> History { get; set; } = new List<List<ChatMessageDTO>>();
}

public class ChatMessageDTO
{
    public string? Role { get; set; }
    public string? Content { get; set; }
    public ChatMessage ToChatMessage() => new ChatMessage(Role, Content);    
}