namespace Lab4.Infrastructure
{
    public class Configs 
    {
        public string OutputPath { get; }

        public Configs() 
        {
            OutputPath = GetProjectRootPath("output.txt");
        }

        private static string GetProjectRootPath(string fileName)
        {
            string binPath = AppContext.BaseDirectory;
            string projectRoot = binPath[..binPath.IndexOf("bin")];
            return Path.Combine(projectRoot, fileName);
        }
    }
}