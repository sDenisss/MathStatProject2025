using Lab1.Solutions;
using Plotly.NET;
using Plotly.NET.LayoutObjects;
using ZedGraph;

public class Program
{
    public static double[] Akit = { 
                2, 4, 2, 4, 3, 3, 3, 2, 0, 6, 1, 2, 3, 2, 2, 4, 3, 3, 5, 1,
                0, 2, 4, 3, 2, 2, 3, 3, 1, 3, 3, 3, 1, 1, 2, 3, 1, 4, 3, 1,
                7, 4, 3, 4, 2, 3, 2, 3, 3, 1, 4, 3, 1, 4, 5, 3, 4, 2, 4, 6,
                3, 6, 4, 1, 3, 2, 4, 1, 3, 1, 0, 0, 4, 6, 4, 7, 4, 1, 3 };
    public static double[] Bkit = {

                65, 71, 67, 73, 68, 68, 72, 68, 67, 70, 78, 74, 79, 65, 72,
                65, 71, 70, 69, 69, 76, 71, 63, 77, 75, 70, 74, 65, 71, 68,
                74, 69, 69, 66, 71, 69, 73, 74, 80, 69, 73, 76, 69, 69, 67,
                67, 74, 68, 74, 60, 70, 66, 70, 68, 64, 75, 78, 71, 70, 69,
                73, 75, 74, 72, 80, 72, 69, 69, 71, 70, 73, 65, 66, 67, 69,
                71, 70, 72, 76, 72, 73, 64, 74, 71, 76, 68, 69, 75, 76, 73,
                74, 78, 66, 75, 72, 69, 68, 63, 70, 70, 78, 76, 73, 73, 67,
                71, 66, 66, 72, 69, 71, 71, 68, 72, 69, 73, 73, 66, 72, 73,
                70, 69, 74, 72, 69, 74, 70, 74, 72, 76, 71, 66, 62, 69, 74,
                76, 74, 69, 64, 75, 71, 76, 68, 68, 78, 71, 71, 68, 67, 74,
                68, 81, 72, 68, 72, 71, 71, 71, 69, 61, 74, 66, 70, 72, 65,
                67, 73, 78, 73, 71, 75, 73, 71, 72, 68, 67, 69, 69, 77, 63,
                71, 74, 67, 68, 69, 74, 69, 67, 74, 66, 74, 74, 69, 75, 70,
                73, 63, 77, 74, 75 };
    public static void Main(string[] args)
    {
        // A.SortAndInput(Akit, Akit.Length);
        B.SortAndInput(Bkit, Bkit.Length);

        // A.MaxMinAndDifference(Akit, Akit.Length);
        B.MaxMinAndDifference(Bkit, Bkit.Length);

        // A.CreateStatRyad(Akit);
        B.CreateStatRyad(Bkit);

        // A.CalculateSkewnessAndKurtosis(Akit);
        B.CalculateSkewnessAndKurtosis(Bkit);

        // System.Console.WriteLine(Akit[39]);

        System.Console.WriteLine(Bkit[99]);
        System.Console.WriteLine(Bkit[100]);

        A.BuildBoxPlot(Akit, "Akit BoxPlot");
        B.BuildBoxPlot(Bkit, "Bkit BoxPlot");

        A.BuildHistogram(Akit, "Akit Histogram");
        B.BuildHistogram(Bkit, "Bkit Histogram");

        
        // A.BuildPolygon(Akit, "Akit Polygon");
        B.BuildPolygon(Bkit, "Bkit Polygon");

        A.BuildEmpFuncRasp(Akit, "Akit EmpFuncRasp");
        B.BuildEmpFuncRasp(Bkit, "Bkit EmpFuncRasp");

    }

    public static double[] getAkit() {
        return Akit;
    }

    public static double[] getBkit() {
        return Bkit;
    }
}