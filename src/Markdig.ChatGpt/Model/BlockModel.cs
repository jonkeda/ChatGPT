using System.Text;

namespace Markdig.ChatGpt.Model
{
    public class BlockModel : IBlockModel, IAddChild
    {
        public InlineModelCollection Inlines { get; } = new();

        public void AddChild(INode child)
        {
            if (child is IInlineModel inline)
            {
                Inlines.Add(inline);
            }
        }


        public void ToText(StringBuilder sb)
        {
            foreach (var inline in Inlines)
            {
                inline.ToText(sb);
            }
        }
    }
}
