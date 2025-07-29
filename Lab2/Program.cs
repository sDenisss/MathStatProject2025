
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Lab1;
using Lab1.Solutions;

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
        //A Метод произведений
        System.Console.WriteLine("МЕТОД ПРОИЗВЕДЕНИЙ(начало)");
        Sol.SortAndInput(Akit);       
        Sol.MaxMinAndDifference(Akit); 
        Sol.CreateStatRyad(Akit);
        Sol.CalculateStatisticalTable(Akit);
        Sol.CalculateStatistics();
        Sol.CalculateCentralMoments();
        Sol.CalculateSkewnessAndKurtosis();
        Sol.SolveModaAndMediana(Akit);
        System.Console.WriteLine("МЕТОД ПРОИЗВЕДЕНИЙ(конец)");

        double mu = StatisticalAnalysis.MathWait;
        double sigma = StatisticalAnalysis.StdDev;
        double[] bins = StatisticalAnalysis.MidsIntervals.ToArray(); // Границы интервалов
        Console.WriteLine("Теоретические частоты (логнормальное):");
        for (int i = 0; i < bins.Length - 1; i++)
        {
            // Интегрируем PDF по интервалу (упрощённо: середина интервала * ширина)
            double mid = (bins[i] + bins[i+1]) / 2;
            double freq = Sol_2.LognormalPDF(mid, mu, sigma) * (bins[i+1] - bins[i]);
            StatisticalAnalysis.TeorFreq.Add(freq);
            Console.WriteLine($"[{bins[i]:F1}-{bins[i+1]:F1}]: {freq:F4}");
        }

        Sol_3.BuildPolygonEmp(Akit, "PolygonsA");
        Sol_3.BuildPolygonTeor(Akit, "PolygonsTeorA");

        Sol_4.CallAllSol4Methods(Akit);
        Sol_5.CompareWithLab1A();


        StatisticalAnalysis.MidsIntervals.Clear();
        StatisticalAnalysis.OtnosFreq.Clear();
        StatisticalAnalysis.TeorFreq.Clear();


        System.Console.WriteLine("Выборка B");
        //B Метод сумм
        System.Console.WriteLine("МЕТОД СУММ(начало)");
        Sol.SortAndInput(Bkit);       
        Sol.MaxMinAndDifference(Bkit); 
        Sol.CreateStatRyad(Bkit);
        Sol.CalculateStatisticalTable(Bkit);
        Sol.BuildTable(Bkit);
        Sol.SolveSAndD();
        Sol.SolveStartMoments(Bkit);

        Sol.CalculateStatistics();
        Sol.CalculateCentralMoments();
        Sol.CalculateSkewnessAndKurtosis();
        Sol.SolveModaAndMediana(Bkit);
        System.Console.WriteLine("МЕТОД СУММ(конец)");

        // mu = StatisticalAnalysis.MathWait;
        mu = Sol_2.CalculateMean(Bkit);
        sigma = StatisticalAnalysis.StdDev;
        bins = StatisticalAnalysis.MidsIntervals.ToArray();

        Console.WriteLine("Теоретические частоты (нормальное):");
        for (int i = 0; i < bins.Length - 1; i++)
        {
            double prob = Sol_2.NormalCDF(bins[i+1], mu, sigma) - Sol_2.NormalCDF(bins[i], mu, sigma);
            StatisticalAnalysis.TeorFreq.Add(prob);
            Console.WriteLine($"[{bins[i]:F1}-{bins[i+1]:F1}]: {prob:F4}");
        }

        Sol_3.BuildPolygonEmp(Bkit, "PolygonsB");
        Sol_3.BuildPolygonTeor(Bkit, "PolygonsTeorB");

        Sol_4.CallAllSol4Methods2(Bkit);
        Sol_5.CompareWithLab1B();
    }
}