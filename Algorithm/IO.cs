using System;
using System.Collections.Generic;
using System.IO;

namespace Algorithm
{
    public class IO
    {
        int buffCost;
        int[] buffSchedule;


        char[] separators = { ';', ' ' };
        void ParseWorkerAndTask(ref List<Worker> workers, ref List<Task> tasks)
        {
            StreamReader sr = new StreamReader("DataForAlgorithm.txt");
            int counterOfWorkerSerialNumber = 0;
            int counterOfTaskSerialNumber = 0;
            string line;



            while ((line = sr.ReadLine()) != null)
            {
                string[] buff = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (buff.Length > 0)
                {
                    if (buff.Length == 4)
                    {
                        Worker newWorker = new Worker();


                        IO.TryParse();
                        workers.Add(newWorker);
                        ///делаем парс таска
                        counterOfWorkerSerialNumber++;
                        newWorker.serialNumber = counterOfTaskSerialNumber;
                    }
                    if (buff.Length == 2)
                    {
                        Task newTask = new Task();


                        IO.TryParse(newTask, buff);
                        tasks.Add(newTask);
                        ///делаем парс воркера
                        counterOfTaskSerialNumber++;
                        newTask.serialNumber = counterOfTaskSerialNumber;
                    }
                }
            }
        }


        void PrintSchedule()
        {

        }

        public static bool TryParseForWorker(ref Worker newWorker, string[] buff)
        {




            return false;
        }

        public static string TryParseForTask(ref Task newTask, string[] buff)
        {
            int i;

            newTask.name = buff[0];
            if (Boolean.TryParse(buff[1], out newTask.importance)) i = 1;
            if (Int32.TryParse(buff[2], out newTask.deadline)) i = 2; ;
            if (Int32.TryParse(buff[3], out newTask.duration)) i = 3;

            if (i>0) return "Вы неверно ввели" + buff[i] + "в задании" + newTask.name;
        }


    }
}
