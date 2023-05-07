// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license.
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Wpf;
using QuoteBlock = Markdig.Syntax.QuoteBlock;

namespace Markdig.Renderers.Wpf
{
    public class QuoteBlockRenderer : ModelObjectRenderer<QuoteBlock>
    {
        /// <inheritdoc/>
        protected override void Write(ModelRenderer renderer, QuoteBlock obj)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var section = new QuoteBlockModel();

            renderer.Push(section);
            renderer.WriteChildren(obj);
            renderer.Pop();
        }
    }
}
