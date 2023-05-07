// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license. 
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Extensions.Tables;
using Markdig.Wpf;

namespace Markdig.Renderers.Wpf.Extensions
{
    public class TableRenderer : ModelObjectRenderer<Table>
    {
        protected override void Write(ModelRenderer renderer, Table table)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (table == null) throw new ArgumentNullException(nameof(table));

            var wpfTable = new TableModel();

            //wpfTable.SetResourceReference(FrameworkContentElement.StyleProperty, Styles.TableStyleKey);

            foreach (var tableColumnDefinition in table.ColumnDefinitions)
            {
                wpfTable.Columns.Add(new TableColumnModel
                {
/*                    Width = (tableColumnDefinition?.Width ?? 0) != 0 ?
                        new GridLength(tableColumnDefinition!.Width, GridUnitType.Star) :
                        GridLength.Auto,
*/                });
            }

            var wpfRowGroup = new TableRowGroupModel();

            renderer.Push(wpfTable);
            renderer.Push(wpfRowGroup);

            foreach (var rowObj in table)
            {
                var row = (TableRow)rowObj;
                var wpfRow = new TableRowModel();

                renderer.Push(wpfRow);

                if (row.IsHeader)
                {
                    wpfRow.IsHeader = true;
                }

                for (var i = 0; i < row.Count; i++)
                {
                    var cellObj = row[i];
                    var cell = (TableCell)cellObj;
                    var wpfCell = new TableCellModel
                    {
                        ColumnSpan = cell.ColumnSpan,
                        RowSpan = cell.RowSpan,
                    };

                    renderer.Push(wpfCell);
                    renderer.Write(cell);
                    renderer.Pop();

                    if (table.ColumnDefinitions.Count > 0)
                    {
                        var columnIndex = cell.ColumnIndex < 0 || cell.ColumnIndex >= table.ColumnDefinitions.Count
                            ? i
                            : cell.ColumnIndex;
                        columnIndex = columnIndex >= table.ColumnDefinitions.Count ? table.ColumnDefinitions.Count - 1 : columnIndex;
                        var alignment = table.ColumnDefinitions[columnIndex].Alignment;
                        if (alignment.HasValue)
                        {
                            switch (alignment)
                            {
                                case TableColumnAlign.Center:
                                    wpfCell.TextAlignment = TextAlignmentModel.Center;
                                    break;
                                case TableColumnAlign.Right:
                                    wpfCell.TextAlignment = TextAlignmentModel.Right;
                                    break;
                                case TableColumnAlign.Left:
                                    wpfCell.TextAlignment = TextAlignmentModel.Left;
                                    break;
                            }
                        }
                    }
                }

                renderer.Pop();
            }

            renderer.Pop();
            renderer.Pop();
        }
    }
}
