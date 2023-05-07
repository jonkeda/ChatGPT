namespace Markdig.ChatGpt.Model
{
    public class TableCellModel : BlockModel
    {
        public int ColumnSpan { get; set; }
        public int RowSpan { get; set; }
        public TextAlignmentModel TextAlignment { get; set; }
    }
}