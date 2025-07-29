using Plotly.NET;
using MathNet.Numerics.IntegralTransforms;

namespace Lab1.Solutions
{
    class Sol_4
    {
        public static (double lower, double upper) NormalMeanCI(double[] data)
        {
            double mean = data.Average();
            double variance = data.Select(x => Math.Pow(x - mean, 2)).Average();
            double stdError = Math.Sqrt(variance / data.Length);
            double z = Sol_2.NormalCDF(0, 1, 0.975);
            Console.WriteLine($"ДИ для мат ожидание (нормальное): [{mean - z * stdError:F4}, {mean + z * stdError:F4}]");
            return (mean - z * stdError, mean + z * stdError);
        }

        public static (double lower, double upper) NormalVarianceCI(double[] data)
        {
            double mean = data.Average();
            double variance = data.Select(x => Math.Pow(x - mean, 2)).Average();
            double stdError = Math.Sqrt(2 * Math.Pow(variance, 2) / data.Length);
            double z = Sol_2.NormalCDF(0, 1, 0.975);
            Console.WriteLine($"ДИ для (ср кв отклонение)^2 (нормальное): [{variance - z * stdError:F4}, {variance + z * stdError:F4}]");
            return (variance - z * stdError, variance + z * stdError);
        }

        public static (double lower, double upper) LognormalMeanCI(double[] data)
        {
            double[] logData = data.Select(x => Math.Log(x)).ToArray();
            double mu = logData.Average();
            double sigma2 = logData.Select(x => Math.Pow(x - mu, 2)).Average();
            double term = mu + sigma2 / 2;
            double stdError = Math.Sqrt(sigma2 / data.Length + Math.Pow(sigma2, 2) / (2 * data.Length));
            double z = Sol_2.NormalCDF(0, 1, 0.975);
            Console.WriteLine($"ДИ для среднего логнормального отклонения (логнормальное): [{Math.Exp(term - z * stdError):F4}, {Math.Exp(term + z * stdError):F4}]");
            return (Math.Exp(term - z * stdError), Math.Exp(term + z * stdError));
        }

        public static void CallAllSol4Methods(double[] data)
        {
            NormalMeanCI(data);
            NormalVarianceCI(data);
            LognormalMeanCI(data);
        }

        //////////////////////////////////////////////
        ///              Для 2.4                   ///
        ////////////////////////////////////////////// 
        
        // Точечная оценка дисперсии (исправленная)
        private static double CalculateSampleVariance2(double[] data)
        {
            double mean = data.Average();
            return data.Select(x => Math.Pow(x - mean, 2)).Sum() / (data.Length - 1); // Исправлено на несмещенную оценку
        }

        // Точный ДИ для математического ожидания (нормальное распределение)
        public static (double lower, double upper) ExactNormalMeanCI2(double[] data)
        {
            double mean = data.Average();
            double variance = CalculateSampleVariance2(data);
            double stdError = Math.Sqrt(variance / data.Length);
            
            // Используем t-распределение
            var tDist = new MathNet.Numerics.Distributions.StudentT(0, 1, data.Length - 1);
            double t = tDist.InverseCumulativeDistribution(0.975);
            
            var ci = (mean - t * stdError, mean + t * stdError);
            Console.WriteLine($"Точный ДИ для мат ожидание (нормальное): [{mean - t * stdError:F4}, {mean + t * stdError:F4}]");
            return ci;
        }

        // Точный ДИ для дисперсии (нормальное распределение)
        public static (double lower, double upper) ExactNormalVarianceCI2(double[] data)
        {
            double variance = CalculateSampleVariance2(data);
            int n = data.Length;
            
            // Используем χ²-распределение
            var chi2Lower = new MathNet.Numerics.Distributions.ChiSquared(n - 1);
            var chi2Upper = new MathNet.Numerics.Distributions.ChiSquared(n - 1);
            
            double lowerBound = (n - 1) * variance / chi2Upper.InverseCumulativeDistribution(0.975);
            double upperBound = (n - 1) * variance / chi2Lower.InverseCumulativeDistribution(0.025);
            
            var ci = (lowerBound, upperBound);
            Console.WriteLine($"Точный ДИ для ср кв отклонение)^2 (нормальное): [{lowerBound:F4}, {upperBound:F4}]");
            return ci;
        }

        // Асимптотический ДИ для среднего логнормального распределения
        public static (double lower, double upper) LognormalMeanCI2(double[] data)
        {
            double[] logData = data.Select(x => Math.Log(x)).ToArray();
            double mu = logData.Average();
            double sigma2 = logData.Select(x => Math.Pow(x - mu, 2)).Average();
            double term = mu + sigma2 / 2;
            
            double stdError = Math.Sqrt(sigma2 / data.Length + Math.Pow(sigma2, 2) / (2 * data.Length));
            double z = Sol_2.NormalCDF(0, 1, 0.975);
            
            var ci = (Math.Exp(term - z * stdError), Math.Exp(term + z * stdError));
            Console.WriteLine($"Асимптотический ДИ для среднего логнормального отклонения (логнормальное): [{Math.Exp(term - z * stdError):F4}, {Math.Exp(term + z * stdError):F4}]");
            return ci;
        }

        public static void CallAllSol4Methods2(double[] data)
        {
            ExactNormalMeanCI2(data);
            ExactNormalVarianceCI2(data);
            LognormalMeanCI(data);
        }
    }
}