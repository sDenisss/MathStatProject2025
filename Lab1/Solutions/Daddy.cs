using System.Globalization;
using Plotly.NET;
using ZedGraph;
using System.Drawing;
using Plotly.NET.LayoutObjects;

namespace Lab1.Solutions
{
    public class Daddy
    {
        // Метод для нахождения минимального, максимального значения и размаха массива
        public static void MaxMinAndDifference(double[] numbers, int n) {
            double min = numbers[0]; // Инициализация минимального значения первым элементом массива
            double max = numbers[n-1]; // Инициализация максимального значения последним элементом массива
            double differenceMaxMin = max - min; // Вычисление размаха (разницы между max и min)

            System.Console.WriteLine("Количество элементов " + n); // Вывод количества элементов массива

            System.Console.WriteLine($"Минимальное значение: {min}"); // Вывод минимального значения
            System.Console.WriteLine($"Максимальное значение: {max}"); // Вывод максимального значения
            System.Console.WriteLine($"Размах: {differenceMaxMin}"); // Вывод размаха
        }

        // Метод для сортировки массива и вывода исходных и отсортированных данных
        public static void SortAndInput(double[] numbers, int n) {
            Console.WriteLine("Исходные данные: "); // Вывод заголовка для исходных данных
            for (int i = 0; i < n; i++)
            {
                Console.Write(numbers[i] + " "); // Вывод элементов массива до сортировки
            }

            Array.Sort(numbers); // Сортировка массива

            Console.WriteLine("\nВариационный ряд:"); // Вывод заголовка для отсортированных данных
            for (int i = 0; i < n; i++)
            {
                Console.Write(numbers[i] + " "); // Вывод элементов массива после сортировки
            }
        }

        // Метод для создания статистического ряда (группировка данных по интервалам)
        public static void CreateStatRyad(double[] numbers) {
            Array.Sort(numbers); // Сортировка массива

            double intervalCount = Math.Round(1 + 3.322 * Math.Log10(numbers.Length)); // Вычисление количества интервалов
            double intervalDiff = (numbers[numbers.Length - 1] - numbers[0]) / intervalCount; // Вычисление ширины интервала
            
            double a = numbers[0]; // Начало первого интервала
            double b = a + intervalDiff; // Конец первого интервала
            int c = 0; // Счетчик элементов в текущем интервале

            foreach (int n in numbers) {
                if (n >= a && n < b) {
                    c++; // Увеличение счетчика, если элемент попадает в текущий интервал
                } else {
                    Console.WriteLine($"Интервал [{a:F2} - {b:F2}]: {c} элементов"); // Вывод информации о текущем интервале

                    // Переход к следующему интервалу
                    while (n >= b) { // Пока число не попадет в новый интервал
                        a += intervalDiff; // Сдвиг начала интервала
                        b += intervalDiff; // Сдвиг конца интервала
                        c = 0; // Обнуление счетчика
                    }
                    c++; // Учет текущего элемента в новом интервале
                }
            }
            // Вывод информации о последнем интервале
            Console.WriteLine($"Интервал [{a:F2} - {b:F2}]: {c} элементов");
        }

        // Метод для вычисления коэффициента асимметрии и эксцесса
        public static void CalculateSkewnessAndKurtosis(double[] numbers)
        {
            int N = numbers.Length; // Количество элементов в массиве
            double mean = numbers.Average(); // Вычисление среднего значения
            double variance = numbers.Sum(x => Math.Pow(x - mean, 2)) / N; // Вычисление дисперсии
            double stdDev = Math.Sqrt(variance); // Вычисление стандартного отклонения

            double skewness = numbers.Sum(x => Math.Pow(x - mean, 3)) / (N * Math.Pow(stdDev, 3)); // Вычисление коэффициента асимметрии
            double kurtosis = numbers.Sum(x => Math.Pow(x - mean, 4)) / (N * Math.Pow(stdDev, 4)) - 3; // Вычисление коэффициента эксцесса

            Console.WriteLine($"Коэффициент асимметрии (Skewness): {skewness:F4}"); // Вывод коэффициента асимметрии
            Console.WriteLine($"Коэффициент эксцесса (Kurtosis): {kurtosis:F4}"); // Вывод коэффициента эксцесса
        }


        // Метод для вычисления математического ожидания, дисперсии и среднеквадратичного отклонения
        public static (double mean, double variance, double stdDev) CalculateStatistics(double[] numbers)
        {
            int n = numbers.Length; // Количество элементов в выборке

            // Вычисление математического ожидания (среднего значения)
            double mean = numbers.Average();

            // Вычисление дисперсии
            double variance = numbers.Sum(x => Math.Pow(x - mean, 2)) / n;

            // Вычисление среднеквадратичного отклонения
            double stdDev = Math.Sqrt(variance);

            // Возврат результатов в виде кортежа
            return (mean, variance, stdDev);
        }

        // Метод для построения boxplot (ящик с усами)
        public static void BuildBoxPlot(double[] numbers, string fileName)
        {
            // Создание boxplot
            var chart = Chart2D.Chart.BoxPlot<double, double, string>(
                numbers
            );

            // Сохранение графика в файл
            chart.SaveHtml(fileName);
            // chart.Show(); // Раскомментировать для отображения графика
        }

        // Метод для построения гистограммы
        //график, визуализирующий распределение данных по интервалам.
        public static void BuildHistogram(double[] numbers, string fileName)
        {
            // Array.Sort(numbers); // Сортировка массива

            // // Вычисление кумулятивных значений
            // double[] cumulative = Enumerable.Range(1, numbers.Length)
            //     .Select(i => (double)i / numbers.Length)
            //     .ToArray();

            // // Создание данных для ступенчатой функции
            // var stepX = new List<double>(); // X-координаты для ступенек
            // var stepY = new List<double>(); // Y-координаты для ступенек

            // for (int i = 0; i < numbers.Length; i++)
            // {
            //     // Добавляем начало ступеньки
            //     stepX.Add(numbers[i]);
            //     stepY.Add(i > 0 ? cumulative[i - 1] : 0); // Значение до текущей точки

            //     // Добавляем конец ступеньки
            //     stepX.Add(numbers[i]);
            //     stepY.Add(cumulative[i]);
            // }

            // // Построение графика ступенчатой функции
            // var chart = Chart2D.Chart.Line<double, double, string>(
            //     stepX.ToArray(), stepY.ToArray()
            // )
            // .WithTitle("Эмпирическая функция распределения (Кумулята)"); // Установка заголовка графика
            // // Создание гистограммы
            var chart = Chart2D.Chart.Histogram<double, double, string>(
                numbers
            )
            .WithTitle("Гистограмма"); // Установка заголовка графика

            // Сохранение графика в файл
            chart.SaveHtml(fileName);
            // chart.Show(); // Раскомментировать для отображения графика
        }

        // Метод для построения полигона частот
        //ломаная линия, соединяющая точки, соответствующие частотам или плотностям.
        public static void BuildPolygon(double[] numbers, string fileName)
        {
            Array.Sort(numbers); // Сортировка массива

            // Вычисление оптимального количества интервалов
            int binCount = (int)Math.Round(1 + 3.322 * Math.Log10(numbers.Length));
            double min = numbers.Min(), max = numbers.Max(); // Нахождение минимального и максимального значений
            double binWidth = (max - min) / binCount; // Вычисление ширины интервала

            // Вычисление средних значений интервалов
            double[] binCenters = Enumerable.Range(0, binCount)
                .Select(i => min + binWidth * (i + 0.5))
                .ToArray();

            double[] frequencies = new double[binCount]; // Массив для хранения частот

            double a = min; // Начало первого интервала
            double b = a + binWidth; // Конец первого интервала
            int binIndex = 0; // Индекс текущего интервала

            // Подсчет частот
            foreach (var num in numbers)
            {
                // Переход к следующему интервалу, если число выходит за пределы текущего
                while (num >= b && binIndex < binCount - 1)
                {
                    a = b;
                    b += binWidth;
                    binIndex++;
                }

                frequencies[binIndex]++; // Увеличение частоты текущего интервала
            }

            // Построение полигона частот
            var polygonChart = Chart2D.Chart.Line<double, double, string>(
                binCenters, frequencies
            )
            .WithTitle("Полигон частот"); // Установка заголовка графика

            polygonChart.SaveHtml(fileName); // Сохранение графика в файл
        }

        // Вспомогательная функция для вычисления значения функции f(x) = sin(x) / x
        private static double f (double x)
        {
            if (x == 0)
            {
                return 1; // Возвращаем 1 при x = 0, чтобы избежать деления на ноль
            }

            return Math.Sin (x) / x; // Возвращаем значение функции
        }

        // Метод для построения эмпирической функции распределения (кумуляты)
        public static void BuildEmpFuncRasp(double[] numbers, string fileName)
        {
            Array.Sort(numbers); // Сортировка массива

            // Вычисление кумулятивных значений
            double[] cumulative = Enumerable.Range(1, numbers.Length)
                .Select(i => (double)i / numbers.Length)
                .ToArray();

            // Создание данных для ступенчатой функции
            var stepX = new List<double>(); // X-координаты для ступенек
            var stepY = new List<double>(); // Y-координаты для ступенек

            for (int i = 0; i < numbers.Length; i++)
            {
                // Добавляем начало ступеньки
                stepX.Add(numbers[i]);
                stepY.Add(i > 0 ? cumulative[i - 1] : 0); // Значение до текущей точки

                // Добавляем конец ступеньки
                stepX.Add(numbers[i]);
                stepY.Add(cumulative[i]);
            }

            // Построение графика ступенчатой функции
            var empiricalChart = Chart2D.Chart.Line<double, double, string>(
                stepX.ToArray(), stepY.ToArray()
            )
            .WithTitle("Эмпирическая функция распределения"); // Установка заголовка графика

            empiricalChart.SaveHtml(fileName); // Сохранение графика в файл
        }

        public static void BuildOgiva(double[] numbers, string fileName)
        {
            Array.Sort(numbers); // Сортировка массива

            // Вычисление кумулятивных значений
            double[] cumulative = Enumerable.Range(1, numbers.Length)
                .Select(i => (double)i / numbers.Length)
                .ToArray();

            // Создание данных для ступенчатой функции
            var stepX = new List<double>(); // X-координаты для ступенек
            var stepY = new List<double>(); // Y-координаты для ступенек

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i-1] != numbers[i])
                {
                    // Добавляем начало ступеньки
                    stepX.Add(numbers[i-1]);
                    stepY.Add((i-1) > 0 ? cumulative[numbers.Length-i] : 1); // Значение до текущей точки

                    // Добавляем конец ступеньки
                    // stepX.Add(numbers[i]);
                    // stepY.Add(cumulative[i]);
                }
            }
            stepX.Add(numbers[numbers.Length-1]);
            stepY.Add(cumulative[1]); // Значение до текущей точки
            // int j = 0;
            // stepX.Add(numbers[0]);
            // stepY.Add(1); // Значение до текущей точки
            // for (int i = numbers.Length-1; i > 1; i--)
            // {
            //     if (numbers[i-1] != numbers[i])
            //     {
            //         // Добавляем начало ступеньки
            //         stepX.Add(numbers[j]);
            //         stepY.Add(cumulative[i]); // Значение до текущей точки
                    
            //         // Добавляем конец ступеньки
            //         // stepX.Add(numbers[i]);
            //         // stepY.Add(cumulative[i]);
            //     }
            //     j++;
            // }
            // stepX.Add(numbers[numbers.Length-1]);
            // stepY.Add(cumulative[0]); // Значение до текущей точки


            // Построение графика ступенчатой функции
            var empiricalChart = Chart2D.Chart.Line<double, double, string>(
                stepX.ToArray(), stepY.ToArray()
            )
            .WithTitle("Огива"); // Установка заголовка графика

            empiricalChart.SaveHtml(fileName); // Сохранение графика в файл
        }

        public static void BuildCumulates(double[] numbers, string fileName)
        {
            Array.Sort(numbers); // Сортировка массива

            // Вычисление кумулятивных значений
            double[] cumulative = Enumerable.Range(1, numbers.Length)
                .Select(i => (double)i / numbers.Length)
                .ToArray();

            // Создание данных для ступенчатой функции
            var stepX = new List<double>(); // X-координаты для ступенек
            var stepY = new List<double>(); // Y-координаты для ступенек

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i-1] != numbers[i])
                {
                    // Добавляем начало ступеньки
                    stepX.Add(numbers[i-1]);
                    stepY.Add((i-1) > 0 ? cumulative[i-1] : 0); // Значение до текущей точки

                    // Добавляем конец ступеньки
                    // stepX.Add(numbers[i]);
                    // stepY.Add(cumulative[i]);
                }
            }
            stepX.Add(numbers[numbers.Length-1]);
            stepY.Add(1); // Значение до текущей точки

            // Построение графика ступенчатой функции
            var empiricalChart = Chart2D.Chart.Line<double, double, string>(
                stepX.ToArray(), stepY.ToArray()
            )
            .WithTitle("Кумулята"); // Установка заголовка графика

            empiricalChart.SaveHtml(fileName); // Сохранение графика в файл
        }
    }
}