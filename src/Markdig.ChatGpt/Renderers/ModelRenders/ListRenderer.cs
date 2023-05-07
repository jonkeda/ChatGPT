// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license. 
// See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;
using Markdig.ChatGpt.Model;
using Markdig.Syntax;

namespace Markdig.Renderers.Wpf
{
    public class ListRenderer : ModelObjectRenderer<ListBlock>
    {
        protected override void Write(ModelRenderer renderer, ListBlock listBlock)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (listBlock == null) throw new ArgumentNullException(nameof(listBlock));

            var list = new ListModel();

            if (listBlock.IsOrdered)
            {
                list.MarkerStyle = TextMarkerStyleModel.Decimal;

                if (listBlock.OrderedStart != null && (listBlock.DefaultOrderedStart != listBlock.OrderedStart))
                {
                    list.StartIndex = int.Parse(listBlock.OrderedStart, NumberFormatInfo.InvariantInfo);
                }
            }
            else
            {
                list.MarkerStyle = TextMarkerStyleModel.Disc;
            }

            renderer.Push(list);

            foreach (var item in listBlock)
            {
                var listItemBlock = (ListItemBlock)item;
                var listItem = new ListItemModel();
                renderer.Push(listItem);
                renderer.WriteChildren(listItemBlock);
                renderer.Pop();
            }

            renderer.Pop();
        }
    }
}
