// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license.
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Syntax;
using Markdig.Wpf;

namespace Markdig.Renderers.Wpf
{
    public class ParagraphRenderer : ModelObjectRenderer<ParagraphBlock>
    {
        /// <inheritdoc/>
        protected override void Write(ModelRenderer renderer, ParagraphBlock obj)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var paragraph = new ParagraphModel();

            renderer.Push(paragraph);
            renderer.WriteLeafInline(obj);
            renderer.Pop();
        }
    }
}
