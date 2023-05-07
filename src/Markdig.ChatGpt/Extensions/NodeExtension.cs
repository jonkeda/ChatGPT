using System.Text;
using Markdig.ChatGpt.Model;

namespace Markdig.ChatGpt.Extensions
{
    public static class NodeExtension
    {
        public static string ToText(this INode node)
        {
            var sb = new StringBuilder();
            node.ToText(sb);
            return sb.ToString();
        }
    }
}
