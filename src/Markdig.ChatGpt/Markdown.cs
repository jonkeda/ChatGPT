// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license.
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Renderers;

namespace Markdig.Wpf
{
    /// <summary>
    /// Provides methods for parsing a Markdown string to a syntax tree and converting it to other formats.
    /// </summary>
    public static partial class MarkdownModel
    {
        /// <summary>
        /// Converts a Markdown string to a FlowDocument.
        /// </summary>
        /// <param name="markdown">A Markdown text.</param>
        /// <param name="pipeline">The pipeline used for the conversion.</param>
        /// <returns>The result of the conversion</returns>
        /// <exception cref="System.ArgumentNullException">if markdown variable is null</exception>
        public static DocumentModel ToDocumentModel(string markdown, MarkdownPipeline? pipeline = null)
        {
            if (markdown == null)
                throw new ArgumentNullException(nameof(markdown));
            pipeline = pipeline ?? new MarkdownPipelineBuilder().Build();

            // We override the renderer with our own writer
            var result = new DocumentModel();

            var renderer = new ModelRenderer(result);

            pipeline.Setup(renderer);

            var document = Markdig.Markdown.Parse(markdown, pipeline);
            renderer.Render(document);

            return result;
        }
    }
}
