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
        finally
        {
            this.OnChange?.Invoke();
        }
    }

    public async Task<List<List<ChatMessageDTO>>> GetAll()
    {
        if (_chatHistory?.History == null)
        {
            throw new ArgumentNullException(nameof(_chatHistory.History));
        }
        await Task.Yield();
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

        this.OnChange?.Invoke();

        if (_current.HasValue)
        {
            return _chatHistory.History[_current.Value];
        }
        else
        {
            return null;
        }
    }

    public async Task<List<ChatMessageDTO>?> GetCurrent()
    {
        await Task.Yield();
        if (_chatHistory.History.Any() && _current.HasValue)
        {
            return _chatHistory.History[_current.Value];
        }
        else
        {
            return null;
        }
    }

    public async Task<List<ChatMessageDTO>?> AddNew(List<ChatMessageDTO> chat)
    {
        _chatHistory.History.Insert(0, chat);
        await SetCurrent(chat);
        this.OnChange?.Invoke();
        return await this.GetCurrent();
    }

    public async Task<List<ChatMessageDTO>> Delete(List<ChatMessageDTO> chat)
    {
        var current = await this.GetCurrent();
        _chatHistory.History.Remove(chat);
        await this.SaveChangeOnCurrentAsync();
        await this.SetCurrent(current);
        this.OnChange?.Invoke();
        return await GetCurrent();
    }

    public async Task SaveChangeOnCurrentAsync()
    {
        await _protectedSessionStore.SetAsync(CHAT_HISTORY_STORAGE, _chatHistory);
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}

