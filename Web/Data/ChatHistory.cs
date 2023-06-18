using Azure.AI.OpenAI;

namespace Miaocrosoft.GPT.Data;

public class ChatHistory
{
    public List<List<ChatMessage>> History { get; set; } = new List<List<ChatMessage>>();
}