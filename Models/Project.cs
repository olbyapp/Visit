using Visit.Interfaces;

namespace Visit.Models
{
    public class Project : IProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DescriptionFileName { get; set; }
        public string Stack { get; set; }
        public List<string> Points { get; set; }
        public string GitHubLink { get; set; }

    }
}
