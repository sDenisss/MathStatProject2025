using Plotly.NET;
using MathNet.Numerics.Distributions;

namespace Lab3.Solutions
{
    class Sol_3
    {
        public static void CheckHypothesisAboutUnknownAverage(double[] data, A0Direction a0Direction, double alpha = 0.05)
        {
            int n = data.Length;
            double mean = data.Average();

            // a₀ = [x̄] ± 0.5
            double a0 = Math.Floor(mean) + (a0Direction == A0Direction.PlusHalf ? 0.5 : -0.5);

            // Стандартное отклонение
            double variance = data.Sum(x => Math.Pow(x - mean, 2)) / (n - 1);
            double stdDev = Math.Sqrt(variance);

            // Z-статистика
            double z = (mean - a0) / (stdDev / Math.Sqrt(n));

            // Критическое значение (через MathNet.Numerics)
            double zCritical = Normal.InvCDF(0, 1, 1 - alpha / 2); // двусторонний критерий

            Console.WriteLine($"Среднее выборки x̄ = {mean:F4}");
            Console.WriteLine($"a₀ = {a0}");
            Console.WriteLine($"Стандартное отклонение s = {stdDev:F4}");
            Console.WriteLine($"Z = {z:F4}, Z_crit = ±{zCritical:F4}");

            if (Math.Abs(z) < zCritical)
                Console.WriteLine("Нет оснований отвергать нулевую гипотезу H₀.");
            else
                Console.WriteLine("Нулевая гипотеза H₀ отвергается.");
        }
    }
}