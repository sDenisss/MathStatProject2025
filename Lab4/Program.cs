using System;
using Lab4.Infrastructure;

class Program
{
        
        // Пример данных для выборок D и E
        private static double[][] sampleD = new double[][] {
            new double[] {63, 51, 40, 47},           // f1
            new double[] {50, 40, 44, 43},           // f2
            new double[] {65, 45, 34, 53},           // f3
            new double[] {35, 34, 47, 46},           // f4
            new double[] {46, 57, 36, 41},           // f5
            new double[] {43, 60, 53, 57}            // f6
        };

        private static double[][] sampleE = new double[][] {
            new double[] {-35, -46, -37},                   // ef1
            new double[] {-32, -30, -34, -34, -35},         // ef2
            new double[] {-40},                             // ef3
            new double[] {-29, -34, -32, -34, -33},         // ef4
            new double[] {-28, -30, -39, -35, -37, -40, -34}, // ef5
            new double[] {-35, -36, -36},                   // ef6
            new double[] {-30, -34}                         // ef7
        };

    static void Main()
    {

        Configs openFile = new Configs();
        string outputFilePath = openFile.OutputPath;

        using (var writer = new StreamWriter(outputFilePath))
        {
            Console.SetOut(writer); // перенаправляем вывод в файл
            var lab = new StatisticsLab4();

            // Вывод исходных данных
            lab.OutputXYZ();

            // Выполнение заданий для выборки C
            lab.BuildLinearRegressionModels();
            lab.EvaluateModels();
            lab.AnalyzeResiduals();
            lab.CorrelationAnalysis();
            lab.BuildMultipleRegressionModel();
            lab.AnalyzeMultipleRegression();

            lab.TestHypothesesForSamples(sampleD, sampleE);
        }

        // Открыть файл после завершения
        FileWorker.OpenFile(outputFilePath);
    }
}