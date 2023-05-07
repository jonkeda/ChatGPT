// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license.
// See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Markdig.ChatGpt.Model;
using Markdig.Helpers;
using Markdig.Renderers.Wpf;
using Markdig.Renderers.Wpf.Extensions;
using Markdig.Renderers.Wpf.Inlines;
using Markdig.Syntax;
using Markdig.Wpf;

namespace Markdig.Renderers
{
    /// <summary>
    /// WPF renderer for a Markdown <see cref="MarkdownDocument"/> object.
    /// </summary>
    /// <seealso cref="RendererBase" />
    public class ModelRenderer : RendererBase
    {
        private readonly Stack<INode> stack = new Stack<INode>();
        private char[] buffer;

        public ModelRenderer()
        {
            buffer = new char[1024];
        }

        public ModelRenderer(DocumentModel document)
        {
            buffer = new char[1024];
            LoadDocument(document);
        }

        public virtual void LoadDocument(DocumentModel document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            stack.Push(document);
            LoadRenderers();
        }

        public DocumentModel? Document { get; protected set; }

        /// <inheritdoc/>
        public override object? Render(MarkdownObject markdownObject)
        {
            Write(markdownObject);
            return Document;
        }

        /// <summary>
        /// Writes the inlines of a leaf inline.
        /// </summary>
        /// <param name="leafBlock">The leaf block.</param>
        /// <returns>This instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteLeafInline(LeafBlock leafBlock)
        {
            if (leafBlock == null) throw new ArgumentNullException(nameof(leafBlock));
            var inline = (Syntax.Inlines.Inline)leafBlock.Inline;
            while (inline != null)
            {
                Write(inline);
                inline = inline.NextSibling;
            }
        }

        /// <summary>
        /// Writes the lines of a <see cref="LeafBlock"/>
        /// </summary>
        /// <param name="leafBlock">The leaf block.</param>
        public void WriteLeafRawLines(LeafBlock leafBlock)
        {
            if (leafBlock == null) throw new ArgumentNullException(nameof(leafBlock));
            if (leafBlock.Lines.Lines != null)
            {
                var lines = leafBlock.Lines;
                var slices = lines.Lines;
                for (var i = 0; i < lines.Count; i++)
                {
                    if (i != 0)
                        WriteInline(new LineBreakModel());

                    WriteText(ref slices[i].Slice);
                }
            }
        }

        public void Push(INode o)
        {
            stack.Push(o);
        }

        public void Pop()
        {
            var popped = stack.Pop();
            if (stack.Peek() is IAddChild add)
            {
                add.AddChild(popped);
            }
        }

        public void WriteBlock(IBlockModel block)
        {
            if (stack.Peek() is IAddChild add)
            {
                add.AddChild(block);
            }
        }

        public void WriteInline(IInlineModel inline)
        {
            if (stack.Peek() is IAddChild add)
            {
                AddInline(add, inline);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteText(ref StringSlice slice)
        {
            if (slice.Start > slice.End)
                return;

            WriteText(slice.Text, slice.Start, slice.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteText(string? text)
        {
            WriteInline(new RunModel(text));
        }

        public void WriteText(string? text, int offset, int length)
        {
            if (text == null)
                return;

            if (offset == 0 && text.Length == length)
            {
                WriteText(text);
            }
            else
            {
                if (length > buffer.Length)
                {
                    buffer = text.ToCharArray();
                    WriteText(new string(buffer, offset, length));
                }
                else
                {
                    text.CopyTo(offset, buffer, 0, length);
                    WriteText(new string(buffer, 0, length));
                }
            }
        }

        /// <summary>
        /// Loads the renderer used for render WPF
        /// </summary>
        protected virtual void LoadRenderers()
        {
            // Default block renderers
            ObjectRenderers.Add(new CodeBlockRenderer());
            ObjectRenderers.Add(new ListRenderer());
            ObjectRenderers.Add(new HeadingRenderer());
            ObjectRenderers.Add(new ParagraphRenderer());
            ObjectRenderers.Add(new QuoteBlockRenderer());
            ObjectRenderers.Add(new ThematicBreakRenderer());

            // Default inline renderers
            ObjectRenderers.Add(new AutolinkInlineRenderer());
            ObjectRenderers.Add(new CodeInlineRenderer());
            ObjectRenderers.Add(new DelimiterInlineRenderer());
            ObjectRenderers.Add(new EmphasisInlineRenderer());
            ObjectRenderers.Add(new HtmlEntityInlineRenderer());
            ObjectRenderers.Add(new LineBreakInlineRenderer());
            ObjectRenderers.Add(new LinkInlineRenderer());
            ObjectRenderers.Add(new LiteralInlineRenderer());

            // Extension renderers
            ObjectRenderers.Add(new TableRenderer());
            ObjectRenderers.Add(new TaskListRenderer());
        }

        private static void AddInline(IAddChild parent, IInlineModel inline)
        {
            parent.AddChild(inline);
        }
    }
}
