using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AI;
using AI.Model.Json.Chat;
using AI.Model.Services;
using ChatGPT.Model.Services;
using ChatGPT.ViewModels.ChildChat;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatGPT.ViewModels.Chat;

public class ChatViewModel : ObservableObject
{
    private string? _name;
    private ChatSettingsViewModel? _settings;
    private ObservableCollection<ChatMessageViewModel> _messages;
    private ChatMessageViewModel? _currentMessage;
    private bool _isEnabled;
    private CancellationTokenSource? _cts;
    private ObservableCollection<ChatViewModel> _chats;
    private ChildChatViewModel _childChat;

    [JsonConstructor]
    public ChatViewModel()
    {
        _messages = new ObservableCollection<ChatMessageViewModel>();
        _chats = new ObservableCollection<ChatViewModel>();
        _childChat = new ChildChatViewModel(this);
        _isEnabled = true;
    }

    public ChatViewModel(ChatSettingsViewModel settings) : this()
    {
        _settings = settings;
    }

    public ChatViewModel(
        string directions = "You are a helpful assistant.",
        decimal temperature = 0.7m,
        decimal topP = 1m,
        decimal presencePenalty = 0m,
        decimal frequencyPenalty = 0m,
        int maxTokens = 2000,
        string? apiKey = null,
        string model = "gpt-3.5-turbo") : this()
    {
        _settings = new ChatSettingsViewModel
        {
            Temperature = temperature,
            TopP = topP,
            PresencePenalty = presencePenalty,
            FrequencyPenalty = frequencyPenalty,
            MaxTokens = maxTokens,
            ApiKey = apiKey,
            Model = model,
            Directions = directions
        };
    }

    [JsonPropertyName("name")]
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [JsonPropertyName("settings")]
    public ChatSettingsViewModel? Settings
    {
        get => _settings;
        set => SetProperty(ref _settings, value);
    }

    [JsonPropertyName("messages")]
    public ObservableCollection<ChatMessageViewModel> Messages
    {
        get => _messages;
        set => SetProperty(ref _messages, value);
    }

    [JsonPropertyName("chats")]
    public ObservableCollection<ChatViewModel> Chats
    {
        get => _chats;
        set => SetProperty(ref _chats, value);
    }

    [JsonPropertyName("currentMessage")]
    public ChatMessageViewModel? CurrentMessage
    {
        get => _currentMessage;
        set => SetProperty(ref _currentMessage, value);
    }

    [JsonIgnore]
    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }

    [JsonIgnore]
    public ChildChatViewModel ChildChat
    {
        get => _childChat;
        set => SetProperty(ref _childChat, value);
    }

    public void SetMessageActions(ChatMessageViewModel message)
    {
        message.SetSendAction(SendAsync);
        message.SetCopyAction(CopyAsync);
        message.SetRemoveAction(Remove);
        message.SetAddChatAction(AddChildChatAsync);
    }

    public async Task CopyAsync(ChatMessageViewModel message)
    {
        var app = Defaults.Locator.GetService<IApplicationService>();
        if (app is { })
        {
            if (message.Message is { } text)
            {
                await app.SetClipboardTextAsync(text);
            }
        }
    }

    public void Remove(ChatMessageViewModel message)
    {
        if (message.IsAwaiting)
        {
            try
            {
                _cts?.Cancel();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        if (message is { CanRemove: true })
        {
            Messages.Remove(message);

            var lastMessage = Messages.LastOrDefault();
            if (lastMessage is { })
            {
                lastMessage.IsSent = false;

                if (Messages.Count == 2)
                {
                    lastMessage.CanRemove = false;
                }
            }
        }
    }

    public async Task<bool> SendAsync(ChatMessageViewModel sendMessage, bool onlyAddMessage = false)
    {
        var isError = true;

        if (Settings is null)
        {
            return isError;
        }

        IsEnabled = false;

        try
        {
            sendMessage.CanRemove = true;
            sendMessage.IsSent = true;

            var isCanceled = false;
            ChatResultViewModel? result = null;
            if (!onlyAddMessage)
            {
                if (string.IsNullOrWhiteSpace(sendMessage.Message))
                {
                    Messages.Remove(sendMessage);
                }

                var chatPrompt = CreateChatMessages();

                _cts = new CancellationTokenSource();

                try
                {
                    result = await CreateResultMessageAsync(chatPrompt, _cts.Token);
                    isError = result == null || result.IsError;
                }
                catch (OperationCanceledException)
                {
                    isError = true;
                }

                isCanceled = _cts.IsCancellationRequested;

                _cts.Dispose();
                _cts = null;
            }

            if (!isCanceled)
            {
                var nextMessage = new ChatMessageViewModel
                {
                    Role = "user",
                    Message = "",
                    IsSent = false,
                    CanRemove = true,
                    Format = Settings.Format,
                    PromptTokens = result!.PromptTokens,
                    CompletionTokens = result!.CompletionTokens,
                    TotalTokens = result!.TotalTokens,
                };
                SetMessageActions(nextMessage);
                Messages.Add(nextMessage);
                CurrentMessage = nextMessage;

                isError = false;
            }
        }
        catch (Exception)
        {
            isError = true;
        }

        IsEnabled = true;

        return isError;
    }

    private async Task<ChatResultViewModel?> CreateResultMessageAsync(ChatMessage[] messages, CancellationToken token)
    {
        if (Settings is null)
        {
            return null;
        }

        // Sending...

        var resultMessage = new ChatMessageViewModel
        {
            Role = "assistant",
            Message = "Sending...",
            IsSent = false,
            CanRemove = true,
            IsAwaiting = true,
            Format = Defaults.TextMessageFormat
        };
        SetMessageActions(resultMessage);
        Messages.Add(resultMessage);
        CurrentMessage = resultMessage;

        // Response

        var result = await SendAsync(messages, token);

        // Update

        if (result is null)
        {
            resultMessage.Message = "Send result is empty.";
            resultMessage.IsError = true;
        }
        else
        {
            resultMessage.Message = result.Message;
            resultMessage.IsError = result.IsError;
            resultMessage.PromptTokens = result.PromptTokens;
            resultMessage.CompletionTokens = result.CompletionTokens;
            resultMessage.TotalTokens = result.TotalTokens;
        }

        resultMessage.Format = Settings.Format;
        resultMessage.IsAwaiting = false;
        resultMessage.IsSent = true;

        return result;
    }

    public ChatMessage[] CreateChatMessages()
    {
        var chatMessages = new List<ChatMessage>();

        // TODO: Ensure that chat prompt does not exceed maximum token limit.

        for (var i = 0; i < Messages.Count; i++)
        {
            var message = Messages[i];

            if (i == 0)
            {
                var content = Settings?.Directions ?? "";

                if (message.Message != Defaults.WelcomeMessage)
                {
                    content = message.Message;
                }

                chatMessages.Add(new ChatMessage
                {
                    Role = message.Role,
                    Content = content
                });

                continue;
            }

            if (!string.IsNullOrEmpty(message.Message))
            {
                chatMessages.Add(new ChatMessage
                {
                    Role = message.Role,
                    Content = message.Message
                });
            }
        }

        return chatMessages.ToArray();
    }

    private static async Task<ChatResponse?> GetResponseDataAsync(ChatServiceSettings chatServiceSettings, ChatSettingsViewModel chatSettings, CancellationToken token)
    {
        var chat = Defaults.Locator.GetService<IChatService>();
        if (chat is null)
        {
            return new ChatResponseError
            {
                Error = new ChatError
                {
                    Message = "Cant locate chat service."
                }
            };
        }

        // API Key

        var apiKey = Environment.GetEnvironmentVariable(Constants.EnvironmentVariableApiKey);
        var restoreApiKey = !string.IsNullOrWhiteSpace(chatSettings.ApiKey);

        if (string.IsNullOrWhiteSpace(chatSettings.ApiKey) && string.IsNullOrEmpty(apiKey))
        {
            return new ChatResponseError
            {
                Error = new ChatError { Message = "The OpenAI api key is not set." }
            };
        }

        // API Model

        var apiModel = Environment.GetEnvironmentVariable(Constants.EnvironmentVariableApiModel);
        var restoreApiModel = !string.IsNullOrWhiteSpace(chatSettings.Model);

        if (string.IsNullOrWhiteSpace(chatSettings.Model) && string.IsNullOrEmpty(apiModel))
        {
            return new ChatResponseError
            {
                Error = new ChatError { Message = "The OpenAI api model is not set." }
            };
        }

        // Settings

        if (restoreApiKey)
        {
            Environment.SetEnvironmentVariable(Constants.EnvironmentVariableApiKey, chatSettings.ApiKey);
        }

        if (restoreApiModel)
        {
            Environment.SetEnvironmentVariable(Constants.EnvironmentVariableApiModel, chatSettings.Model);
        }

        // Get

        ChatResponse? responseData;

        try
        {
            responseData = await chat.GetResponseDataAsync(chatServiceSettings, token);
        }
        catch (Exception e)
        {
            responseData = new ChatResponseError()
            {
                Error = new ChatError
                {
                    Message = $"{e}"
                }
            };
        }

        if (restoreApiKey && !string.IsNullOrWhiteSpace(apiKey))
        {
            Environment.SetEnvironmentVariable(Constants.EnvironmentVariableApiKey, apiKey);
        }

        if (restoreApiModel && !string.IsNullOrWhiteSpace(apiModel))
        {
            Environment.SetEnvironmentVariable(Constants.EnvironmentVariableApiModel, apiModel);
        }

        return responseData;
    }

    public async Task<ChatResultViewModel?> SendAsync(ChatMessage[] messages, CancellationToken token)
    {
        if (Settings is null)
        {
            return default;
        }

        var chatServiceSettings = new ChatServiceSettings
        {
            Model = Settings.Model,
            Messages = messages,
            Suffix = null,
            Temperature = Settings.Temperature,
            MaxTokens = Settings.MaxTokens,
            TopP = 1.0m,
            Stop = null,
            ApiUrl = Settings.ApiUrl,
        };

        var result = new ChatResultViewModel
        {
            Message = default,
            IsError = false
        };

        var responseData = await GetResponseDataAsync(chatServiceSettings, Settings, token);
        if (responseData is null)
        {
            result.Message = "Response data is empty.";
            result.IsError = true;
        }
        else if (responseData is ChatResponseError error)
        {
            var message = error.Error?.Message;
            result.Message = message ?? "Response error.";
            result.IsError = true;
        }
        else if (responseData is ChatResponseSuccess success)
        {
            var message = success.Choices?.FirstOrDefault()?.Message?.Content?.Trim();
            result.Message = message ?? "";
            result.IsError = false;
            if (success.Usage != null)
            {
                result.PromptTokens = success.Usage.PromptTokens;
                result.CompletionTokens = success.Usage.CompletionTokens;
                result.TotalTokens = success.Usage.TotalTokens;
            }

        }

        return result;
    }

    public ChatViewModel AddSystemMessage(string? message)
    {
        Messages.Add(new ChatMessageViewModel
        {
            Role = "system",
            Message = message
        });
        return this;
    }

    public ChatViewModel AddUserMessage(string? message)
    {
        Messages.Add(new ChatMessageViewModel
        {
            Role = "user",
            Message = message
        });
        return this;
    }

    public ChatViewModel AddAssistantMessage(string? message)
    {
        Messages.Add(new ChatMessageViewModel
        {
            Role = "assistant",
            Message = message
        });
        return this;
    }

    private ObservableCollection<ChatMessageViewModel> CopyMessages(out ChatMessageViewModel? currentMessage)
    {
        var messages = new ObservableCollection<ChatMessageViewModel>();

        currentMessage = null;

        foreach (var message in _messages)
        {
            var messageCopy = message.Copy();

            messages.Add(messageCopy);

            if (message == _currentMessage)
            {
                currentMessage = messageCopy;
            }
        }

        return messages;
    }

    public ChatViewModel Copy()
    {
        return new ChatViewModel
        {
            Name = _name,
            Settings = _settings?.Copy(),
            Messages = CopyMessages(out var currentMessage),
            CurrentMessage = currentMessage
        };
    }


    public async Task AddChildChatAsync(ChatMessageViewModel message)
    {
        _childChat.SetMessage(message);
    }

    public void CreateNewChildChat(ChildChatViewModel child)
    {
        if (child.ChatCreation == ChatCreation.Single)
        {
            if (string.IsNullOrEmpty(child.Name))
            {
                child.Name = "Chat";
            }

            AddNewChildChat(child, child.Prompt);
        }
        else if (child is { ChatCreation: ChatCreation.PerLine, Data: { } })
        {
            var sr = new StringReader(child.Data);
            while (sr.ReadLine() is { } line)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                child.Name = line;
                AddNewChildChat(child, $"{child.Prompt}\n {line}");
            }
        }

    }

    private void AddNewChildChat(ChildChatViewModel child, string? prompt)
    {
        var childChat = new ChatViewModel(Settings!)
        {
            Name = child.Name
        };
        childChat.AddMessage("system", child.SettingPrompt, true, false);
        childChat.AddMessage("user", prompt, false, false);
        Chats.Add(childChat);
    }

    public void AddMessage(string role, string? message, bool isSent, bool canRemove)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }
        var msg = new ChatMessageViewModel 
        {
            Format = Defaults.TextMessageFormat,
            Role = role, 
            Message = message,
            IsSent = isSent,
            CanRemove = canRemove
        };
        SetMessageActions(msg);
        Messages.Add(msg);
    }
}
