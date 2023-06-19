using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Miaocrosoft.GPT.Data;

public class ChatHistoryStorage
{
    const string CHAT_HISTORY_STORAGE = "ChatHistoryStorage";
    ProtectedLocalStorage _protectedSessionStore;
    ChatHistory _chatHistory;
    int? _current;

    public ChatHistoryStorage(ProtectedLocalStorage protectedSessionStore)
    {
        this._protectedSessionStore = protectedSessionStore;
    }

    public async Task LoadHistory()
    {
        if (_chatHistory != null)
        {
            return;
        }
        ProtectedBrowserStorageResult<ChatHistory> result;
        try
        {
            result = await _protectedSessionStore.GetAsync<ChatHistory>(CHAT_HISTORY_STORAGE);
            if (!result.Success || result.Value == null)
            {
                throw new Exception($"Read storage {CHAT_HISTORY_STORAGE} failed");
            }
            else
            {
                _chatHistory = result.Value;
            }
        }
        catch (System.Exception)
        {
            _chatHistory = new ChatHistory();
        }
    }

    public async Task<List<List<ChatMessageDTO>>> GetAll()
    {
        return _chatHistory.History;
    }

    public async Task<List<ChatMessageDTO>?> SetCurrent(List<ChatMessageDTO>? target)
    {
        if (target == null)
        {
            _current = null;
        }
        else
        {
            _current = _chatHistory.History.IndexOf(target);
            if (_current == -1)
            {
                _current = _chatHistory.History.Any() ? 0 : null;
            }
        }
        if (_current.HasValue)
        {
            return _chatHistory.History[_current.Value];
        }
        else
        {
            return null;
        }
    }

    public async Task<List<ChatMessageDTO>> GetCurrent()
    {
        if (_chatHistory.History.Any() && _current.HasValue)
        {
            return _chatHistory.History[_current.Value];
        }
        else
        {
            return null;
        }
    }

    public async Task<List<ChatMessageDTO>> AddNew(List<ChatMessageDTO> chat)
    {
        _chatHistory.History.Insert(0, chat);
        return await this.SetCurrent(null);
    }

    public async Task<List<ChatMessageDTO>> Delete(List<ChatMessageDTO> chat)
    {
        var current = await this.GetCurrent();
        _chatHistory.History.Remove(chat);
        await this.SaveChangeOnCurrentAsync();
        return await this.SetCurrent(current);
    }

    public async Task SaveChangeOnCurrentAsync()
    {
        await _protectedSessionStore.SetAsync(CHAT_HISTORY_STORAGE, _chatHistory);
    }
}
