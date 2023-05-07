// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license. 
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Syntax;

namespace Markdig.Renderers.Wpf
{
    public class CodeBlockRenderer : ModelObjectRenderer<CodeBlock>
    {
        protected override void Write(ModelRenderer renderer, CodeBlock obj)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));

            var paragraph = new CodeBlockModel();
            renderer.Push(paragraph);
            renderer.WriteLeafRawLines(obj);
            renderer.Pop();
        }
    }
}
