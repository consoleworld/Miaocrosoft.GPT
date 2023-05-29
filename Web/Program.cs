using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Miaocrosoft.GPT.Data;
using Azure.AI.OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

string nonAzureOpenAIApiKey = builder.Configuration.GetValue(typeof(string), "api-key-from-platform.openai.com")?.ToString() ?? throw new ArgumentNullException("api-key-from-platform.openai.com");
builder.Services.AddSingleton(new OpenAIClient(nonAzureOpenAIApiKey, new OpenAIClientOptions()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
