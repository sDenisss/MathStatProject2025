using System;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using System.Collections.Generic;
using MathNet.Numerics.Statistics;


namespace Project
{
    public static class StatisticalTester
    {
        public static void RunAllTests()
        {
            TestMetric("Possession", GetWinners(Arrays.possession1, true), GetLosers(Arrays.possession2, false));
            TestMetric("Pass Accuracy", GetWinners(Arrays.passCompletePercent1, true), GetLosers(Arrays.passCompletePercent2, false));
            TestMetric("Shots Accuracy", GetWinners(Arrays.shotsOnTargetPercent1, true), GetLosers(Arrays.shotsOnTargetPercent2, false));
            TestMetric("Goals", GetWinners(Arrays.scores1, true), GetLosers(Arrays.scores2, false));
            TestMetric("xG", GetWinners(Arrays.xG1, true), GetLosers(Arrays.xG2, false));
        }

        private static List<double> GetWinners(int[] data, bool team1)
        {
            var result = new List<double>();
            for (int i = 0; i < FileWorkerAndBuildArrays.matchCount; i++)
            {
                if (team1 && Arrays.scores1[i] > Arrays.scores2[i])
                    result.Add(data[i]);
                else if (!team1 && Arrays.scores2[i] > Arrays.scores1[i])
                    result.Add(data[i]);
            }
            return result;
        }

        private static List<double> GetWinners(double[] data, bool team1)
        {
            var result = new List<double>();
            for (int i = 0; i < FileWorkerAndBuildArrays.matchCount; i++)
            {
                if (team1 && Arrays.scores1[i] > Arrays.scores2[i])
                    result.Add(data[i]);
                else if (!team1 && Arrays.scores2[i] > Arrays.scores1[i])
                    result.Add(data[i]);
            }
            return result;
        }

        private static List<double> GetLosers(int[] data, bool team1)
        {
            var result = new List<double>();
            for (int i = 0; i < FileWorkerAndBuildArrays.matchCount; i++)
            {
                if (team1 && Arrays.scores2[i] > Arrays.scores1[i])
                    result.Add(data[i]);
                else if (!team1 && Arrays.scores1[i] > Arrays.scores2[i])
                    result.Add(data[i]);
            }
            return result;
        }

        private static List<double> GetLosers(double[] data, bool team1)
        {
            var result = new List<double>();
            for (int i = 0; i < FileWorkerAndBuildArrays.matchCount; i++)
            {
                if (team1 && Arrays.scores2[i] > Arrays.scores1[i])
                    result.Add(data[i]);
                else if (!team1 && Arrays.scores1[i] > Arrays.scores2[i])
                    result.Add(data[i]);
            }
            return result;
        }

        private static void TestMetric(string name, List<double> winners, List<double> losers)
        {
            double mean1 = winners.Average();
            double mean2 = losers.Average();
            double var1 = Statistics.Variance(winners);
            double var2 = Statistics.Variance(losers);
            int n1 = winners.Count;
            int n2 = losers.Count;

            double se = Math.Sqrt(var1 / n1 + var2 / n2);
            double tStat = (mean1 - mean2) / se;

            // Степени свободы по формуле Уэлча
            double df = Math.Pow(var1 / n1 + var2 / n2, 2) /
                        ((Math.Pow(var1 / n1, 2) / (n1 - 1)) + (Math.Pow(var2 / n2, 2) / (n2 - 1)));

            // Распределение Стьюдента
            var tDist = new StudentT(0, 1, df);
            double pValue = 2 * (1 - tDist.CumulativeDistribution(Math.Abs(tStat))); // двусторонний тест

            Console.WriteLine($"\n=== {name} ===");
            Console.WriteLine($"Среднее (победители): {mean1:F2}");
            Console.WriteLine($"Среднее (проигравшие): {mean2:F2}");
            Console.WriteLine($"t-статистика: {tStat:F3}");
            Console.WriteLine($"p-value: {pValue:F4}");

            if (pValue < 0.05)
                Console.WriteLine("→ Различие статистически значимо (p < 0.05)");
            else
                Console.WriteLine("→ Различие НЕ статистически значимо");
        }

    }
}
