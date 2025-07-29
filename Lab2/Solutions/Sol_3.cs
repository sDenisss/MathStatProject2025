using Plotly.NET;

namespace Lab1.Solutions
{
    class Sol_3
    {
        public static void BuildPolygonEmp(double[] numbers, string fileName)
        {
            Array.Sort(numbers); // Сортировка массива

            // Вычисление оптимального количества интервалов
            int binCount = (int)Math.Round(1 + 3.322 * Math.Log10(numbers.Length));
            double min = numbers.Min(), max = numbers.Max(); // Нахождение минимального и максимального значений
            double binWidth = (max - min) / binCount; // Вычисление ширины интервала

            // Вычисление средних значений интервалов
            double[] binCenters = StatisticalAnalysis.MidsIntervals.ToArray();

            for (int i = 0; i < binCount; i++)
            {
                binCenters[i] = StatisticalAnalysis.MidsIntervals[i] - binWidth/2;
            }

            double[] frequencies = new double[binCount]; // Массив для хранения частот
            double sumFreq = 0;

            for (int i = 0; i < binCount; i++)
            {
                sumFreq += StatisticalAnalysis.OtnosFreq[i];
                frequencies[i] = sumFreq;
            }

            // Построение полигона частот
            var polygonChart = Chart2D.Chart.Line<double, double, string>(
                binCenters, frequencies
            )
            .WithTitle("Эмпирическое распределение"); // Установка заголовка графика

            polygonChart.SaveHtml(fileName); // Сохранение графика в файл
        }

        public static void BuildPolygonTeor(double[] numbers, string fileName)
        {
            Array.Sort(numbers); // Сортировка массива

            // Вычисление оптимального количества интервалов
            int binCount = (int)Math.Round(1 + 3.322 * Math.Log10(numbers.Length));
            double min = numbers.Min(), max = numbers.Max(); // Нахождение минимального и максимального значений
            double binWidth = (max - min) / binCount; // Вычисление ширины интервала

            // Вычисление средних значений интервалов
            double[] binCenters = StatisticalAnalysis.MidsIntervals.ToArray();

            for (int i = 0; i < binCount; i++)
            {
                binCenters[i] = StatisticalAnalysis.MidsIntervals[i] + binWidth/2;
            }

            double[] frequencies = new double[binCount]; // Массив для хранения частот

            for (int i = 0; i < binCount; i++)
            {
                frequencies[i] = StatisticalAnalysis.TeorFreq[i];
                // System.Console.WriteLine(frequencies[i]);
            }

            // Построение полигона частот
            var polygonChart = Chart2D.Chart.Line<double, double, string>(
                binCenters, frequencies
            )
            .WithTitle("Теоретическое распределение"); // Установка заголовка графика

            polygonChart.SaveHtml(fileName); // Сохранение графика в файл
        }
    }
}