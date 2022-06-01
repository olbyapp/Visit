using System.Reflection;
using System.Text;
using System.Text.Json;
using Visit.Interfaces;
using Visit.Models;
using ILogger = Visit.Interfaces.ILogger;

namespace Visit.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ILogger _logger;
        private readonly string _projectsConfigsPath = Path.Combine(Directory.GetCurrentDirectory(), "ProjectConfigs");
        private readonly string _projectsMDPath = Path.Combine(Directory.GetCurrentDirectory(), "MdDescriptions");
        private readonly List<IProject> _projects = new();

        public ProjectService(ILogger logger)
        {
            _logger = logger;
            LoadProjects();
        }

        private void LoadProjects()
        {
 
            var projectFiles = Directory.GetFiles(_projectsConfigsPath);

            foreach (var projectFile in projectFiles)
            {
                _logger.Log("InitialisationModules", "Init " + projectFile);

                string pathToConfig = Path.Combine(_projectsConfigsPath, projectFile);
                if (File.Exists(pathToConfig))
                {
                    using (FileStream fs = new FileStream(pathToConfig, FileMode.Open))
                    {
   
                        var project = JsonSerializer.Deserialize(fs, typeof(Project));
                        if (project != default)
                        {
                            _projects.Add((IProject)project);
                        }
                           
                    }
                }
            }
        }

        public List<IProject> GetProjects()
        {
            return _projects;
        }

        public string ReadMdFile(IProject project)
        {
            string pathToMd = Path.Combine(_projectsMDPath, project.DescriptionFileName);
            if (File.Exists(pathToMd))
            {
                using FileStream fs = new FileStream(pathToMd, FileMode.Open, FileAccess.Read);
                using var sr = new StreamReader(fs, Encoding.UTF8);
                return sr.ReadToEnd();
            }
            else
                return "Описание проекта не найдено";
        }
    }
}
