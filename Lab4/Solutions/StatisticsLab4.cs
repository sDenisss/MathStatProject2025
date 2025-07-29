using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;
using Plotly.NET;
using MathNet.Numerics;

public class StatisticsLab4
{
    // Выборка C (из предыдущего задания)

    // Массив X (столбцы 1, 4, 7, 10)
        public static double[] x = new double[]
        {
                37, 39, 30, 32, 40, 34, 33, 40, 38, 35, 37, 35, 26, 29, 37, 31, 38, 38, 33, 36, 37, 48,
                41, 31, 44, 38, 28, 42, 31, 26, 32, 31, 40, 31, 34, 45, 36, 38, 35, 32, 37, 40, 31,
                42, 39, 34, 36, 31, 41, 27, 27, 37, 38, 47, 40, 33, 36, 41, 35, 41, 39, 33, 35,
                40, 35, 24, 37, 42, 38, 35, 38, 32, 38, 37, 41, 33, 32, 42, 44, 42, 35, 33, 35
        };

        // Массив Y (столбцы 2, 5, 8, 11)
        public static double[] y = new double[]
        {
            159, 174, 133, 137, 173, 145, 150, 162, 157, 140, 157, 149, 114, 118, 166, 134, 154, 159, 151, 144, 158, 202,
            182, 127, 191, 159, 126, 181, 132, 116, 140, 143, 163, 126, 139, 185, 161, 153, 150, 129, 157, 161, 134,
            176, 173, 138, 154, 133, 182, 109, 124, 154, 157, 198, 176, 149, 154, 179, 150, 172, 169, 133, 141,
            169, 147, 115, 160, 168, 153, 159, 162, 146, 154, 167, 182, 139, 128, 178, 178, 180, 142, 136, 141
        };

        // Массив Z (столбцы 3, 6, 9, 12)
        public static double[] z = new double[]
        {
            58, 63, 59, 59, 67, 53, 53, 69, 68, 55, 67, 53, 37, 45, 73, 50, 66, 59, 54, 68, 58, 93,
            72, 51, 71, 63, 55, 81, 46, 42, 58, 59, 61, 43, 53, 74, 65, 63, 62, 51, 63, 67, 59,
            72, 60, 56, 71, 52, 77, 47, 51, 66, 72, 90, 67, 65, 69, 81, 52, 75, 70, 49, 61,
            73, 56, 31, 69, 78, 63, 69, 74, 58, 60, 71, 81, 60, 58, 72, 73, 75, 63, 60, 61
        };

    public void OutputXYZ()
    {
        Console.WriteLine("Массив X:");
        Console.WriteLine(string.Join(", ", x));
        Console.WriteLine("\nМассив Y:");
        Console.WriteLine(string.Join(", ", y));
        Console.WriteLine("\nМассив Z:");
        Console.WriteLine(string.Join(", ", z));
    }

    // 1.1 Построение моделей линейной регрессии
    public void BuildLinearRegressionModels()
    {
        // Модель 1: z = f(x)
        var model1 = SimpleLinearRegression(x, z);
        Console.WriteLine("\nМодель 1: z = {0:F2} + {1:F2}*x", model1.Item1, model1.Item2);
        Console.WriteLine($"R²: {CalculateRSquared(x, z, model1.Item1, model1.Item2):F3}");
        BuildCorrelationField(x, y, "correlation_plot1.html");

        // Модель 2: z = f(y)
        var model2 = SimpleLinearRegression(y, z);
        Console.WriteLine("\nМодель 2: z = {0:F2} + {1:F2}*y", model2.Item1, model2.Item2);
        Console.WriteLine($"R²: {CalculateRSquared(y, z, model2.Item1, model2.Item2):F3}");

        BuildCorrelationField(y, z, "correlation_plot2.html");
    }

    public static void BuildCorrelationField(double[] x, double[] z, string fileToSave)
    {
        // Корреляционное поле (точки)
        var scatter = Chart2D.Chart.Scatter<double, double, string>(
            x: x,
            y: z,
            mode: StyleParam.Mode.Markers,
            Name: "Данные"
        );

        // Построение линейной регрессии: y = a + b * x
        (double a, double b) = LinearRegression(x, z);
        double[] yReg = x.Select(xi => a + b * xi).ToArray();

        var line = Chart2D.Chart.Line<double, double, string>(
            x: x,
            y: yReg,
            Name: $"Регрессия: y = {a:0.00} + {b:0.00}x"
        );

        // Объединяем графики
        var chart = Chart.Combine(new[] { scatter, line })
            .WithTitle("Корреляционное поле и линия регрессии");
            // .WithX_AxisStyle("x")
            // .WithY_AxisStyle("z");

        // Сохраняем как HTML
        chart.SaveHtml(fileToSave);
        Console.WriteLine("График сохранён: correlation_plot.html");
    }

    static (double a, double b) LinearRegression(double[] x, double[] y)
    {
        double xMean = x.Average();
        double yMean = y.Average();
        double b = x.Zip(y, (xi, yi) => (xi - xMean) * (yi - yMean)).Sum()
                 / x.Sum(xi => Math.Pow(xi - xMean, 2));
        double a = yMean - b * xMean;
        return (a, b);
    }
    

    // 1.2 Оценка качества моделей
    public void EvaluateModels()
    {
        // Для модели 1 (z = f(x))
        var model1 = SimpleLinearRegression(x, z);
        var (mae1, elasticity1) = CalculateMAEAndElasticity(x, z, model1.Item1, model1.Item2);
        Console.WriteLine("\nМодель 1:");
        Console.WriteLine($"Средняя ошибка аппроксимации: {mae1:F2}%");
        Console.WriteLine($"Средняя эластичность: {elasticity1:F4}");

        // Для модели 2 (z = f(y))
        var model2 = SimpleLinearRegression(y, z);
        var (mae2, elasticity2) = CalculateMAEAndElasticity(y, z, model2.Item1, model2.Item2);
        Console.WriteLine("\nМодель 2:");
        Console.WriteLine($"Средняя ошибка аппроксимации: {mae2:F2}%");
        Console.WriteLine($"Средняя эластичность: {elasticity2:F4}");

        // F-тест и t-тест
        Console.WriteLine("\nТесты для модели 1:");
        PerformFTestAndTTest(x, z, model1.Item1, model1.Item2);

        Console.WriteLine("\nТесты для модели 2:");
        PerformFTestAndTTest(y, z, model2.Item1, model2.Item2);
    }


    // 1.3 Анализ остатков
    public void AnalyzeResiduals()
    {
        var model1 = SimpleLinearRegression(x, z);
        var residuals1 = CalculateResiduals(x, z, model1.Item1, model1.Item2);
        Console.WriteLine("\nАнализ остатков модели 1:");
        CheckGaussMarkovConditions(residuals1);
        BuildResidualPlot(x, z, model1.Item1, model1.Item2, "residual_plot1.html", "x");


        var model2 = SimpleLinearRegression(y, z);
        var residuals2 = CalculateResiduals(y, z, model2.Item1, model2.Item2);
        Console.WriteLine("\nАнализ остатков модели 2:");
        CheckGaussMarkovConditions(residuals2);
        BuildResidualPlot(y, z, model2.Item1, model2.Item2, "residual_plot2.html", "y");

    }
    public static void BuildResidualPlot(double[] x, double[] z, double a, double b, string fileToSave, string xLabel = "x")
    {
        // Вычисляем предсказанные значения и остатки
        double[] predicted = x.Select(xi => a + b * xi).ToArray(); //предсказаннные значения
        // foreach (var item in predicted)
        // {
        //     System.Console.WriteLine(item);
        // }
        double[] residuals = z.Zip(predicted, (zi, ziHat) => zi - ziHat).ToArray(); //Остаток — это разница между реальным значением и тем, что предсказала модель.

        // График остатков
        var scatter = Chart2D.Chart.Scatter<double, double, string>(
            x: x,
            y: residuals,
            mode: StyleParam.Mode.Markers,
            Name: "Остатки"
        );

        // Линия y = 0
        double[] zeroLine = new double[x.Length];
        var line = Chart2D.Chart.Line<double, double, string>(
            x: x,
            y: zeroLine,
            Name: "y = 0 (идеал)"
        );

        var chart = Chart.Combine(new[] { scatter, line })
            .WithTitle($"График остатков для модели по {xLabel}");

        chart.SaveHtml(fileToSave);
        Console.WriteLine($"График остатков сохранён: {fileToSave}");
    }


    // 1.4 Корреляционный анализ
    public void CorrelationAnalysis()
    {
        double corrXZ = Correlation.Pearson(x, z);
        double corrYZ = Correlation.Pearson(y, z);
        double rSquaredXZ = Math.Pow(corrXZ, 2);
        double rSquaredYZ = Math.Pow(corrYZ, 2);

        Console.WriteLine("\nКорреляционный анализ:");
        Console.WriteLine($"Коэффициент корреляции τxz: {corrXZ:F3}, значимость: {CheckCorrelationSignificance(corrXZ, x.Length)}");
        Console.WriteLine($"Коэффициент детерминации R² (x,z): {rSquaredXZ:F3}");
        Console.WriteLine($"Коэффициент корреляции τyz: {corrYZ:F3}, значимость: {CheckCorrelationSignificance(corrYZ, y.Length)}");
        Console.WriteLine($"Коэффициент детерминации R² (y,z): {rSquaredYZ:F3}");
    }

    // 1.5 Двухфакторная регрессия
    public void BuildMultipleRegressionModel()
    {
        var design = Matrix<double>.Build.DenseOfColumns(new[] {
            Vector<double>.Build.Dense(x.Length, 1.0), // intercept
            Vector<double>.Build.Dense(x),
            Vector<double>.Build.Dense(y)
        });
        
        var zVector = Vector<double>.Build.Dense(z);
        var coefficients = design.QR().Solve(zVector);
        
        Console.WriteLine("\nМодель множественной регрессии:");
        Console.WriteLine($"z = {coefficients[0]:F2} + {coefficients[1]:F2}*x + {coefficients[2]:F2}*y");
        Console.WriteLine($"R²: {CalculateRSquaredMultiple(design, zVector, coefficients):F3}");
    }

    // 1.6-1.8 Анализ множественной регрессии
    public void AnalyzeMultipleRegression()
    {
        var design = Matrix<double>.Build.DenseOfColumns(new[] {
            Vector<double>.Build.Dense(x.Length, 1.0),
            Vector<double>.Build.Dense(x),
            Vector<double>.Build.Dense(y)
        });
        var zVector = Vector<double>.Build.Dense(z);
        var coefficients = design.QR().Solve(zVector);
        
        Console.WriteLine("\nАнализ множественной регрессии:");
        
        // F-тест
        double rSquared = CalculateRSquaredMultiple(design, zVector, coefficients);
        int n = x.Length;
        int k = 2;
        double fStat = (rSquared / k) / ((1 - rSquared) / (n - k - 1));
        double fCrit = new FisherSnedecor(k, n - k - 1).InverseCumulativeDistribution(0.95);
        Console.WriteLine($"F-статистика: {fStat:F2}, критическое значение: {fCrit:F2}");
        Console.WriteLine("Модель статистически значима");
        
        // t-тесты
        var residuals = CalculateResidualsMultiple(design, zVector, coefficients);
        double mse = residuals.Select(r => r * r).Sum() / (n - k - 1);
        var covMatrix = mse * (design.Transpose() * design).Inverse();
        
        for (int i = 0; i < 3; i++)
        {
            double se = Math.Sqrt(covMatrix[i, i]);
            double tStat = coefficients[i] / se;
            double tCrit = new StudentT(0, 1, n - k - 1).InverseCumulativeDistribution(0.975);
            Console.WriteLine($"Коэффициент b{i}: {coefficients[i]:F2}, t-статистика: {tStat:F2}, критическое значение: ±{tCrit:F2}");
        }
        
        // Проверка условий Гаусса-Маркова
        Console.WriteLine("\nАнализ остатков:");
        CheckGaussMarkovConditions(residuals);
        
        // Средняя ошибка аппроксимации
        double mape = residuals.Select((r, i) => Math.Abs(r / z[i])).Average() * 100;
        Console.WriteLine($"Средняя ошибка аппроксимации: {mape:F2}%");
        
        // Эластичность
        double meanX = x.Average();
        double meanY = y.Average();
        double meanZ = z.Average();
        Console.WriteLine($"Эластичность по x: {coefficients[1] * meanX / meanZ:F3}");
        Console.WriteLine($"Эластичность по y: {coefficients[2] * meanY / meanZ:F3}");
        
        // Мультиколлинеарность
        double vifX = 1 / (1 - Math.Pow(Correlation.Pearson(x, y), 2));
        Console.WriteLine($"VIF для x: {vifX:F2}");
        Console.WriteLine($"VIF для y: {vifX:F2}");
    }

    // 2.1-2.2 Проверка гипотез для выборок D и E
    public void TestHypothesesForSamples(double[][] sampleD, double[][] sampleE)
    {
        Console.WriteLine("\nТестирование выборки D (α=0.05):");
        PerformANOVATest(sampleD, 0.05);
        
        Console.WriteLine("\nТестирование выборки E (α=0.01):");
        PerformANOVATest(sampleE, 0.01);
    }

    // Вспомогательные методы
    private Tuple<double, double> SimpleLinearRegression(double[] x, double[] y)
    {
        double xMean = x.Average();
        double yMean = y.Average();
        
        double covariance = x.Zip(y, (xi, yi) => (xi - xMean) * (yi - yMean)).Sum();
        double xVariance = x.Sum(xi => Math.Pow(xi - xMean, 2));
        
        double b = covariance / xVariance;
        double a = yMean - b * xMean;
        
        return Tuple.Create(a, b);
    }

    private double CalculateRSquared(double[] x, double[] y, double a, double b)
    {
        double yMean = y.Average();
        double sst = y.Sum(yi => Math.Pow(yi - yMean, 2));
        double sse = y.Zip(x, (yi, xi) => Math.Pow(yi - (a + b * xi), 2)).Sum();
        return 1 - sse / sst;
    }

    private double CalculateRSquaredMultiple(Matrix<double> design, Vector<double> y, Vector<double> coefficients)
    {
        double yMean = y.Average();
        double sst = y.Sum(yi => Math.Pow(yi - yMean, 2));
        double sse = (design * coefficients - y).PointwisePower(2).Sum();
        return 1 - sse / sst;
    }

    private Tuple<double, double> CalculateMAEAndElasticity(double[] x, double[] y, double a, double b)
    {
        double mae = x.Zip(y, (xi, yi) => Math.Abs(yi - (a + b * xi)) / yi).Average() * 100;
        double elasticity = x.Zip(y, (xi, yi) => b * xi / (a + b * xi)).Average();
        return Tuple.Create(mae, elasticity);
    }

    private double[] CalculateResiduals(double[] x, double[] y, double a, double b)
    {
        return x.Zip(y, (xi, yi) => yi - (a + b * xi)).ToArray();
    }

    private double[] CalculateResidualsMultiple(Matrix<double> design, Vector<double> y, Vector<double> coefficients)
    {
        return (design * coefficients - y).ToArray();
    }

    private void CheckGaussMarkovConditions(double[] residuals)
    {
        // 1. Нулевое среднее
        double mean = residuals.Average();
        Console.WriteLine($"Среднее остатков: {mean:E3} (должно быть близко к 0)");
        
        // 2. Гомоскедастичность (тест Бреуша-Пагана)
        double bp = residuals.Zip(Enumerable.Range(1, residuals.Length), 
                                (r, i) => r * r * i).Sum();
        double bpCrit = new ChiSquared(1).InverseCumulativeDistribution(0.95);
        Console.WriteLine($"Тест на гомоскедастичность: статистика {bp:F2}, критическое значение {bpCrit:F2}");
        
        // 3. Отсутствие автокорреляции (тест Дарбина-Уотсона)
        double dw = residuals.Zip(residuals.Skip(1), (r1, r2) => Math.Pow(r2 - r1, 2)).Sum() / 
                   residuals.Sum(r => r * r);
        Console.WriteLine($"Тест Дарбина-Уотсона: {dw:F2} (должен быть около 2)");
    }

    private void PerformFTestAndTTest(double[] x, double[] y, double a, double b)
    {
        int n = x.Length;
        double yMean = y.Average();
        
        // F-тест
        double sst = y.Sum(yi => Math.Pow(yi - yMean, 2));
        double sse = y.Zip(x, (yi, xi) => Math.Pow(yi - (a + b * xi), 2)).Sum();
        double fStat = ((sst - sse) / 1) / (sse / (n - 2));
        double fCrit = new FisherSnedecor(1, n - 2).InverseCumulativeDistribution(0.95);
        Console.WriteLine($"F-тест: статистика {fStat:F2}, критическое значение {fCrit:F2}");
        
        // t-тест для коэффициента
        double xMean = x.Average();
        double xVar = x.Sum(xi => Math.Pow(xi - xMean, 2));
        double se = Math.Sqrt(sse / (n - 2) / xVar);
        double tStat = b / se;
        double tCrit = new StudentT(0, 1, n - 2).InverseCumulativeDistribution(0.975);
        Console.WriteLine($"t-тест для коэффициента: статистика {tStat:F2}, критическое значение ±{tCrit:F2}");
    }

    private bool CheckCorrelationSignificance(double r, int n)
    {
        double t = r * Math.Sqrt(n - 2) / Math.Sqrt(1 - r * r);
        double pValue = 2 * (1 - new StudentT(0, 1, n - 2).CumulativeDistribution(Math.Abs(t)));
        return pValue < 0.05;
    }

    private void PerformANOVATest(double[][] samples, double alpha)
    {
        int k = samples.Length;
        int n = samples.Sum(g => g.Length);
        
        double grandMean = samples.SelectMany(g => g).Average();
        double ssb = samples.Sum(g => g.Length * Math.Pow(g.Average() - grandMean, 2));
        double ssw = samples.Sum(g => g.Sum(xi => Math.Pow(xi - g.Average(), 2)));
        
        double msb = ssb / (k - 1);
        double msw = ssw / (n - k);
        double fStat = msb / msw;
        
        double fCrit = new FisherSnedecor(k - 1, n - k).InverseCumulativeDistribution(1 - alpha);
        
        Console.WriteLine($"ANOVA: F = {fStat:F2}, критическое значение = {fCrit:F2}");
        Console.WriteLine(fStat > fCrit ? "Гипотеза о равенстве средних отвергается" : "Гипотеза не отвергается");
    }
}
