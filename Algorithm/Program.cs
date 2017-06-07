using System;
using Algorithm;
using System.Collections.Generic;

public static class Program
{
    public static List<Worker> workers = new List<Worker>();
    public static List<Task> tasks = new List<Task>();

    public static void Main()
    {
        Input io = new Input();
        io.ParseWorkerAndTaskAndCondition(ref workers, ref tasks);

        GA ga = new GA();

        //ga.FitnessFunction = new GAFunction(theActualFunction);

        ga.Go();

        Output output = new Output(ga.thisGeneration[0] as Genome);
        //double fitness = 0.0f;
        //ga.GetBest(ref fitness);
        //Console.WriteLine("Best: {0}", fitness);
        //io.PrintSchedule(ga);
    
        
        Console.ReadLine();
    }
}