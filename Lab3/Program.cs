
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Lab3.Infrastructure;
using Lab1;
using Lab3.Solutions;

public class Program
{
    public static double[] Akit = {
                2, 3, 1, 6, 4, 6, 3, 3, 1, 3, 1, 2, 4, 4, 4, 3, 0, 3, 2, 4,
                2, 3, 2, 3, 3, 2, 0, 6, 1, 0, 2, 2, 6, 2, 0, 2, 4, 3, 1, 5,
                3, 0, 4, 4, 3, 5, 3, 2, 5, 2, 0, 2, 0, 2, 5, 0, 1, 3, 3, 2,
                0, 2, 2, 2, 5 };
    public static double[] Bkit = {

                184, 181, 201, 178, 190, 188, 181, 180, 186, 180, 176, 186,
                185, 184, 187, 176, 189, 194, 196, 190, 193, 180, 186, 195,
                197, 189, 197, 190, 176, 200, 196, 188, 203, 191, 180, 181,
                188, 185, 188, 173, 184, 180, 189, 178, 190, 175, 193, 184,
                177, 179, 177, 203, 185, 182, 191, 183, 183, 211, 189, 177,
                195, 196, 175, 188, 189, 187, 193, 185, 184, 193, 181, 185,
                214, 177, 196, 195, 193, 172, 190, 200, 176, 179, 185, 182,
                175, 180, 179, 170, 206, 181, 197, 197, 180, 193, 192, 200,
                175, 196, 174, 171, 160, 187, 185, 206, 187, 182, 175, 172,
                191, 179, 191, 199, 197, 177, 175, 170, 174, 194, 188, 182,
                179, 186, 190, 183, 196, 183, 185, 174, 195, 179, 197, 182,
                183, 184, 185, 172, 193, 175, 172, 179, 179, 184, 190, 183,
                178, 192, 186, 157, 172, 185, 180, 193, 177, 174, 200, 195,
                184, 186, 185, 206, 192, 189, 189, 184, 183, 182, 179, 186,
                184, 169, 189, 180, 183, 192, 186, 200, 176, 191, 186, 182,
                202, 184, 192, 179, 204, 197, 194, 182, 172, 185, 175, 187,
                182, 184, 186, 201, 197, 188, 188, 194, 184, 193, 178, 191,
                203, 193, 190, 185, 181, 187, 181, 196, 204, 177, 178, 167,
                178, 194, 188, 182, 182, 199, 180, 181, 187, 187, 178, 181,
                180, 182, 160, 183, 193, 189, 193, 191 };
    public static void Main(string[] args)
    {
        Configs openFile = new Configs();
        string outputFilePath = openFile.OutputPath;

        using (var writer = new StreamWriter(outputFilePath))
        {
            Console.SetOut(writer); // перенаправляем вывод в файл

            Console.WriteLine(
                @"| Выборка | Гипотеза 1                | Гипотеза 2                          |
| ------- | ------------------------- | ----------------------------------- |
| `Akit`  | Равномерное распределение | Распределение Пуассона              |
| `Bkit`  | Нормальное распределение  | Распределение Лапласа               |
            ");

            Console.WriteLine("A выборка");
            Sol.CheckHypothesisA(Akit);
            System.Console.WriteLine("----------------");
            Sol_2.ConclusionA();
            System.Console.WriteLine("----------------");
            Sol_3.CheckHypothesisAboutUnknownAverage(Akit, A0Direction.PlusHalf);
            System.Console.WriteLine("----------------");
            Sol_4.CheckHypothesisAboutUnknownVariance(Akit, Sigma0Direction.PlusHalf);
            System.Console.WriteLine("----------------");
            Sol_5.CheckEqualityOfMeansWithEqualUnknownVariances(Akit);
            System.Console.WriteLine("----------------");

            Console.WriteLine();

            Console.WriteLine("B выборка");
            Sol.TestKolmogorov(Bkit);
            System.Console.WriteLine("----------------");
            Sol_2.ConclusionB();
            System.Console.WriteLine("----------------");
            Sol_3.CheckHypothesisAboutUnknownAverage(Bkit, A0Direction.MinusHalf);
            System.Console.WriteLine("----------------");
            Sol_4.CheckHypothesisAboutUnknownVariance(Bkit, Sigma0Direction.MinusHalf);
            System.Console.WriteLine("----------------");
            Sol_5.CheckEqualityOfMeansWithEqualUnknownVariances(Bkit);
            System.Console.WriteLine("----------------");
        }

        // Открыть файл после завершения
        FileWorker.OpenFile(outputFilePath);
    }
}