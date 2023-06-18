using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Miaocrosoft.GPT.Data;

public class ChatHistoryStorage
{
    const string CHAT_HISTORY_STORAGE = "ChatHistoryStorage";
    ProtectedLocalStorage _protectedSessionStore;
    ChatHistory _chatHistory;
    int _current;

    public ChatHistoryStorage(ProtectedLocalStorage protectedSessionStore)
    {
        this._protectedSessionStore = protectedSessionStore;
    }

    public async Task LoadHistory()
    {
        var result = await _protectedSessionStore.GetAsync<ChatHistory>(CHAT_HISTORY_STORAGE);
        if (!result.Success || result.Value == null)
        {
            _chatHistory = new ChatHistory();
            this.AddNew();
        }
        else
        {
            _chatHistory = result.Value;
        }
    }

    public async Task<List<List<ChatMessage>>> GetAll()
    {
        return _chatHistory.History;
    }

    public async Task<List<ChatMessage>> SetCurrent(List<ChatMessage> title = null)
    {
        if (title == null)
        {
            _current = 0;
            return _chatHistory.History.FirstOrDefault();
        }
        else
        {
            return _chatHistory.History.FirstOrDefault(m => m == title);
        }
    }

    public async Task<List<ChatMessage>> GetCurrent()
    {
        return _chatHistory.History[_current];
    }

    public async Task<List<ChatMessage>> AddNew()
    {
        _chatHistory.History.Insert(0, new List<ChatMessage>());
        return await this.SetCurrent();
    }

    public async Task SaveChangeOnCurrentAsync()
    {
        await _protectedSessionStore.SetAsync(CHAT_HISTORY_STORAGE, _chatHistory);
    }
}
