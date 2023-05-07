// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license. 
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Syntax.Inlines;
using Markdig.Wpf;

namespace Markdig.Renderers.Wpf.Inlines
{
    public class CodeInlineRenderer : ModelObjectRenderer<CodeInline>
    {
        protected override void Write(ModelRenderer renderer, CodeInline obj)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var run = new CodeInlineModel(obj.Content);
            renderer.WriteInline(run);
        }
    }
}
