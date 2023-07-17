using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using ChatGPT.ViewModels.Chat;

namespace ChatGPT.Converters;

public class RoleBackgroundColorConverter : IValueConverter
{
    public static RoleBackgroundColorConverter Instance = new();

    public static IImmutableSolidColorBrush user = new ImmutableSolidColorBrush(new Color(255, 240, 240, 240));

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string layout)
        {
            var icon = layout switch
            {
                Roles.System => Brushes.LightGray,
                Roles.User => Brushes.White,
                Roles.Assistant => user,
                _ => Brushes.Transparent
            };
            return icon;
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
