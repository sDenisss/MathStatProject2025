using System;
using System.Linq;
using ScottPlot;

class Boxplot
{
    static void BoxPlotBuilder(double[] numbers)
    {

        // Сортируем данные
        Array.Sort(numbers);

        // Определяем основные статистики
        double q1 = Quartile(numbers, 0.25);
        double q2 = Quartile(numbers, 0.50);
        double q3 = Quartile(numbers, 0.75);
        double iqr = q3 - q1; // Межквартильный размах
        double lowerBound = q1 - 1.5 * iqr;
        double upperBound = q3 + 1.5 * iqr;

        // Определяем выбросы
        var outliers = numbers.Where(x => x < lowerBound || x > upperBound).ToArray();

        Console.WriteLine($"Первый квартиль (Q1): {q1}");
        Console.WriteLine($"Медиана (Q2): {q2}");
        Console.WriteLine($"Третий квартиль (Q3): {q3}");
        Console.WriteLine($"Межквартильный размах (IQR): {iqr}");
        Console.WriteLine($"Выбросы: {string.Join(", ", outliers)}");

        // // Пример данных
        // double[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // // Создаем объект Box
        // var box = new ScottPlot.Box
        // {
        //     // Задаем данные
        //     Values = numbers,
        //     // Опционально: настройте внешний вид
        //     FillColor = System.Drawing.Color.LightBlue,
        //     BoxWidth = 0.5,
        //     WhiskerWidth = 0.2
        // };

// // Создаем график
// var plt = new ScottPlot.Plot();
// plt.Add.Box(box); // Добавляем boxplot
// plt.Title("Boxplot (Ящик с усами)");
// plt.SavePng("boxplot.png", 800, 600); // Сохраняем изображение
        
        Console.WriteLine("График сохранен в boxplot.png");
    }

    // Функция для нахождения квартилей
    static double Quartile(double[] sortedData, double percentile)
    {
        int n = sortedData.Length;
        double index = percentile * (n - 1);
        int lower = (int)index;
        int upper = lower + 1;

        if (upper >= n) return sortedData[lower];
        return sortedData[lower] + (index - lower) * (sortedData[upper] - sortedData[lower]);
    }
}
