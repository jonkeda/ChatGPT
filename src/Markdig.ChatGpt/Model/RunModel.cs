using System.Text;

namespace Markdig.ChatGpt.Model
{
    public class RunModel : IInlineModel
    {
        public string? Text { get; }

        public RunModel(string? text)
        {
            Text = text;
        }

        public void ToText(StringBuilder sb)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }
            sb.Append(Text);
        }
    }
}
