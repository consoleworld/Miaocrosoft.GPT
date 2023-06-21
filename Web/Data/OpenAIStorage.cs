using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Miaocrosoft.GPT.Data;

public class OpenAIStorage
{
    const string OPENAI_STORAGE = "OpenAIStorage";
    OpenAIClient? openAIClient;
    ProtectedLocalStorage protectedSessionStore;

    public OpenAIStorage(ProtectedLocalStorage protectedSessionStore)
    {
        this.protectedSessionStore = protectedSessionStore;
    }

    public async Task<OpenAIClient?> GetClient()
    {
        if (this.openAIClient == null)
        {
            string? token = await this.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                this.openAIClient = new OpenAIClient(token, new OpenAIClientOptions());
            }
        }
        return this.openAIClient;
    }

    public async Task SetToken(string token)
    {
        await protectedSessionStore.SetAsync(OPENAI_STORAGE, token);
    }

    public async Task<string?> GetToken()
    {
        try
        {
            ProtectedBrowserStorageResult<string> result = await protectedSessionStore.GetAsync<string>(OPENAI_STORAGE);
            if (result.Success)
            {
                return result.Value;
            }
        }
        catch (System.Exception)
        {

        }
        return string.Empty;
    }
}
