namespace Lab3.Solutions
{
    class Sol_2
    {
        public static void ConclusionA()
        {
            System.Console.WriteLine(@"Для выборки Akit лучше соответствует распределение Пуассона, 
            поскольку гипотеза о равномерном распределении отвергается, 
            а гипотеза о распределении Пуассона — нет");
        }
        public static void ConclusionB()
        {
            System.Console.WriteLine(@"Для выборки Bkit лучше соответствует нормальное распределение, 
            поскольку только эта гипотеза не отвергается по критерию Колмогорова-Смирнова");
        }
    }
}