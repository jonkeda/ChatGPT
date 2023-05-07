namespace Markdig.ChatGpt.Model
{
    public interface IAddChild : INode
    {
        void AddChild(INode popped);
    }
}