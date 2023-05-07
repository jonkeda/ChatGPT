using System;
using System.Text;

namespace Markdig.ChatGpt.Model
{
    public class InlineModel : IInlineModel, IAddChild
    {
        public BlockModelCollection Blocks { get; } = new();

        public void AddChild(INode child)
        {
            if (child is IBlockModel block)
            {
                Blocks.Add(block);
            }
        }

        public void ToText(StringBuilder sb)
        {
            foreach (var block in Blocks)
            {
                block.ToText(sb);
            }
        }
    }
}
