using MathNet.Numerics.Distributions;


namespace Lab3.Solutions
{
    class Sol_5
    {
        public static void CheckEqualityOfMeansWithEqualUnknownVariances(double[] data, double alpha = 0.05)
        {
            int n = data.Length;
            int half = n / 2;

            double[] sample1 = data.Take(half).ToArray();
            double[] sample2 = data.Skip(half).ToArray();

            int n1 = sample1.Length;
            int n2 = sample2.Length;

            double mean1 = sample1.Average();
            double mean2 = sample2.Average();

            double var1 = sample1.Sum(x => Math.Pow(x - mean1, 2)) / (n1 - 1);
            double var2 = sample2.Sum(x => Math.Pow(x - mean2, 2)) / (n2 - 1);

            // Объединённая дисперсия
            double sp2 = ((n1 - 1) * var1 + (n2 - 1) * var2) / (n1 + n2 - 2);
            double sp = Math.Sqrt(sp2);

            // T-статистика
            double t = (mean1 - mean2) / (sp * Math.Sqrt(1.0 / n1 + 1.0 / n2));

            // Критическое значение t (двусторонний тест)
            int df = n1 + n2 - 2;
            double tCritical = StudentT.InvCDF(0, 1, df, 1 - alpha / 2); // MathNet.Numerics

            Console.WriteLine($"Среднее первой подвыборки: {mean1:F4}, дисперсия: {var1:F4}");
            Console.WriteLine($"Среднее второй подвыборки: {mean2:F4}, дисперсия: {var2:F4}");
            Console.WriteLine($"Объединённая дисперсия: {sp2:F4}");
            Console.WriteLine($"T = {t:F4}, t_crit = ±{tCritical:F4}");

            if (Math.Abs(t) < tCritical)
                Console.WriteLine("Нет оснований отвергать гипотезу H₀: средние равны.");
            else
                Console.WriteLine("Гипотеза H₀ отвергается: средние различаются.");
        }

    }
}
