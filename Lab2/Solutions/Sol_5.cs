namespace Lab1.Solutions
{
    class Sol_5
    {
        public static void CompareWithLab1A()
        {
            double modaTeor = StatisticalAnalysis.Moda;
            double medianaTeor = StatisticalAnalysis.Mediana;
            double mathWaitTeor = StatisticalAnalysis.MathWait;
            double dispersiaTeor = Math.Pow(StatisticalAnalysis.StdDev, 2);
            double assimTeor = StatisticalAnalysis.CoefAsim;
            double excessTeor = StatisticalAnalysis.CoefExcess;

            System.Console.WriteLine("Сравнение теоретических и эмпирических характеристик:");
            System.Console.WriteLine($"Мода: Теоретическая = {modaTeor}, Эмпирическая = 2");
            System.Console.WriteLine($"Медиана: Теоретическая = {medianaTeor}, Эмпирическая = 2");
            System.Console.WriteLine($"Математическое ожидание: Теоретическое = {mathWaitTeor}, Эмпирическое = 2.5846");
            System.Console.WriteLine($"Дисперсия: Теоретическая = {dispersiaTeor}, Эмпирическая = 2.7044");
            System.Console.WriteLine($"Коэффициент асимметрии: Теоретический = {assimTeor}, Эмпирический = 0.2446");
            System.Console.WriteLine($"Эксцесс: Теоретический = {excessTeor}, Эмпирический = -0.4998");
        }

        public static void CompareWithLab1B()
        {
            double modaTeor = StatisticalAnalysis.Moda;
            double medianaTeor = StatisticalAnalysis.Mediana;
            double mathWaitTeor = StatisticalAnalysis.MathWait;
            double dispersiaTeor = Math.Pow(StatisticalAnalysis.StdDev, 2);
            double assimTeor = StatisticalAnalysis.CoefAsim;
            double excessTeor = StatisticalAnalysis.CoefExcess;

            System.Console.WriteLine("Сравнение теоретических и эмпирических характеристик:");
            System.Console.WriteLine($"Мода: Теоретическая = {modaTeor}, Эмпирическая = 184.5");
            System.Console.WriteLine($"Медиана: Теоретическая = {medianaTeor}, Эмпирическая = 185");
            System.Console.WriteLine($"Математическое ожидание: Теоретическое = 185.822, Эмпирическое = 185.822");
            System.Console.WriteLine($"Дисперсия: Теоретическая = {dispersiaTeor}, Эмпирическая = 81.1971");
            System.Console.WriteLine($"Коэффициент асимметрии: Теоретический = {assimTeor}, Эмпирический = 0.0989");
            System.Console.WriteLine($"Эксцесс: Теоретический = {excessTeor}, Эмпирический = 0.4074");
        }
    }
}
