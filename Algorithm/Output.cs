using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class Output
    {
        int totalFitness = 0;
        Genome newGenome = new Genome(415);
        Dictionary<int, int> necessity;
        Dictionary<int, int> atTheSameTime;

        //запоминать начало работы у работника при условии одинакового выполнения
        int beginWorker = 0;
        bool startWorker = false;
        bool endWorker = false;

        string secondCond = "";

        List<string> output = new List<string>();
        List<string> notWork = new List<string>();

        public Output(Genome genome)
        {
            //Console.WriteLine("Минимальный результат: " + genome.fitness);
            for (int i = 0; i < Program.workers.Count; i++)
            {
                Program.workers[i].lastWork.Clear();
                Program.workers[i].lastWork.Add(0);
            }

            necessity = new Dictionary<int, int>(Conditions.necessity);
            atTheSameTime = new Dictionary<int, int>(Conditions.atTheSameTime);

            genome.genes.CopyTo(newGenome.genes, 0);

            genome.fitness = FitnessFunction();

            for (int i = 0; i < output.Count; i++)
            {
                Console.WriteLine(output[i]);
            }

            for (int i = 0; i < notWork.Count; i++)
            {
                Console.WriteLine("Задание " + notWork[i] + " не удалось выполнить");
            }
            Console.WriteLine("\nМинимальный результат: " + genome.fitness);
        }

        public int FitnessFunction(int number = -1)
        {
            Task task;

            if (number != -1)
            {
                CheckCondition(number);

                if (CountFitness(number) == 0)
                {
                    return 0;
                }
            }
            else
            {
                for (int i = 0; i < Program.tasks.Count; i++)
                {
                    if (newGenome.genes[i] != 0)
                    {
                        task = Program.tasks[newGenome.genes[i] - 1];

                        if (CheckCondition(i) == 0 || CountFitness(i) == 0)
                        {
                            return 0;
                        }
                    }
                }
            }
            return totalFitness;
        }

        private int CheckCondition(int number)
        {
            for (int i = 0; i < atTheSameTime.Count; i++)
            {
                if (atTheSameTime.ContainsKey(newGenome.genes[number]))
                {
                    int value = atTheSameTime[newGenome.genes[number]];
                    atTheSameTime.Remove(newGenome.genes[number]);

                    //дан старт запоминанию
                    startWorker = true;

                    if (FitnessFunction(value) == 0)
                    {
                        startWorker = false;
                        return 0;
                    }

                    startWorker = false;
                    endWorker = true;
                }
                if (atTheSameTime.ContainsValue(newGenome.genes[number]))
                {
                    int key = atTheSameTime.FirstOrDefault(x => x.Value == newGenome.genes[number]).Key;

                    atTheSameTime.Remove(key);

                    startWorker = true;

                    if (FitnessFunction(key) == 0)
                    {
                        startWorker = false;
                        return 0;
                    }

                    startWorker = false;
                    endWorker = true;
                }
            }

            for (int i = 0; i < necessity.Count; i++)
            {
                if (necessity.ContainsValue(newGenome.genes[number]))
                {
                    int key = necessity.FirstOrDefault(x => x.Value == newGenome.genes[number]).Key;
                    necessity.Remove(key);
                    
                    secondCond = Program.tasks[key - 2].name;

                    if (FitnessFunction(key) == 0)
                    {
                        //for (int j = 0; j < output.Count; j++)
                        //{
                        //    if (output[j].Contains(secondCond))
                        //    {
                        //        //output.RemoveAt(j);
                                secondCond = "";
                        //    }
                        //}
                        return 0;
                    }
                    secondCond = "";
                }
            }
            return 1;
        }

        private int CountFitness(int number)
        {
            if (newGenome.genes[number] != 0)
            {
                Task task = Program.tasks[newGenome.genes[number] - 1];
                Worker worker = Program.workers[newGenome.genes[number + newGenome.genes.Length / 2] - 1];
                int lastWork = worker.lastWork.Last();

                //чтобы не выходить за рамки дозволенного
                if ((task.duration <= worker.schedule.Length - lastWork) &&
                    worker.schedule[task.duration + lastWork - 1] <= task.deadline)
                {
                    totalFitness += worker.costPerHour * task.duration;

                    //запоминать ли начальную позицию
                    if (startWorker)
                    {
                        beginWorker = worker.schedule[lastWork];
                        startWorker = false;
                    }
                    else if (endWorker)
                    {
                        if (worker.schedule.Contains(beginWorker))
                        {
                            endWorker = false;

                            bool buff = false;

                            for (int i = 0; i < worker.schedule.Length && !buff; i++)
                            {
                                if (worker.schedule[i] == beginWorker && i >= lastWork && (i + task.duration) <= task.deadline)
                                {
                                    buff = true;
                                    lastWork = i;
                                }
                            }

                            if (!buff)
                            {
                                return 0;
                            }
                        }
                    }

                    string newOut = "";

                    newOut += "Работник № " + worker.serialNumber + ":";

                    for (int i = lastWork; i < lastWork + task.duration; i++)
                    {
                        newOut += " " + worker.schedule[i] + ",";
                    }

                    newOut += " выполнил " + task.name;

                    output.Add(newOut);

                    worker.lastWork.Add(lastWork + task.duration);

                    //отмечаем, что эту пару задачи-работника мы уже рассмотрели
                    newGenome.genes[number] = 0;
                    newGenome.genes[number + newGenome.genes.Length / 2] = 0;

                    return 1;
                }
                else if (!task.importance && !endWorker)
                {
                    if (!notWork.Contains(task.name))
                    {
                        notWork.Add(task.name);
                    }
                    if (secondCond != "")
                    {
                        for (int i = 0; i < output.Count; i++)
                        {
                            if (output[i].Contains(secondCond))
                            {
                                //output.RemoveAt(i);
                                //secondCond = "";
                                return 0;
                            }
                        }
                    }
                    task.done = false;
                    return 1;
                }
                endWorker = false;
            }
            return 0;
        }
    }
}
