using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using ChatGPT.ViewModels.Chat;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatGPT.ViewModels.Search;

public class FoundChatViewModel :  ObservableObject, FoundChatNode
{
    private ChatViewModel _chat;
    private ObservableCollection<FoundChatMessageViewModel> _messages = new ();

    public FoundChatViewModel(ChatViewModel chat)
    {
        _chat = chat;
    }

    [JsonIgnore]
    public ChatViewModel Chat
    {
        get => _chat;
        set => SetProperty(ref _chat, value);
    }

    [JsonIgnore]
    public ObservableCollection<FoundChatMessageViewModel> Messages
    {
        get => _messages;
        set => SetProperty(ref _messages, value);
    }
}
