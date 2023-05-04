using System;
using System.Text.Json.Serialization;
using ChatGPT.ViewModels.Chat;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatGPT.ViewModels.Search;

public class FoundChatMessageViewModel : ObservableObject, FoundChatNode
{
    public ChatViewModel Chat { get; }
    private ChatMessageViewModel _message;

    public FoundChatMessageViewModel(ChatViewModel chat, ChatMessageViewModel message)
    {
        Chat = chat;
        _message = message;
        Title = GetTitle(message.Message);
    }

    private string? GetTitle(string? msg)
    {
        if (msg == null)
        {
            return null;
        }

        int i = msg.IndexOf('\n');
        if (i < 0)
        {
            i = msg.Length;
        }

        return msg.Substring(0, Math.Min(i, 50));
    }

    [JsonIgnore]
    public ChatMessageViewModel Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public string? Title { get; set; }
}
