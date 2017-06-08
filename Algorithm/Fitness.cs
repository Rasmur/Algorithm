using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class Fitness
    {
        int totalFitness = 0;
        Genome newGenome = new Genome(428);
        Dictionary<int, int> necessity ;
        Dictionary<int, int> atTheSameTime;

        //запоминать начало работы у работника при условии одинакового выполнения
        int beginWorker = 0;
        bool startWorker = false;
        bool endWorker = false;

        bool cond = false;

        int buf1;
        int buf2;

        public Fitness(Genome genome)
        {
            //создаём копию workers
            for (int i = 0; i < Program.workers.Count; i++)
            {
                Program.workers[i].lastWork.Clear();
                Program.workers[i].lastWork.Add(0);
            }

            //создаём копии словарей
            necessity = new Dictionary<int, int>(Conditions.necessity);
            atTheSameTime = new Dictionary<int, int>(Conditions.atTheSameTime);

            //копию генома
            genome.genes.CopyTo(newGenome.genes, 0);
            
            genome.fitness = FitnessFunction();
        }

        public int FitnessFunction(int number = -1)
        {
            Task task;

            if (number != -1)
            {
                buf1 = CheckCondition(number);
                buf2 = CountFitness(number);

                if (buf1 == 0 || buf2 == 0)
                {
                    return 0;
                }
            }
            else
            {
                for (int i = 0; i < Program.tasks.Count - 1; i++)
                {
                    if (newGenome.genes[i] != 0)
                    {
                        task = Program.tasks[newGenome.genes[i] - 1];

                        buf1 = CheckCondition(i);
                        buf2 = CountFitness(i);

                        if (buf1 == 0 || buf2 == 0)
                        {
                            return 0;
                        }
                    }
                }
            }

            if (totalFitness == 0)
            {
                return 1;
            }
            else
            {
                return totalFitness;
            }
        }

        /// <summary>
        /// проверка условий
        /// </summary>
        /// <param name="number">номер задачи</param>
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

                    int j;
                    for (j = 0; newGenome.genes[j] != value; j++)
                    {}

                    if (FitnessFunction(j) == 0)
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

                    int j;
                    for (j = 0; newGenome.genes[j] != key; j++)
                    { }

                    if (FitnessFunction(j) == 0)
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

                    int j;
                    for (j = 0; newGenome.genes[j] != key; j++)
                    { }

                    cond = true;
                    if (FitnessFunction(j) == 0)
                    {
                        cond = false;
                        return 0;
                    }

                    cond = true;                    
                }
            }
            return 1;
        }

        /// <summary>
        /// непосредственно подсчитывается фитнесс
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int CountFitness(int number)
        {
            //если задача ещё не рассмотрена
            if (newGenome.genes[number] != 0)
            {
                Task task = Program.tasks[newGenome.genes[number] - 1];
                Worker worker = Program.workers[newGenome.genes[number + newGenome.genes.Length / 2] - 1];
                int lastWork = worker.lastWork.Last();

                //чтобы не выходить за рамки дозволенного и проверка на работоспособность
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
                                    lastWork = i + task.duration;
                                }
                            }

                            if (!buff)
                            {
                                return 0;
                            }
                        }

                        return 0;
                    }

                    cond = false;
                    worker.lastWork.Add(lastWork + task.duration);

                    //отмечаем, что эту пару задачи-работника мы уже рассмотрели
                    newGenome.genes[number] = 0;
                    newGenome.genes[number + newGenome.genes.Length / 2] = 0;

                    return 1;
                }
                //если задача неважна
                else if (!task.importance && !endWorker && !cond)
                {
                    cond = false;
                    return 1;
                }
                endWorker = false;
            }
            cond = false;
            return 0;
        }
    }
}
