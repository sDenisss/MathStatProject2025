using System;
using System.IO;
using System.Text;

namespace Project
{
    public static class CsvExporter
    {
        public static void ExportToCsv(string path)
        {
            var sb = new StringBuilder();

            // Заголовки
            sb.AppendLine("Team1,Score1,Possession1,PassAccuracy1,PassTotal1,ShotsAccuracy1,ShotsTotal1,xG1,Team2,Score2,Possession2,PassAccuracy2,PassTotal2,ShotsAccuracy2,ShotsTotal2,xG2");

            for (int i = 0; i < FileWorkerAndBuildArrays.matchCount; i++)
            {
                var line = string.Join(",", new[]
                {
                    Arrays.teams1[i],
                    Arrays.scores1[i].ToString(),
                    Arrays.possession1[i].ToString(),
                    Arrays.passCompletePercent1[i].ToString(),
                    Arrays.passTotal1[i].ToString(),
                    Arrays.shotsOnTargetPercent1[i].ToString(),
                    Arrays.shotsTotal1[i].ToString(),
                    Arrays.xG1[i].ToString("F2").Replace(',', '.'),
                    Arrays.teams2[i],
                    Arrays.scores2[i].ToString(),
                    Arrays.possession2[i].ToString(),
                    Arrays.passCompletePercent2[i].ToString(),
                    Arrays.passTotal2[i].ToString(),
                    Arrays.shotsOnTargetPercent2[i].ToString(),
                    Arrays.shotsTotal2[i].ToString(),
                    Arrays.xG2[i].ToString("F2").Replace(',', '.')
                });

                sb.AppendLine(line);
            }

            File.WriteAllText(path, sb.ToString());
            Console.WriteLine($"CSV-файл успешно создан: {path}");
        }
    }
}
