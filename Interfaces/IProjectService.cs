using Visit.Interfaces;

namespace Visit.Interfaces
{
    public interface IProjectService
    {
        List<IProject> GetProjects();
        string ReadMdFile(IProject project);
    }
}