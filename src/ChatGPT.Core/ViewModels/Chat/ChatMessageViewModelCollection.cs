using System.Collections.ObjectModel;

namespace ChatGPT.ViewModels.Chat;

public class ChatMessageViewModelCollection : ObservableCollection<ChatMessageViewModel>
{
    private ChatViewModel? _chat;
    public ChatViewModel Chat
    {
        set
        {
            _chat = value;
            foreach (var message in this)
            {
                _chat.SetMessageActions(message);
            }
        }
    }
    public ChatMessageViewModelCollection()
    {}

    public ChatMessageViewModelCollection(ChatViewModel chat)
    {
        Chat = chat;
    }

    protected override void InsertItem(int index, ChatMessageViewModel item)
    {
        _chat?.SetMessageActions(item);
        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, ChatMessageViewModel item)
    {
        _chat?.SetMessageActions(item);
        base.SetItem(index, item);
    }
}
