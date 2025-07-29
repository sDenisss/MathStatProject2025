using Plotly.NET;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace Lab3.Solutions
{
    class Sol
    {
        public static void CheckHypothesisA(double[] Akit)
        {
            int n = Akit.Length;
            int[] values = Akit.Select(x => (int)x).ToArray();
            int[] observed = new int[7];

            foreach (var val in values)
            {
                if (val >= 0 && val <= 6)
                    observed[val]++;
            }

            Console.WriteLine("== Проверка гипотезы о равномерном распределении ==");
            double expectedUniform = n / 7.0;
            double chi2_uniform = 0;

            for (int i = 0; i <= 6; i++)
            {
                chi2_uniform += Math.Pow(observed[i] - expectedUniform, 2) / expectedUniform;
            }

            double critical_uniform = 12.592; // χ²0.05,6
            Console.WriteLine($"x² наблюдаемое: {chi2_uniform:F3}");
            Console.WriteLine($"x² критическое (α=0.05, df=6): {critical_uniform}");
            Console.WriteLine(chi2_uniform > critical_uniform
                ? "Гипотеза о равномерном распределении отвергается"
                : "Нет оснований отвергать гипотезу о равномерном распределении");

            Console.WriteLine("\n== Проверка гипотезы о распределении Пуассона ==");

            // λ по выборке
            double lambda = Akit.Average();

            double[] poissonProbs = new double[7];
            for (int i = 0; i <= 6; i++)
            {
                poissonProbs[i] = Poisson.PMF(lambda, i);
            }

            double[] expectedPoisson = poissonProbs.Select(p => p * n).ToArray();
            double chi2_poisson = 0;
            for (int i = 0; i <= 6; i++)
            {
                if (expectedPoisson[i] >= 5) // критерий применимости
                {
                    chi2_poisson += Math.Pow(observed[i] - expectedPoisson[i], 2) / expectedPoisson[i];
                }
            }

            double critical_poisson = 11.070; // χ²0.05,5
            Console.WriteLine($"λ (среднее): {lambda:F3}");
            Console.WriteLine($"x² наблюдаемое: {chi2_poisson:F3}");
            Console.WriteLine($"x² критическое (α=0.05, df=5): {critical_poisson}");
            Console.WriteLine(chi2_poisson > critical_poisson
                ? "Гипотеза о распределении Пуассона отвергается"
                : "Нет оснований отвергать гипотезу о распределении Пуассона");
        }


        public static void TestKolmogorov(double[] data)
        {
            int n = data.Length;
            Array.Sort(data);

            // Параметры выборки
            double mean = data.Average();
            double stdDev = Math.Sqrt(data.Select(x => Math.Pow(x - mean, 2)).Sum() / n);
            double a = data.Min();
            double b = data.Max();
            double laplaceB = stdDev / Math.Sqrt(2); // параметр b для Лапласа

            // Распределения
            Func<double, double> cdfUniform = x =>
                x < a ? 0 :
                x >= b ? 1 :
                (x - a) / (b - a);

            Func<double, double> cdfNormal = x => Normal.CDF(mean, stdDev, x);

            Func<double, double> cdfLaplace = x =>
            {
                if (x < mean)
                    return 0.5 * Math.Exp((x - mean) / laplaceB);
                else
                    return 1 - 0.5 * Math.Exp(-(x - mean) / laplaceB);
            };

            Console.WriteLine("\n== Критерий Колмогорова–Смирнова ==\n");
            RunKolmogorovTest(data, cdfNormal, "Нормальное распределение");
            RunKolmogorovTest(data, cdfLaplace, "Распределение Лапласа");
            RunKolmogorovTest(data, cdfUniform, "Равномерное распределение");
        }

        private static void RunKolmogorovTest(double[] sortedData, Func<double, double> theoreticalCdf, string hypothesisName)
        {
            int n = sortedData.Length;
            double dMax = 0;

            for (int i = 0; i < n; i++)
            {
                double empiricalFn = (i + 1) / (double)n;
                double empiricalPrev = i / (double)n;
                double theoretical = theoreticalCdf(sortedData[i]);

                double diff1 = Math.Abs(empiricalFn - theoretical);
                double diff2 = Math.Abs(empiricalPrev - theoretical);
                dMax = Math.Max(dMax, Math.Max(diff1, diff2));
            }

            double dCritical = 1.36 / Math.Sqrt(n); // Для α = 0.05
            Console.WriteLine($"Проверка гипотезы: {hypothesisName}");
            Console.WriteLine($"Dn = {dMax:F4}, D_crit = {dCritical:F4}");
            Console.WriteLine(dMax > dCritical
                ? "Гипотеза отвергается\n"
                : "Гипотеза не отвергается\n");
        }
    }
}