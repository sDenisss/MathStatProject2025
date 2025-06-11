namespace Project
{
    public class Configs 
    {
        public string PRPath { get; }
        public string ExcelGraphs { get; }

        public Configs()
        {
            PRPath = GetProjectRootPath("ParsingResults.txt");
            ExcelGraphs = GetProjectRootPath("ExcelGraphs.csv");
        }

        private static string GetProjectRootPath(string fileName)
        {
            string binPath = AppContext.BaseDirectory;
            string projectRoot = binPath[..binPath.IndexOf("bin")];
            return Path.Combine(projectRoot, fileName);
        }
    }
}