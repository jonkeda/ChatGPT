// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license.
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Wpf.Inlines
{
    /// <summary>
    /// A WPF renderer for a <see cref="LineBreakInline"/>.
    /// </summary>
    /// <seealso cref="LineBreakInline" />
    public class LineBreakInlineRenderer : ModelObjectRenderer<LineBreakInline>
    {
        /// <inheritdoc/>
        protected override void Write(ModelRenderer renderer, LineBreakInline obj)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (obj.IsHard)
            {
                renderer.WriteInline(new LineBreakModel());
            }
            else
            {
                // Soft line break.
                renderer.WriteText(" ");
            }
        }
    }
}

