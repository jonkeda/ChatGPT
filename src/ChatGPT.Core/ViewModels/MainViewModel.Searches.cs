using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ChatGPT.ViewModels.Chat;
using ChatGPT.ViewModels.Search;
using CommunityToolkit.Mvvm.Input;

namespace ChatGPT.ViewModels;

public partial class MainViewModel
{
    private ObservableCollection<FoundChatViewModel> _foundChats;
    private FoundChatNode? _currentFoundChat;
    private string? _searchText;

    [JsonIgnore]
    public ObservableCollection<FoundChatViewModel> FoundChats
    {
        get => _foundChats;
        set => SetProperty(ref _foundChats, value);
    }

    [JsonIgnore]
    public FoundChatNode? CurrentFoundChat
    {
        get => _currentFoundChat;
        set
        {
            if (SetProperty(ref _currentFoundChat, value))
            {
                SelectChat(CurrentFoundChat);
            }
        }
    }

    private void SelectChat(FoundChatNode? currentFoundChat)
    {
        ChatViewModel? chat = null;
        ChatMessageViewModel? chatMessage = null;
        if (currentFoundChat is FoundChatViewModel foundChat)
        {
            chat = foundChat.Chat;
        }
        else if (currentFoundChat is FoundChatMessageViewModel foundMessage)
        {
            chat = foundMessage.Chat;
            chatMessage = foundMessage.Message;
        }

        if (chat != null)
        {
            CurrentChat = chat;
            if (chatMessage != null)
            {
                chat.CurrentMessage = chatMessage;
            }
        }

    }

    [JsonIgnore]
    public string? SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    [JsonIgnore]
    public IAsyncRelayCommand SearchChatCommand { get; }

    private async Task SearchChatActionAsync()
    {
        SearchChatCallback();
        await Task.Yield();
    }

    private void SearchChatCallback()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            return;
        }

        FoundChatViewModel? foundChat = null;
        ObservableCollection<FoundChatViewModel> foundChats = new();
        foreach (var chat in Chats)
        {
            foreach (var message in chat.Messages)
            {
                if (message.Message != null
                    && message.Message.IndexOf(_searchText!, StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    if (foundChat == null)
                    {
                        foundChat = new FoundChatViewModel(chat);
                    }

                    foundChat.Messages.Add(new FoundChatMessageViewModel(chat, message) );
                }
            }

            if (foundChat != null)
            {
                foundChats.Add(foundChat);
            }
        }

        FoundChats = foundChats;
    }
}
