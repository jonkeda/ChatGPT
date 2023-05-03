using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatGPT.ViewModels.Layouts;

public partial class DesktopLayoutViewModel : LayoutViewModel
{
    private string _settingsWidth;
    private string _chatsWidth;
    private string _promptsWidth;
    private string _searchWidth;

    [JsonConstructor]
    public DesktopLayoutViewModel()
    {
        Name = "Desktop";

        ShowSearch = false;
        ShowChats = true;
        ShowSettings = true;
        ShowPrompts = true;

        _settingsWidth = "290";
        _searchWidth = "290";
        _chatsWidth = "290";
        _promptsWidth = "290";
    }

    [JsonPropertyName("settingsWidth")]
    public string SettingsWidth
    {
        get => _settingsWidth;
        set => SetProperty(ref _settingsWidth, value);
    }

    [JsonPropertyName("chatsWidth")]
    public string ChatsWidth
    {
        get => _chatsWidth;
        set => SetProperty(ref _chatsWidth, value);
    }

    [JsonPropertyName("searchWidth")]
    public string SearchWidth
    {
        get => _searchWidth;
        set => SetProperty(ref _searchWidth, value);
    }

    [JsonPropertyName("promptsWidth")]
    public string PromptsWidth
    {
        get => _promptsWidth;
        set => SetProperty(ref _promptsWidth, value);
    }

    public override async Task BackAsync()
    {
        // TODO:
        await Task.Yield();
    }

    protected override void ShowSettingsAction()
    {
        ShowSettings = !ShowSettings;
    }

    protected override void ShowChatsAction()
    {
        ShowChats = !ShowChats;
    }

    protected override void ShowSearchAction()
    {
        ShowSearch = !ShowSearch;
    }

    protected override void ShowPromptsAction()
    {
        ShowPrompts = !ShowPrompts;
    }

    public override LayoutViewModel Copy()
    {
        return new DesktopLayoutViewModel()
        {
            ShowSearch = ShowSearch,
            ShowChats = ShowChats,
            ShowSettings = ShowSettings,
            ShowPrompts = ShowPrompts,
            SettingsWidth = _settingsWidth,
            ChatsWidth = _chatsWidth,
            PromptsWidth = _promptsWidth
        };
    }
}
