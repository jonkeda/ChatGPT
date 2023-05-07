// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license. 
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Syntax;
using Markdig.Wpf;

namespace Markdig.Renderers.Wpf
{
    public class HeadingRenderer : ModelObjectRenderer<HeadingBlock>
    {
        protected override void Write(ModelRenderer renderer, HeadingBlock obj)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var paragraph = new HeaderModel();
            paragraph.Level = obj.Level;

            renderer.Push(paragraph);
            renderer.WriteLeafInline(obj);
            renderer.Pop();
        }
    }
}
