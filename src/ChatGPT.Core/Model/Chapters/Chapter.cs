namespace ChatGPT.Model.Chapters
{
    public class Chapter
    {
        public ChapterCollection Chapters { get; } = new ChapterCollection();
        public string? Name { get; set; }
    }
}
