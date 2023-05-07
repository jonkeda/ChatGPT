// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license. 
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Syntax;
using Markdig.Wpf;

namespace Markdig.Renderers.Wpf
{
    public class ThematicBreakRenderer : ModelObjectRenderer<ThematicBreakBlock>
    {
        protected override void Write(ModelRenderer renderer, ThematicBreakBlock obj)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var line = new LineModel { X2 = 1 };

            var paragraph = new ParagraphModel();

            paragraph.Inlines.Add(new InlineUIContainerModel(line));

            renderer.WriteBlock(paragraph);
        }
    }
}
