using System.Diagnostics;
using System.Text;

namespace Project
{

    class FileWorkerAndBuildArrays
    {
        public const int matchCount = 64;
        public static StringBuilder ReadTextInFile(string pathInput)
        {
            string? line;
            StringBuilder strFromFile = new StringBuilder();
            try
            {

                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(pathInput);
                //Read the first line of text
                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window
                    // Console.WriteLine(line);
                    strFromFile.AppendLine(line);
                    //Read the next line
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();
                // Console.WriteLine(strFromFile);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return strFromFile;
        }

        public static void BuildArrays()
        {
            string[] lines = File.ReadAllLines(new Configs().PRPath);
            int matchCount = FileWorkerAndBuildArrays.matchCount;

            // Инициализация массивов
            Arrays.teams1 = new string[matchCount];
            Arrays.teams2 = new string[matchCount];
            Arrays.scores1 = new int[matchCount];
            Arrays.scores2 = new int[matchCount];
            Arrays.possession1 = new int[matchCount];
            Arrays.possession2 = new int[matchCount];
            Arrays.passCompletePercent1 = new int[matchCount];
            Arrays.passTotal1 = new int[matchCount];
            Arrays.passCompletePercent2 = new int[matchCount];
            Arrays.passTotal2 = new int[matchCount];
            Arrays.shotsOnTargetPercent1 = new int[matchCount];
            Arrays.shotsTotal1 = new int[matchCount];
            Arrays.shotsOnTargetPercent2 = new int[matchCount];
            Arrays.shotsTotal2 = new int[matchCount];
            Arrays.xG1 = new double[matchCount];
            Arrays.xG2 = new double[matchCount];

            for (int i = 0; i < matchCount; i++)
            {
                int offset = i * 7;

                // Строка с командами и счётом
                var teamsLine = lines[offset + 1];
                var parts = teamsLine.Split(" ");
                var team1 = parts[0];
                var score1 = parts[1];
                Arrays.teams1[i] = team1;
                Arrays.scores1[i] = int.Parse(score1);

                var team2 = parts[4];
                var score2 = parts[3];
                Arrays.teams2[i] = team2;
                Arrays.scores2[i] = int.Parse(score2);


                // // Владение мячом
                var pos = lines[offset + 2].Split(" ");
                Arrays.possession1[i] = int.Parse(pos[1].Replace("%", ""));
                Arrays.possession2[i] = int.Parse(pos[3].Replace("%", ""));


                // Passing Accuracy
                var passLine = lines[offset + 3].Split(" ");
                Arrays.passCompletePercent1[i] = int.Parse(passLine[6].Replace("%", ""));
                Arrays.passTotal1[i] = int.Parse(passLine[2]);
                Arrays.passCompletePercent2[i] = int.Parse(passLine[8].Replace("%", ""));
                Arrays.passTotal2[i] = int.Parse(passLine[10]);

                // Shots on Target
                var shotsLine = lines[offset + 4].Split(" ");
                Arrays.shotsOnTargetPercent1[i] = int.Parse(shotsLine[7].Replace("%", ""));
                Arrays.shotsTotal1[i] = int.Parse(shotsLine[3]);
                Arrays.shotsOnTargetPercent2[i] = int.Parse(shotsLine[9].Replace("%", ""));
                Arrays.shotsTotal2[i] = int.Parse(shotsLine[11]);

                // xG
                var xg = lines[offset + 5].Split(" ");
                Arrays.xG1[i] = float.Parse(xg[1]);
                Arrays.xG2[i] = float.Parse(xg[3]);
            }
        }

    }
}