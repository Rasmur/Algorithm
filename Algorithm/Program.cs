using System;
using Algorithm;
using System.Collections.Generic;

public static class Program
{
    public static List<Worker> workers = new List<Worker>();
    public static List<Task> tasks = new List<Task>();
    public static List<Task> startTasks = new List<Task>(tasks);

    public static void Main()
    {
        Input io = new Input();
        io.ParseWorkerAndTaskAndCondition(ref workers, ref tasks);

        startTasks.AddRange(tasks);

        GA ga = new GA();

        //ga.FitnessFunction = new GAFunction(theActualFunction);

        ga.Go();

        Fitness fit = new Fitness((ga.thisGeneration[0] as Genome));

        if ((ga.thisGeneration[0] as Genome).fitness != 0)
        {
            Output output = new Output(ga.thisGeneration[0] as Genome);
        }
        else
        {
            Console.WriteLine("Не нашлось такого расписания, которое бы удовлетворяло всем условиям");
        }
        //double fitness = 0.0f;
        //ga.GetBest(ref fitness);
        //Console.WriteLine("Best: {0}", fitness);
        //io.PrintSchedule(ga);
    
        
        Console.ReadLine();
    }
}