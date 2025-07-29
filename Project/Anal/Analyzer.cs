using System;
using System.Linq;

using System;
using System.Linq;

namespace Project
{
    public class Analyzer
    {
        public static void PrintAverages()
        {
            int matchCount = Arrays.scores1.Length;

            double avgPoss1 = Arrays.possession1.Average();
            double avgPoss2 = Arrays.possession2.Average();

            double avgPassAcc1 = Arrays.passCompletePercent1.Average();
            double avgPassAcc2 = Arrays.passCompletePercent2.Average();

            double avgPassTotal1 = Arrays.passTotal1.Average();
            double avgPassTotal2 = Arrays.passTotal2.Average();

            double avgShotsAcc1 = Arrays.shotsOnTargetPercent1.Average();
            double avgShotsAcc2 = Arrays.shotsOnTargetPercent2.Average();

            double avgShotsTotal1 = Arrays.shotsTotal1.Average();
            double avgShotsTotal2 = Arrays.shotsTotal2.Average();

            double avgGoals1 = Arrays.scores1.Average();
            double avgGoals2 = Arrays.scores2.Average();

            double avgXG1 = Arrays.xG1.Average();
            double avgXG2 = Arrays.xG2.Average();

            Console.WriteLine("=== Средние значения по всем матчам ===");
            Console.WriteLine($"Владение мячом: {avgPoss1:F1}% - {avgPoss2:F1}%");
            Console.WriteLine($"Точность пасов: {avgPassAcc1:F1}% - {avgPassAcc2:F1}%");
            Console.WriteLine($"Всего пасов: {avgPassTotal1:F0} - {avgPassTotal2:F0}");
            Console.WriteLine($"Точность ударов: {avgShotsAcc1:F1}% - {avgShotsAcc2:F1}%");
            Console.WriteLine($"Ударов всего: {avgShotsTotal1:F1} - {avgShotsTotal2:F1}");
            Console.WriteLine($"Голы: {avgGoals1:F2} - {avgGoals2:F2}");
            Console.WriteLine($"Ожидаемые голы (xG): {avgXG1:F2} - {avgXG2:F2}");
        }

        public static void PrintWinnerLoserAverages()
        {
            Console.WriteLine("\n=== Победители vs Проигравшие ===");

            var winners = new MatchStats();
            var losers = new MatchStats();

            for (int i = 0; i < Arrays.scores1.Length; i++)
            {
                if (Arrays.scores1[i] > Arrays.scores2[i])
                {
                    winners.Add(i, team1: true);
                    losers.Add(i, team1: false);
                }
                else if (Arrays.scores2[i] > Arrays.scores1[i])
                {
                    winners.Add(i, team1: false);
                    losers.Add(i, team1: true);
                }
            }

            winners.PrintAverages("Победители");
            losers.PrintAverages("Проигравшие");
        }
    }

    public class MatchStats
    {
        private List<int> possession = new();
        private List<int> passTotal = new();
        private List<int> passAccPercent = new();
        private List<int> shotsTotal = new();
        private List<int> shotsAccPercent = new();
        private List<int> goals = new();
        private List<double> xG = new();

        public void Add(int i, bool team1)
        {
            possession.Add(team1 ? Arrays.possession1[i] : Arrays.possession2[i]);
            passTotal.Add(team1 ? Arrays.passTotal1[i] : Arrays.passTotal2[i]);
            passAccPercent.Add(team1 ? Arrays.passCompletePercent1[i] : Arrays.passCompletePercent2[i]);
            shotsTotal.Add(team1 ? Arrays.shotsTotal1[i] : Arrays.shotsTotal2[i]);
            shotsAccPercent.Add(team1 ? Arrays.shotsOnTargetPercent1[i] : Arrays.shotsOnTargetPercent2[i]);
            goals.Add(team1 ? Arrays.scores1[i] : Arrays.scores2[i]);
            xG.Add(team1 ? Arrays.xG1[i] : Arrays.xG2[i]);
        }

        public void PrintAverages(string label)
        {
            Console.WriteLine($"\n{label}:");
            Console.WriteLine($"Владение мячом: {possession.Average():F1}%");
            Console.WriteLine($"Точность пасов: {passAccPercent.Average():F1}%");
            Console.WriteLine($"Пасы всего: {passTotal.Average():F0}");
            Console.WriteLine($"Точность ударов: {shotsAccPercent.Average():F1}%");
            Console.WriteLine($"Ударов всего: {shotsTotal.Average():F1}");
            Console.WriteLine($"Голы: {goals.Average():F2}");
            Console.WriteLine($"Ожидаемые голы (xG): {xG.Average():F2}");
        }
    }
}
