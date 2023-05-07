using System.Text;

namespace Markdig.ChatGpt.Model
{
    public class InlineUIContainerModel : IInlineModel
    {
        public ControlModel Control { get; }

        public InlineUIContainerModel(ControlModel control)
        {
            Control = control;
        }

        public void ToText(StringBuilder sb)
        {
            Control.ToText(sb);
        }
    }
}
