using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ChatGPT.ViewModels.Chat;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatGPT.ViewModels.ChildChat;

public class ChildChatViewModel : ObservableObject
{
    private readonly ChatViewModel _chatViewModel;
    private bool _isEnabled;
    private string? _settingPrompt;
    private string? _name;
    private string? _prompt;
    private string? _data;
    private ChatCreation? _chatCreation = ChildChat.ChatCreation.PerLine;

    public ChildChatViewModel(ChatViewModel chatViewModel)
    {
        _chatViewModel = chatViewModel;
        OkCommand = new RelayCommand(OkAction);
        CancelCommand = new RelayCommand(CancelAction);
    }

    [JsonIgnore]
    public IRelayCommand OkCommand { get; }

    [JsonIgnore]
    public IRelayCommand CancelCommand { get; }


    [JsonIgnore]
    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }

    [JsonIgnore]
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [JsonIgnore]
    public string? SettingPrompt
    {
        get => _settingPrompt;
        set => SetProperty(ref _settingPrompt, value);
    }

    [JsonIgnore]
    public string? Prompt
    {
        get => _prompt;
        set => SetProperty(ref _prompt, value);
    }

    [JsonIgnore]
    public ChatCreation[] ChatCreations
    {
        get
        {
            return (ChatCreation[])Enum.GetValues(typeof(ChatCreation));
        }
    }

    [JsonIgnore]
    public ChatCreation? ChatCreation
    {
        get => _chatCreation;
        set => SetProperty(ref _chatCreation, value);
    }

    [JsonIgnore]
    public string? Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }

    public void Enable()
    {
        IsEnabled = true;
    }

    private void CancelAction()
    {
        IsEnabled = false;
    }

    private void OkAction()
    {
        IsEnabled = false;
        _chatViewModel.CreateNewChildChat(this);
    }

    public void SetMessage(ChatMessageViewModel message)
    {
        SettingPrompt = _chatViewModel.Settings?.Directions;
        Data = message.Message;

        Enable();
    }
}

public enum ChatCreation
{
    Single,
    PerLine,
    PerChapter
}
