// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license.
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Syntax.Inlines;
using Markdig.Wpf;

namespace Markdig.Renderers.Wpf.Inlines
{
    /// <summary>
    /// A WPF renderer for an <see cref="EmphasisInline"/>.
    /// </summary>
    /// <seealso cref="EmphasisInline" />
    public class EmphasisInlineRenderer : ModelObjectRenderer<EmphasisInline>
    {
        protected override void Write(ModelRenderer renderer, EmphasisInline obj)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            SpanModel? span = null;

            switch (obj.DelimiterChar)
            {
                case '*':
                case '_':
                    // span = obj.DelimiterCount == 2 ? (Span)new Bold() : new Italic();
                    span = new SpanModel();
                    break;
                case '~':
                    span = new SpanModel();
                    // span.SetResourceReference(FrameworkContentElement.StyleProperty, obj.DelimiterCount == 2 ? Styles.StrikeThroughStyleKey : Styles.SubscriptStyleKey);
                    break;
                case '^':
                    span = new SpanModel();
                    // span.SetResourceReference(FrameworkContentElement.StyleProperty, Styles.SuperscriptStyleKey);
                    break;
                case '+':
                    span = new SpanModel();
                    // span.SetResourceReference(FrameworkContentElement.StyleProperty, Styles.InsertedStyleKey);
                    break;
                case '=':
                    span = new SpanModel();
                    // span.SetResourceReference(FrameworkContentElement.StyleProperty, Styles.MarkedStyleKey);
                    break;
            }

            if (span != null)
            {
                renderer.Push(span);
                renderer.WriteChildren(obj);
                renderer.Pop();
            }
            else
            {
                renderer.WriteChildren(obj);
            }
        }
    }
}
