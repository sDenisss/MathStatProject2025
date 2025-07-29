using System.Diagnostics;
using System.Text;

public class FileWorker
{
    public void OutputTextInFile(string pathOutput, StringBuilder strInFile)
    {
        // Проверка на null
        if (strInFile == null)
        {
            throw new ArgumentNullException(nameof(strInFile), "StringBuilder не может быть null");
        }

        // Запись в файл
        string[] lines = strInFile.ToString().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        using (StreamWriter file = new StreamWriter(pathOutput))
        {
            foreach (string line in lines)
            {
                file.WriteLine(line); // выводим построчно
            }
        }
    }

    public static void OpenFile(string pathToFile)
    {
        Process.Start("notepad.exe", pathToFile);
    }

}