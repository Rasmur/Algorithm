using System;
using Algorithm;
using System.Collections.Generic;


public class Test
{
    List<Worker> workers = new List<Worker>();
    List<Task> tasks = new List<Task>();

    //  optimal solution for this is (0.5,0.5)
    public static double theActualFunction(double[] values)
    {
        if (values.GetLength(0) != 2)
            throw new ArgumentOutOfRangeException("should only have 2 args");

        double x = values[0];
        double y = values[1];
        double n = 9;  //  should be an int, but I don't want to waste time casting.

        double f1 = Math.Pow(15 * x * y * (1 - x) * (1 - y) * Math.Sin(n * Math.PI * x) * Math.Sin(n * Math.PI * y), 2);
        return f1;
    }

    public static void Main()
    {
        GA ga = new GA();

        ga.FitnessFunction = new GAFunction(theActualFunction);

        ga.Go();

        double[] values;
        double fitness;
        ga.GetBest(out values, out fitness);
        Console.WriteLine("Best ({0}):", fitness);
        for (int i = 0; i < values.Length; i++)
            Console.WriteLine("{0} ", values[i]);
        
        Console.ReadLine();
    }
}