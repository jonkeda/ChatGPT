using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatGPT.ViewModels.Chat;

public class ChatResultViewModel : ObservableObject
{
    private string? _message;
    private bool _isError;
    private int _promptTokens;
    private int _completionTokens;
    private int _totalTokens;

    [JsonPropertyName("name")]
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [JsonPropertyName("isError")]
    public bool IsError
    {
        get => _isError;
        set => SetProperty(ref _isError, value);
    }

    [JsonPropertyName("promptTokens")]
    public int PromptTokens
    {
        get => _promptTokens;
        set => SetProperty(ref _promptTokens, value);
    }

    [JsonPropertyName("completionTokens")]
    public int CompletionTokens
    {
        get => _completionTokens;
        set => SetProperty(ref _completionTokens, value);
    }

    [JsonPropertyName("totalTokens")]
    public int TotalTokens
    {
        get => _totalTokens;
        set => SetProperty(ref _totalTokens, value);
    }
}
