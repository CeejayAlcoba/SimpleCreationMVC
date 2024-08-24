using SimpleCreation.Models;
using System.IO;
using System.IO.Compression;

namespace SimpleCreation.Services
{
    public class FileService
    {
        public void Create(string folderName,string fileName,string content )
        {
            string directoryPath = Path.Combine("Project","Project", folderName);
            Directory.CreateDirectory(directoryPath);

            string folderPath = Path.Combine(directoryPath, fileName);

            File.WriteAllText(folderPath, content);


        }
        public void Delete(bool isProject = true, bool isProjectZip = true)
        {
            string project = Path.Combine("Project");
            string projectZip = Path.Combine("ProjectZip");
            if(Directory.Exists(project) && isProject)
            {
                Directory.Delete(project, true);
            }
            if(Directory.Exists(projectZip) && isProjectZip)
            {
                Directory.Delete(projectZip, true);
            }
        }
        public string ZipProjectFile()
        {
            string projectPath = ProjectFolderPath();
            string directoryPath = Path.Combine("ProjectZip");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);


                string zipPath = Path.Combine(directoryPath, "Project.zip");
                File.Delete(zipPath);
                ZipFile.CreateFromDirectory(projectPath, zipPath);
                return zipPath;
            }
            return Path.Combine(directoryPath, "Project.zip");
         
        }
        public string ZipPublishedFile()
        {
            string publishedPath = PublishedFolderPath();
            string directoryPath = Path.Combine("Published");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);

                string zipPath = Path.Combine(directoryPath, "Published.zip");
               
                ZipFile.CreateFromDirectory(publishedPath, zipPath);
                return zipPath;
            }
            return Path.Combine(directoryPath, "Published.zip");

        }
        public string PublishedFolderPath()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            return Path.Combine(directoryPath, "Published");
        }
        public string ProjectFolderPath()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            return  Path.Combine(directoryPath,"Project");
        }

       
    }
}
