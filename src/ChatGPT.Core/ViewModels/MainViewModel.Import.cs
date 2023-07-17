using System.Collections.Generic;
using System.Linq;
using AI.Model.Json.ChatGPT;
using ChatGPT.ViewModels.Chat;

namespace ChatGPT.ViewModels;

public partial class MainViewModel
{
    private void ImportChats(ChatGpt[] gptChats)
    {
        var chats = new List<ChatViewModel>();

        foreach (var gptChat in gptChats.Reverse())
        {
            var chat = new ChatViewModel()
            {
                Name = gptChat.Title,
                Settings = CreateDefaultChatSettings()
            };

            if (gptChat.Messages is { })
            {
                foreach (var message in gptChat.Messages)
                {
                    var role = message.Role;
                    var content = message.Content?.LastOrDefault();

                    if (role == Roles.System && string.IsNullOrEmpty(content))
                    {
                        content = "You are a helpful assistant.";
                    }

                    var item = new ChatMessageViewModel
                    {
                        Role = role,
                        Message = content,
                        Format = role == Roles.Assistant
                            ? Defaults.MarkdownMessageFormat
                            : Defaults.TextMessageFormat,
                        IsSent = true,
                        CanRemove = true
                    };
                    chat.Messages.Add(item);
                }
            }

            var prompt = new ChatMessageViewModel
            {
                Role = Roles.User,
                Message = "",
                Format = Defaults.TextMessageFormat,
                IsSent = false,
                CanRemove = true
            };
            chat.Messages.Add(prompt);

            chat.CurrentMessage = chat.Messages.LastOrDefault();
            chats.Add(chat);
        }

        // Chats.Clear();

        foreach (var chat in chats)
        {
            Chats.Add(chat);
        }

        CurrentChat = chats.LastOrDefault();
    }
}
