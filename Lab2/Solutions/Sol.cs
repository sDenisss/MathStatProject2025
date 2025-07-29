using Plotly.NET;
using System.Linq;

namespace Lab1.Solutions
{
    class Sol 
    {
        // Метод для нахождения минимального, максимального значения и размаха массива
        public static void MaxMinAndDifference(double[] numbers) {
            int n = numbers.Length;
            double min = numbers[0]; // Инициализация минимального значения первым элементом массива
            double max = numbers[n-1]; // Инициализация максимального значения последним элементом массива
            double differenceMaxMin = max - min; // Вычисление размаха (разницы между max и min)

            System.Console.WriteLine("Количество элементов " + n); // Вывод количества элементов массива

            System.Console.WriteLine($"Минимальное значение: {min}"); // Вывод минимального значения
            System.Console.WriteLine($"Максимальное значение: {max}"); // Вывод максимального значения
            System.Console.WriteLine($"Размах: {differenceMaxMin}"); // Вывод размаха
        }

        // Метод для сортировки массива и вывода исходных и отсортированных данных
        public static void SortAndInput(double[] numbers) {
            int n = numbers.Length;
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
            Console.WriteLine();
        }

        // Метод для создания статистического ряда (группировка данных по интервалам)
        public static void CreateStatRyad(double[] numbers) 
        {
            Array.Sort(numbers); // Сортировка массива

            // Вычисление количества интервалов по формуле Стерджеса
            int intervalCount = (int)Math.Round(1 + 3.322 * Math.Log10(numbers.Length));
            double intervalDiff = (numbers[numbers.Length - 1] - numbers[0]) / intervalCount;
            StatisticalAnalysis.Step = intervalDiff;

            Console.WriteLine($"Количество интервалов: {intervalCount}");
            Console.WriteLine($"Ширина интервала: {intervalDiff:F2}");

            double a = numbers[0] - (intervalDiff / 2); // Начало первого интервала
            double b = a + intervalDiff; // Конец первого интервала
            int totalCount = numbers.Length;
            int cumulativeFrequency = 0;

            int i = 0;
            while (i < numbers.Length)
            {
                int count = 0;
                double currentA = a;
                double currentB = b;

                // Подсчет элементов в текущем интервале
                while (i < numbers.Length && numbers[i] < currentB)
                {
                    count++;
                    i++;
                }

                // Расчет характеристик
                double midpoint = (currentA + currentB) / 2;
                StatisticalAnalysis.MidsIntervals.Add(midpoint);
                
                double relativeFrequency = (double)count / totalCount;
                StatisticalAnalysis.OtnosFreq.Add(relativeFrequency);
                cumulativeFrequency += count;

                // Вывод информации об интервале
                Console.WriteLine($"[{currentA:F2} - {currentB:F2}): " +
                                $"Середина: {midpoint:F2} | " +
                                $"Частота: {count} | " +
                                $"Отн. частота: {relativeFrequency:F4} | " +
                                $"Накопл. частота: {cumulativeFrequency}");

                // Переход к следующему интервалу
                a = currentB;
                b = currentB + intervalDiff;
            }
        }

        public static void CalculateStatisticalTable(double[] data)
        {
            // Группировка данных
            var groupedData = data.GroupBy(x => x)
                                .Select(g => new { Value = g.Key, Count = g.Count() })
                                .OrderBy(x => x.Value)
                                .ToList();

            int totalCount = data.Length;
            double b = StatisticalAnalysis.B; // ложный нуль (варианта с максимальной частотой)
            double h = StatisticalAnalysis.Step;  // шаг

            // Форматирование столбцов (ширина каждого столбца)
            const int col1 = 8;  // xi*
            const int col2 = 6;  // ni
            const int col3 = 8;  // Ui
            const int col4 = 10; // niUi
            const int col5 = 12; // niUi^2
            const int col6 = 12; // niUi^3
            const int col7 = 12; // niUi^4
            const int col8 = 15; // ni(Ui+1)^4

            // Шапка таблицы
            Console.WriteLine(new string('-', col1+col2+col3+col4+col5+col6+col7+col8 + 7));
            Console.WriteLine(
                $"| {"xi*".PadLeft(col1)} " +
                $"| {"ni".PadLeft(col2)} " +
                $"| {"Ui".PadLeft(col3)} " +
                $"| {"niUi".PadLeft(col4)} " +
                $"| {"niUi^2".PadLeft(col5)} " +
                $"| {"niUi^3".PadLeft(col6)} " +
                $"| {"niUi^4".PadLeft(col7)} " +
                $"| {"ni(Ui+1)^4".PadLeft(col8)} |");
            Console.WriteLine(new string('-', col1+col2+col3+col4+col5+col6+col7+col8 + 7));

            double sumNi = 0;
            double sumNiUi = 0;
            double sumNiUi2 = 0;
            double sumNiUi3 = 0;
            double sumNiUi4 = 0;
            double sumNiUiPlus1_4 = 0;

            foreach (var item in groupedData)
            {
                double xi = item.Value;
                int ni = item.Count;
                double Ui = (xi - b) / h;
                double niUi = ni * Ui;
                double niUi2 = ni * Math.Pow(Ui, 2);
                double niUi3 = ni * Math.Pow(Ui, 3);
                double niUi4 = ni * Math.Pow(Ui, 4);
                double niUiPlus1_4 = ni * Math.Pow(Ui + 1, 4);

                sumNi += ni;
                sumNiUi += niUi;
                sumNiUi2 += niUi2;
                sumNiUi3 += niUi3;
                sumNiUi4 += niUi4;
                sumNiUiPlus1_4 += niUiPlus1_4;

                Console.WriteLine(
                    $"| {xi.ToString("F2").PadLeft(col1)} " +
                    $"| {ni.ToString().PadLeft(col2)} " +
                    $"| {Ui.ToString("F2").PadLeft(col3)} " +
                    $"| {niUi.ToString("F2").PadLeft(col4)} " +
                    $"| {niUi2.ToString("F2").PadLeft(col5)} " +
                    $"| {niUi3.ToString("F2").PadLeft(col6)} " +
                    $"| {niUi4.ToString("F2").PadLeft(col7)} " +
                    $"| {niUiPlus1_4.ToString("F2").PadLeft(col8)} |");
            }

            // Итоговая строка
            Console.WriteLine(new string('-', col1+col2+col3+col4+col5+col6+col7+col8 + 7));
            Console.WriteLine(
                $"| {"Итог".PadLeft(col1)} " +
                $"| {sumNi.ToString().PadLeft(col2)} " +
                $"| {"-".PadLeft(col3)} " +
                $"| {sumNiUi.ToString("F2").PadLeft(col4)} " +
                $"| {sumNiUi2.ToString("F2").PadLeft(col5)} " +
                $"| {sumNiUi3.ToString("F2").PadLeft(col6)} " +
                $"| {sumNiUi4.ToString("F2").PadLeft(col7)} " +
                $"| {sumNiUiPlus1_4.ToString("F2").PadLeft(col8)} |");
            Console.WriteLine(new string('-', col1+col2+col3+col4+col5+col6+col7+col8 + 7));

            // Проверка
            double checkValue = sumNi + 4*sumNiUi + 6*sumNiUi2 + 4*sumNiUi3 + sumNiUi4;
            Console.WriteLine($"\nПроверка: {sumNi} + 4*{sumNiUi:F2} + 6*{sumNiUi2:F2} + 4*{sumNiUi3:F2} + {sumNiUi4:F2} = {checkValue:F2}");
            Console.WriteLine($"Сумма ni(Ui+1)^4: {sumNiUiPlus1_4:F2}");

            // Условные начальные моменты
            double v1 = sumNiUi / totalCount;
            double v2 = sumNiUi2 / totalCount;
            double v3 = sumNiUi3 / totalCount;
            double v4 = sumNiUi4 / totalCount;

            Console.WriteLine("\nУсловные начальные моменты:");
            Console.WriteLine($"v1 = {v1,10:F2}");
            Console.WriteLine($"v2 = {v2,10:F2}");
            Console.WriteLine($"v3 = {v3,10:F2}");
            Console.WriteLine($"v4 = {v4,10:F2}");
            StatisticalAnalysis.V1 = v1;
            StatisticalAnalysis.V2 = v2;
            StatisticalAnalysis.V3 = v3;
            StatisticalAnalysis.V4 = v4;
        }

        // Метод для вычисления математического ожидания, дисперсии и среднеквадратичного отклонения
        public static void CalculateStatistics()
        {
            Console.WriteLine($"V1 = {StatisticalAnalysis.V1}");
            Console.WriteLine($"Step = {StatisticalAnalysis.Step}");
            Console.WriteLine($"B = {StatisticalAnalysis.B}");
            // Вычисление математического ожидания (среднего значения)
            double mean = StatisticalAnalysis.V1*StatisticalAnalysis.Step+StatisticalAnalysis.B;

            // Вычисление дисперсии
            double variance = (StatisticalAnalysis.V2-Math.Pow(StatisticalAnalysis.V1, 2))*Math.Pow(StatisticalAnalysis.Step, 2);

            // Вычисление среднеквадратичного отклонения
            double stdDev = Math.Sqrt(variance);

            System.Console.WriteLine($"математическое ожидание: {mean}");
            // System.Console.WriteLine(StatisticalAnalysis.Step + "-------------------------------");
            System.Console.WriteLine($"дисперсию: {variance}");
            System.Console.WriteLine($"Среднее квадратическое отклонение: {stdDev}");
            StatisticalAnalysis.MathWait = mean;
            StatisticalAnalysis.StdDev = stdDev;
        }
        public static void CalculateCentralMoments()
        {
            double v1 = StatisticalAnalysis.V1;
            double v2 = StatisticalAnalysis.V2;
            double v3 = StatisticalAnalysis.V3;
            double v4 = StatisticalAnalysis.V4;
            double h = StatisticalAnalysis.Step;
            // Центральный момент 3-го порядка
            double m3 = (v3 - 3 * v1 * v2 + 2 * Math.Pow(v1, 3)) * Math.Pow(h, 3);
            
            // Центральный момент 4-го порядка
            double m4 = (v4 - 4 * v1 * v3 + 6 * Math.Pow(v1, 2) * v2 - 3 * Math.Pow(v1, 4)) * Math.Pow(h, 4);

            System.Console.WriteLine($"центральный момент 3-го порядка: {m3}");
            System.Console.WriteLine($"центральный момент 4-го порядка: {m4}");
            StatisticalAnalysis.M3 = m3;
            StatisticalAnalysis.M4 = m4;

        }


        // Метод для вычисления коэффициента асимметрии и эксцесса
        public static void CalculateSkewnessAndKurtosis()
        {
            double skewness = StatisticalAnalysis.M3/Math.Pow(StatisticalAnalysis.StdDev, 3); // Вычисление коэффициента асимметрии
            double kurtosis = (StatisticalAnalysis.M4/Math.Pow(StatisticalAnalysis.StdDev, 4))-3; // Вычисление коэффициента эксцесса

            StatisticalAnalysis.CoefAsim = skewness;
            StatisticalAnalysis.CoefExcess = kurtosis;

            Console.WriteLine($"Коэффициент асимметрии (Skewness): {skewness:F4}"); // Вывод коэффициента асимметрии
            Console.WriteLine($"Коэффициент эксцесса (Kurtosis): {kurtosis:F4}"); // Вывод коэффициента эксцесса
        }


        public static void BuildTable(double[] data)
        {
            // Группировка данных с определением интервалов
            var (intervals, frequencies) = GroupData(data);

            // Находим вариант с максимальной частотой (ложный нуль)
            int maxFreqIndex = Array.IndexOf(frequencies, frequencies.Max());
            double b = intervals[maxFreqIndex]; // ложный нуль
            double h = intervals[1] - intervals[0]; // шаг

            Console.WriteLine("Метод сумм для вычисления моментов распределения");
            Console.WriteLine($"Ложный нуль (b) = {b}, шаг (h) = {h}\n");

            // Строим таблицу
            PrintTable(intervals, frequencies, b, h);
        }

        private static (double[] intervals, int[] frequencies) GroupData(double[] data)
        {
            // Определяем интервалы группировки (можно адаптировать под ваши данные)
            double min = data.Min();
            double max = data.Max();
            int intervalCount = 7; // как в примере
            double step = (max - min) / intervalCount;

            // Создаем интервалы
            double[] intervals = new double[intervalCount];
            for (int i = 0; i < intervalCount; i++)
            {
                intervals[i] = min + i * step + step / 2; // середины интервалов
            }

            // Считаем частоты
            int[] frequencies = new int[intervalCount];
            foreach (var value in data)
            {
                int index = (int)Math.Floor((value - min) / step);
                if (index >= intervalCount) index = intervalCount - 1;
                frequencies[index]++;
            }

            return (intervals, frequencies);
        }

        private static void PrintTable(double[] intervals, int[] frequencies, double b, double h)
        {
            // Заголовок таблицы
            Console.WriteLine("|   xi   |  ni  |  b1  |  b2  |  b3  |  b4  |  a1  |  a2  |  a3  |  a4  |");
            Console.WriteLine(new string('-', 74));

            int n = intervals.Length;
            int[] b1 = new int[n], b2 = new int[n], b3 = new int[n], b4 = new int[n];
            int[] a1 = new int[n], a2 = new int[n], a3 = new int[n], a4 = new int[n];

            // Находим индекс ложного нуля
            int zeroIndex = Array.IndexOf(intervals, b);

            // Заполняем b-столбцы (выше ложного нуля)
            for (int i = 0; i < zeroIndex; i++)
            {
                b1[i] = (i == 0) ? frequencies[i] : b1[i - 1] + frequencies[i];
                b2[i] = (i <= 1) ? frequencies[i] : b2[i - 1] + frequencies[i];
                b3[i] = (i <= 2) ? frequencies[i] : b3[i - 1] + frequencies[i];
                b4[i] = (i <= 3) ? frequencies[i] : b4[i - 1] + frequencies[i];
            }

            // Заполняем a-столбцы (ниже ложного нуля)
            for (int i = n - 1; i > zeroIndex; i--)
            {
                a1[i] = (i == n - 1) ? frequencies[i] : a1[i + 1] + frequencies[i];
                a2[i] = (i >= n - 2) ? frequencies[i] : a2[i + 1] + frequencies[i];
                a3[i] = (i >= n - 3) ? frequencies[i] : a3[i + 1] + frequencies[i];
                a4[i] = (i >= n - 4) ? frequencies[i] : a4[i + 1] + frequencies[i];
            }

            // Вывод таблицы
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"| {intervals[i],6:F2} | {frequencies[i],4} | {b1[i],4} | {b2[i],4} | {b3[i],4} | {b4[i],4} | {a1[i],4} | {a2[i],4} | {a3[i],4} | {a4[i],4} |");
            }

            // Итоговые суммы
            int totalB1 = b1.Take(zeroIndex).Sum();
            int totalA1 = a1.Skip(zeroIndex + 1).Sum();
            Console.WriteLine(new string('-', 74));
            Console.WriteLine($"| Итого  | {frequencies.Sum(),4} | {totalB1,4} | {b2.Take(zeroIndex).Sum(),4} | {b3.Take(zeroIndex).Sum(),4} | {b4.Take(zeroIndex).Sum(),4} | {totalA1,4} | {a2.Skip(zeroIndex + 1).Sum(),4} | {a3.Skip(zeroIndex + 1).Sum(),4} | {a4.Skip(zeroIndex + 1).Sum(),4} |");

            StatisticalAnalysis.A1 = totalA1;
            StatisticalAnalysis.A2 = a2.Skip(zeroIndex + 1).Sum();
            StatisticalAnalysis.A3 = a3.Skip(zeroIndex + 1).Sum();
            StatisticalAnalysis.A4 = a4.Skip(zeroIndex + 1).Sum();
            StatisticalAnalysis.B1 = totalB1;
            StatisticalAnalysis.B2 = b2.Skip(zeroIndex + 1).Sum();
            StatisticalAnalysis.B3 = b3.Skip(zeroIndex + 1).Sum();
            StatisticalAnalysis.B4 = b4.Skip(zeroIndex + 1).Sum();
        }

        public static void SolveSAndD()
        {
            // Вычисляем S значения
            StatisticalAnalysis.S1 = StatisticalAnalysis.A1 + StatisticalAnalysis.B1;
            Console.WriteLine($"S1 = {StatisticalAnalysis.S1}");
            
            StatisticalAnalysis.S2 = StatisticalAnalysis.A2 + StatisticalAnalysis.B2;
            Console.WriteLine($"S2 = {StatisticalAnalysis.S2}");
            
            StatisticalAnalysis.S3 = StatisticalAnalysis.A3 + StatisticalAnalysis.B3;
            Console.WriteLine($"S3 = {StatisticalAnalysis.S3}");
            
            StatisticalAnalysis.S4 = StatisticalAnalysis.A4 + StatisticalAnalysis.B4;
            Console.WriteLine($"S4 = {StatisticalAnalysis.S4}");

            // Вычисляем D значения
            StatisticalAnalysis.D1 = StatisticalAnalysis.A1 - StatisticalAnalysis.B1;
            Console.WriteLine($"D1 = {StatisticalAnalysis.D1}");
            
            StatisticalAnalysis.D2 = StatisticalAnalysis.A2 - StatisticalAnalysis.B2;
            Console.WriteLine($"D2 = {StatisticalAnalysis.D2}");
            
            StatisticalAnalysis.D3 = StatisticalAnalysis.A3 - StatisticalAnalysis.B3;
            Console.WriteLine($"D3 = {StatisticalAnalysis.D3}");
        }

        public static void SolveStartMoments(double[] nums)
        {
            int n = nums.Length;
            StatisticalAnalysis.V1 = StatisticalAnalysis.D1 / n;
            StatisticalAnalysis.V2 = (StatisticalAnalysis.S1 + 2*StatisticalAnalysis.S2)/n;
            StatisticalAnalysis.V3 = (StatisticalAnalysis.D1+6*StatisticalAnalysis.D2+6*StatisticalAnalysis.D3) / n;
            StatisticalAnalysis.V4 = (StatisticalAnalysis.S1+14*StatisticalAnalysis.S2+36*StatisticalAnalysis.S3+24*StatisticalAnalysis.S4) / n;
            
             Console.WriteLine($"условный начальный момент 1-го порядка = {StatisticalAnalysis.V1}");
             Console.WriteLine($"условный начальный момент 2-го порядка = {StatisticalAnalysis.V2}");
             Console.WriteLine($"условный начальный момент 3-го порядка = {StatisticalAnalysis.V3}");
             Console.WriteLine($"условный начальный момент 4-го порядка = {StatisticalAnalysis.V4}");
        }
        public static void SolveModaAndMediana(double[] nums)
        {
            if (nums == null || nums.Length == 0)
            {
                Console.WriteLine("Массив пуст или не инициализирован");
                return;
            }

            // Сортировка массива для вычисления медианы
            Array.Sort(nums);

            // Вычисление медианы
            double mediana;
            int n = nums.Length;
            if (n % 2 == 1)
            {
                mediana = nums[n / 2];
            }
            else
            {
                mediana = (nums[n / 2 - 1] + nums[n / 2]) / 2.0;
            }

            // Вычисление моды
            double moda = 0;
            int maxCount = 0;
            var counts = new Dictionary<double, int>();

            foreach (double num in nums)
            {
                if (counts.ContainsKey(num))
                {
                    counts[num]++;
                }
                else
                {
                    counts[num] = 1;
                }

                if (counts[num] > maxCount)
                {
                    maxCount = counts[num];
                    moda = num;
                }
            }

            // Если все элементы встречаются одинаково часто, моды нет
            if (maxCount == 1)
            {
                Console.WriteLine($"Медиана: {mediana:F2}");
                Console.WriteLine("Мода отсутствует (все элементы уникальны)");
            }
            else
            {
                Console.WriteLine($"Мода: {moda:F2}");
                Console.WriteLine($"Медиана: {mediana:F2}");
            }
            StatisticalAnalysis.Moda = moda;
            StatisticalAnalysis.Mediana = mediana;
        }

    }
}