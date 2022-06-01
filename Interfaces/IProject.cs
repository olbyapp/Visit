
namespace Visit.Interfaces
{
    public interface IProject
    {
        int Id { get; set; }
        string Name { get; set; }
        string DescriptionFileName { get; set; }
        string GitHubLink { get; set; }
        List<string> Points { get; set; }
        string Stack { get; set; }
    }
}