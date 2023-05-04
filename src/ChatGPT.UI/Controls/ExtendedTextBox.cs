using Avalonia.Controls;

using Avalonia;

namespace ChatGPT.Controls
{
    public class ExtendedTextBox : TextBox
    {
        public static readonly StyledProperty<bool> AutoFocusProperty =
            AvaloniaProperty.Register<ExtendedTextBox, bool>(nameof(AutoFocus), defaultValue: false);

        public bool AutoFocus
        {
            get => GetValue(AutoFocusProperty);
            set => SetValue(AutoFocusProperty, value);
        }

        public ExtendedTextBox()
        {
            AutoFocusProperty.Changed.AddClassHandler<ExtendedTextBox>((sender, e) => sender.OnAutoFocusChanged(e));
        }

        private void OnAutoFocusChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue!)
            {
                Focus();
            }
        }
    }
}
