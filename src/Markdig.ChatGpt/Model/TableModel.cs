using System;
using System.Text;

namespace Markdig.ChatGpt.Model
{
    public class TableModel : IBlockModel, IAddChild
    {
        public TableColumnModelCollection Columns { get; } = new();
        public TableRowModelCollection Rows { get; } = new();
        public TableRowGroupModelCollection Groups { get; } = new();

        public void AddChild(INode child)
        {
            if (child is TableRowModel row)
            {
                Rows.Add(row);
            }
            else if (child is TableColumnModel column)
            {
                Columns.Add(column);
            }
            else if (child is TableRowGroupModel group)
            {
                Groups.Add(group);
            }

        }

        public void ToText(StringBuilder sb)
        {
            foreach (var group in Groups)
            {
                group.ToText(sb);
            }
            foreach (var row in Rows)
            {
                row.ToText(sb);
            }
        }
    }
}
