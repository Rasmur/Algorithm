﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace Algorithm
{
    public class Input
    {
        char[] separators = { ';' };


        public void ParseWorkerAndTaskAndCondition(ref List<Worker> workers, ref List<Task> tasks)
        {
            StreamReader sr = new StreamReader("DataForAlgorithm.txt");
            int counterOfWorkerSerialNumber = 0;
            int counterOfTaskSerialNumber = 0;
            string line;

            ///доходим до начала списка
            while ((line = sr.ReadLine()) != "Ваш список:")
            {
                //sr.ReadLine();
            }

            ///пока не дошли до конца файла
            while ((line = sr.ReadLine()) != null)
            {
                string[] buff = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (buff.Length > 0)
                {
                    //если это задание
                    if (buff.Length == 2)
                    {
                        Worker newWorker = new Worker();

                        string result = Input.TryParseForWorker(ref newWorker, buff, ref counterOfWorkerSerialNumber);
                        if (result == null)
                        {
                            workers.Add(newWorker);
                        }
                        else Console.WriteLine(result);
                    }
                    //если это работник
                    else if (buff.Length == 4)
                    {
                        Task newTask = new Task();

                        string result = Input.TryParseForTask(ref newTask, buff, ref counterOfTaskSerialNumber);
                        if (result == null)
                        {
                            tasks.Add(newTask);
                        }
                        else Console.WriteLine(result);
                    }
                    //если это условие
                    else if (buff.Length == 1)
                    {
                        string result = TryParseForCondition(buff);
                        if (result != null) Console.WriteLine(result);
                    }
                }
            }
            sr.Close();
        }


        /// возвращает null, если получилось распарсить
        /// возвращает ошибку, если не удалось распарсить
        static string TryParseForWorker(ref Worker newWorker, string[] buff, ref int counterOfWorkerSerialNumber)
        {
            int i = -1;
            string[] hours = buff[1].Split(new char[] { ',', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
            //newWorker.schedule = new int[hours.Length];

            if (!Int32.TryParse(buff[0], out newWorker.costPerHour)) i = 0;
            newWorker.schedule = new int[hours.Length];
            ///проверку на массив 
            for (int j = 0; j < hours.Length; j++)
            {
                if (Int32.TryParse(hours[j], out newWorker.schedule[j]) == false) i = 1;
            }

            if (i >= 0) return ("Вы неверно ввели " + buff[i] + " у " + newWorker.serialNumber + "-го работника");
            else
            {
                counterOfWorkerSerialNumber++;
                newWorker.serialNumber = counterOfWorkerSerialNumber;
                return null;
            }
        }


        /// возвращает null, если получилось распарсить
        /// возвращает ошибку, если не удалось распарсить
        static string TryParseForTask(ref Task newTask, string[] buff, ref int counterOfTaskSerialNumber)
        {
            int i = 0;

            newTask.name = buff[0];
            if (Boolean.TryParse(buff[1], out newTask.importance) == false) i = 1;
            if (Int32.TryParse(buff[2], out newTask.deadline) == false) i = 2;
            if (Int32.TryParse(buff[3], out newTask.duration) == false) i = 3;

            if (i > 0) return ("Вы неверно ввели " + buff[i] + " в задании" + newTask.name);
            else
            {
                counterOfTaskSerialNumber++;
                newTask.serialNumber = counterOfTaskSerialNumber;
                return null;
            }
        }

        string TryParseForCondition(string[] buff)
        {
            int buffKey = 0;
            int buffValue = 0;
            int buffCondition = 0;
            int counter = -1;

            string[] buffArray = buff[0].Split(new char[] { ',', ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);


            if ((Int32.TryParse(buffArray[0], out buffCondition) && ((buffCondition == 1) || (buffCondition == 2))) == false) counter = 0;
            else if ((Int32.TryParse(buffArray[1], out buffKey) && Program.tasks.Count >= buffKey) == false) counter = 1;
            else if ((Int32.TryParse(buffArray[2], out buffValue) && Program.tasks.Count >= buffValue) == false) counter = 2;

            if (counter >= 0)
            {
                return ("Ошибка в условии " + buffArray[counter]);
            }
            else
            {
                if (buffCondition == 1) Conditions.atTheSameTime.Add(buffKey, buffValue);
                else Conditions.necessity.Add(buffKey, buffValue);
                return null;
            }
        }


        public void PrintSchedule(GA a)
        {
            //берем лучший геном, в отсортированном arraylist  
            Genome g = (Genome)a.thisGeneration[0];
            int leng = g.genes.Length;

            //foreach (var worker in Program.workers)
            //{

            //}


            for (int i = 0; i < leng / 2 - 1; i++)
            {
                Console.WriteLine("Работник № {1} выполняет задание {0}",
                    Program.tasks[g.genes[i] - 1].name, Program.workers[g.genes[i + leng / 2] - 1].serialNumber);
            }
            Console.WriteLine("Общая стоимость: " + g.fitness);
        }
    }
}
