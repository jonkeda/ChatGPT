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
    /// A WPF renderer for a <see cref="AutolinkInline"/>.
    /// </summary>
    /// <seealso cref="AutolinkInline" />
    public class AutolinkInlineRenderer : ModelObjectRenderer<AutolinkInline>
    {
        /// <inheritdoc/>
        protected override void Write(ModelRenderer renderer, AutolinkInline link)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (link == null) throw new ArgumentNullException(nameof(link));

            var url = link.Url;
            if (link.IsEmail)
            {
                url = "mailto:" + url;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                url = "#";
            }

            var hyperlink = new HyperlinkModel
            {
                //Command = Commands.Hyperlink,
                CommandParameter = url,
                NavigateUri = new Uri(url, UriKind.RelativeOrAbsolute),
                ToolTip = link.Url,
            };

            renderer.Push(hyperlink);
            renderer.WriteText(link.Url);
            renderer.Pop();
        }
    }
}
