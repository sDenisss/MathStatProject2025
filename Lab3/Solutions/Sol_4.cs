using Plotly.NET;
using MathNet.Numerics.IntegralTransforms;

namespace Lab3.Solutions
{
    class Sol_4
    {   
        public static void CheckHypothesisAboutUnknownVariance(double[] data, Sigma0Direction direction, double alpha = 0.05)
        {
            int n = data.Length;
            double mean = data.Average();

            // Выборочная дисперсия s²
            double sumSq = data.Sum(x => Math.Pow(x - mean, 2));
            double s2 = sumSq / (n - 1);

            // σ₀² = [s²] ± 0.5
            double sigma0Sq = Math.Floor(s2) + (direction == Sigma0Direction.PlusHalf ? 0.5 : -0.5);

            // Z-статистика по асимптотической норме
            double z = (s2 - sigma0Sq) / Math.Sqrt((2 * Math.Pow(sigma0Sq, 2)) / n);

            // Критическое значение
            double zCritical = 1.96; // для alpha = 0.05

            Console.WriteLine($"Выборочная дисперсия s² = {s2:F4}");
            Console.WriteLine($"Гипотетическая дисперсия σ₀² = {sigma0Sq}");
            Console.WriteLine($"Z = {z:F4}, Z_crit = ±{zCritical:F4}");

            if (Math.Abs(z) < zCritical)
                Console.WriteLine("Нет оснований отвергать гипотезу H₀ о σ².\n");
            else
                Console.WriteLine("Гипотеза H₀ о σ² отвергается.\n");
        }
    }
}