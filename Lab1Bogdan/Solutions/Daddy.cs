using System.Globalization;
using Plotly.NET;
using ZedGraph;
using System.Drawing;
using Plotly.NET.LayoutObjects;


namespace Lab1.Solutions
{
    public class Daddy
    {
         public static void MaxMinAndDifference(double[] numbers, int n) {
            double min = numbers[0];
            double max = numbers[n-1];
            double differenceMaxMin = max - min;

            System.Console.WriteLine("Количество элементов " + n);

            System.Console.WriteLine($"Минимальное значение: {min}");
            System.Console.WriteLine($"Максимальное значение: {max}");
            System.Console.WriteLine($"Размах: {differenceMaxMin}");
        }

        public static void SortAndInput(double[] numbers, int n) {
            Console.WriteLine("Исходные данные: ");
            for (int i = 0; i < n; i++)
            {
                Console.Write(numbers[i] + " ");
            }

            Array.Sort(numbers); // Сортируем массив

            Console.WriteLine("\nВариационный ряд:");
            for (int i = 0; i < n; i++)
            {
                Console.Write(numbers[i] + " ");
            }
        }

        public static void CreateStatRyad(double[] numbers) {
            Array.Sort(numbers); // Сортируем массив

            double intervalCount = Math.Round(1 + 3.322 * Math.Log10(numbers.Length));
            double intervalDiff = (numbers[numbers.Length - 1] - numbers[0]) / intervalCount; 
            
            double a = numbers[0];
            double b = a + intervalDiff;
            int c = 0; 

            foreach (int n in numbers) {
                if (n >= a && n < b) {
                    c++; // Увеличиваем счетчик элементов в текущем интервале
                } else {
                    Console.WriteLine($"Интервал [{a:F2} - {b:F2}]: {c} элементов");

                    // Переход к следующему интервалу
                    while (n >= b) { // Пока число не попадет в новый интервал
                        a += intervalDiff;
                        b += intervalDiff;
                        c = 0; // Обнуляем счетчик
                    }
                    c++; // Засчитываем текущий элемент
                }
            }
            // Выводим последний интервал
            Console.WriteLine($"Интервал [{a:F2} - {b:F2}]: {c} элементов");
        }


        public static void CalculateSkewnessAndKurtosis(double[] numbers)
        {
            int N = numbers.Length;
            double mean = numbers.Average();
            double variance = numbers.Sum(x => Math.Pow(x - mean, 2)) / N;
            double stdDev = Math.Sqrt(variance);

            double skewness = numbers.Sum(x => Math.Pow(x - mean, 3)) / (N * Math.Pow(stdDev, 3));
            double kurtosis = numbers.Sum(x => Math.Pow(x - mean, 4)) / (N * Math.Pow(stdDev, 4)) - 3;

            Console.WriteLine($"Коэффициент асимметрии (Skewness): {skewness:F4}");
            Console.WriteLine($"Коэффициент эксцесса (Kurtosis): {kurtosis:F4}");
        }

        public static void BuildBoxPlot(double[] numbers, string fileName)
        {
            // Создаем boxplot
            var chart = Chart2D.Chart.BoxPlot<double, double, string>(
                numbers
            );

            // Сохраняем график в файл
            chart.SaveHtml(fileName);
            // chart.Show();
        }

        public static void BuildHistogram(double[] numbers, string fileName)
        {
            

            var chart = Chart2D.Chart.Histogram<double, double, string>(
                numbers
            )
            .WithTitle("Гистограмма");
            // Сохраняем график в файл
            chart.SaveHtml(fileName);
            // chart.Show();
        }

        public static void BuildPolygon(double[] numbers, string fileName)
        {
            // Сортируем массив
            Array.Sort(numbers);

            // Разбиваем данные на интервалы (оптимальное количество)
            int binCount = (int)Math.Round(1 + 3.322 * Math.Log10(numbers.Length));
            double min = numbers.Min(), max = numbers.Max();
            double binWidth = (max - min) / binCount;

            // Средние значения интервалов
            double[] binCenters = Enumerable.Range(0, binCount)
                .Select(i => min + binWidth * (i + 0.5))
                .ToArray();

            // Частоты
            double[] frequencies = new double[binCount];

            // Границы интервалов
            double a = min;
            double b = a + binWidth;

            // Индекс текущего интервала
            int binIndex = 0;

            // Подсчет частот
            foreach (var num in numbers)
            {
                // Если число выходит за пределы текущего интервала, переходим к следующему
                while (num >= b && binIndex < binCount - 1)
                {
                    a = b;
                    b += binWidth;
                    binIndex++;
                }

                // Увеличиваем частоту текущего интервала
                frequencies[binIndex]++;
            }

            // Строим график
            var polygonChart = Chart2D.Chart.Line<double, double, string>(
                binCenters, frequencies
            )
            .WithTitle("Полигон частот");

            // Сохраняем
            polygonChart.SaveHtml(fileName);
        }

        private static double f (double x)
        {
            if (x == 0)
            {
                return 1;
            }

            return Math.Sin (x) / x;
        }
        public static void BuildEmpFuncRasp(double[] numbers, string fileName)
        {
                // // Получим панель для рисования
                // GraphPane pane = zedGraph.GraphPane;

                // // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
                // pane.CurveList.Clear ();

                // // Создадим список точек
                // PointPairList list = new PointPairList ();

                // double xmin = -50;
                // double xmax = 50;

                // // Заполняем список точек
                // for (double x = xmin; x <= xmax; x += 0.01)
                // {
                //     // добавим в список точку
                //     list.Add (x, f(x));
                // }

                // // Создадим кривую с названием "Sinc",
                // // которая будет рисоваться голубым цветом (Color.Blue),
                // // Опорные точки выделяться не будут (SymbolType.None)
                // LineItem myCurve = pane.AddCurve ("Sinc", list, Color.Blue, SymbolType.None);

                // // Вызываем метод AxisChange (), чтобы обновить данные об осях.
                // // В противном случае на рисунке будет показана только часть графика,
                // // которая умещается в интервалы по осям, установленные по умолчанию
                // zedGraph.AxisChange ();

                // // Обновляем график
                // zedGraph.Invalidate ();

            // Сортируем
            Array.Sort(numbers);

            // Кумулята
            double[] cumulative = Enumerable.Range(1, numbers.Length)
                .Select(i => (double)i / numbers.Length)
                .ToArray();

            // Строим график
            var empiricalChart = Chart2D.Chart.Line<double, double, string>(
                numbers, cumulative
            )
            .WithTitle("Эмпирическая функция распределения (Кумулята)");

            // Сохраняем
            empiricalChart.SaveHtml(fileName);
        }
    }
}