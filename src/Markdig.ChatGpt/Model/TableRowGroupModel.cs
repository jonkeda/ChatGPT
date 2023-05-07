using System.Collections.ObjectModel;
using System.Text;

namespace Markdig.ChatGpt.Model
{
    public class TableRowGroupModelCollection : Collection<TableRowGroupModel>
    { }

    public class TableRowGroupModel : INode
    {
        public TableRowModelCollection Rows { get; } = new();

        public void AddChild(IAddChild child)
        {
            if (child is TableRowModel row)
            {
                Rows.Add(row);
            }
        }

        public void ToText(StringBuilder sb)
        {
            foreach (var row in Rows)
            {
                row.ToText(sb);
            }
        }
    }
}
