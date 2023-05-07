using System.Collections.ObjectModel;

namespace ChatGPT.Model.Chapters
{
    public class ChapterCollection : Collection<Chapter>
    {

    } 

    public class Chapter
    {
        public ChapterCollection Chapters { get; } = new ChapterCollection();
        public string? Name { get; set; }
    }
}
