using System.Text;

namespace Markdig.ChatGpt.Model
{
    public class ListModel : IBlockModel, IAddChild
    {
        public ListItemModelCollection Items { get; } = new ListItemModelCollection();

        public TextMarkerStyleModel MarkerStyle { get; set; }
        public int StartIndex { get; set; }

        public void AddChild(INode child)
        {
            if (child is ListItemModel item)
            {
                Items.Add(item);
            }
        }

        public void ToText(StringBuilder sb)
        {
            foreach (var item in Items)
            {
                item.ToText(sb);
            }
        }
    }
}
