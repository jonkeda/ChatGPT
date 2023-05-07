using System;
using System.Text;

namespace Markdig.ChatGpt.Model
{
    public class TableRowModel : IAddChild
    {
        public TableCellModelCollection Cells { get; } = new();

        public void AddChild(INode child)
        {
            if (child is TableCellModel cell)
            {
                Cells.Add(cell);
            }
        }

        public bool IsHeader { get; set; }
        public void ToText(StringBuilder sb)
        {
            foreach (var cell in Cells)
            {
                cell.ToText(sb);
            }
        }
    }
}
