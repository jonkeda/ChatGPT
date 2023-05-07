// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license. 
// See the LICENSE.md file in the project root for more information.

using System;
using Markdig.ChatGpt.Model;
using Markdig.Extensions.TaskLists;
using Markdig.Wpf;

namespace Markdig.Renderers.Wpf.Extensions
{
    public class TaskListRenderer : ModelObjectRenderer<TaskList>
    {
        protected override void Write(ModelRenderer renderer, TaskList taskList)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (taskList == null) throw new ArgumentNullException(nameof(taskList));

            var checkBox = new CheckBoxModel
            {
                IsEnabled = false,
                IsChecked = taskList.Checked,
            };

            renderer.WriteInline(new InlineUIContainerModel(checkBox));
        }
    }
}
