using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using ChatGPT.ViewModels.Chat;

namespace ChatGPT.Converters;

public class RoleIconConverter : IValueConverter
{
    public static RoleIconConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string layout)
        {
            var icon = layout switch
            {
                Roles.System => "SystemIcon",
                Roles.User => "PersonIcon",
                Roles.Assistant => "ChatIcon",
                _ => default
            };

            if (icon is null)
            {
                return AvaloniaProperty.UnsetValue;
            }

            if (Application.Current?.Resources.TryGetResource(icon, null, out var resource) == true)
            {
                return resource;
            }
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
