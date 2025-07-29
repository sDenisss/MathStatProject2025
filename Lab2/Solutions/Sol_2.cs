namespace Lab1.Solutions
{
    class Sol_2
    {
        public static double LognormalPDF(double x, double mu, double sigma)
        {
            if (x <= 0) return 0;
            double exponent = -Math.Pow(Math.Log(x) - mu, 2) / (2 * sigma * sigma);
            return (1 / (x * sigma * Math.Sqrt(2 * Math.PI))) * Math.Exp(exponent);
        }

        public static double NormalCDF(double x, double mu, double sigma)
        {
            return 0.5 * (1 + Erf((x - mu) / (sigma * Math.Sqrt(2))));
        }

        static double Erf(double x)
        {
            // Аппроксимация функции ошибок
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            int sign = x < 0 ? -1 : 1;
            x = Math.Abs(x);

            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - ((((a5 * t + a4) * t + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
            return sign * y;
        }

        public static double CalculateMean(double[] sample)
        {
            if (sample == null || sample.Length == 0)
                throw new ArgumentException("Выборка пуста или null");

            double sum = 0;
            foreach (var x in sample)
            {
                sum += x;
            }

            return sum / sample.Length;
        }
    }
}