using System;
using System.Text.Json.Serialization;
using ChatGPT.ViewModels.Chat;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatGPT.ViewModels.ChildChat
{
    public class ChildChatViewModel : ObservableObject
    {
        private bool _isEnabled = false;
        private string? _prompt;

        public ChildChatViewModel(ChatViewModel chatViewModel)
        {
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
        public String? Prompt
        {
            get => _prompt;
            set => SetProperty(ref _prompt, value);
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
        }

    }
}
