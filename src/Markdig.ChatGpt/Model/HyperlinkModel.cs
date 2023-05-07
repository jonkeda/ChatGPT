using System;

namespace Markdig.ChatGpt.Model
{
    public class HyperlinkModel : ControlModel
    {
        //public RoutedCommand Command { get; set; }
        public string CommandParameter { get; set; }
        public Uri NavigateUri { get; set; }
        public string ToolTip { get; set; }
    }
}