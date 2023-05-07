using System.Text;

namespace Markdig.ChatGpt.Model
{
    public interface INode
    {
        public void ToText(StringBuilder sb);
    }
}
