

namespace Project
{
    class Program
    {
        public static async Task Main()
        {

            // await GetData.GetLinksSitesWithMathes();
            FileWorkerAndBuildArrays.BuildArrays();
            Analyzer.PrintAverages();
            Analyzer.PrintWinnerLoserAverages();
            FileWorkerAndBuildArrays.BuildArrays();
            // CsvExporter.ExportToCsv(new Configs().ExcelGraphs);
            StatisticalTester.RunAllTests();
        }
    }

}
