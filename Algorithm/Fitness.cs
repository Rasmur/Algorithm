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
        Genome newGenome = new Genome(415);
        Dictionary<int, int> necessity ;
        Dictionary<int, int> atTheSameTime;

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
                CheckCondition(number);

                if (newGenome.genes[number] != 0 && CountFitness(number) == 0)
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
                        CheckCondition(i);

                        if (CountFitness(i) == 0)
                        {
                            return 0;
                        }
                    }
                }
            }
            return totalFitness;
        }

        /// <summary>
        /// проверка условий
        /// </summary>
        /// <param name="number">номер задачи</param>
        private void CheckCondition(int number)
        {
            for (int i = 0; i < atTheSameTime.Count; i++)
            {
                if (atTheSameTime.ContainsKey(newGenome.genes[number]))
                {
                    int value = atTheSameTime[newGenome.genes[number]];
                    atTheSameTime.Remove(newGenome.genes[number]);
                    FitnessFunction(value);
                }
                if (atTheSameTime.ContainsValue(newGenome.genes[number]))
                {
                    int key = atTheSameTime.FirstOrDefault(x => x.Value == newGenome.genes[number]).Key;
                    atTheSameTime.Remove(key);
                    FitnessFunction(key);
                }
            }

            for (int i = 0; i < necessity.Count; i++)
            {
                if (necessity.ContainsValue(newGenome.genes[number]))
                {
                    int key = necessity.FirstOrDefault(x => x.Value == newGenome.genes[number]).Key;
                    necessity.Remove(key);

                    FitnessFunction(key);
                }
            }
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

                    worker.lastWork.Add(lastWork + task.duration);

                    //отмечаем, что эту пару задачи-работника мы уже рассмотрели
                    newGenome.genes[number] = 0;
                    newGenome.genes[number + newGenome.genes.Length / 2] = 0;

                    return 1;
                }
                //если задача неважна
                else if (!task.importance)
                {
                    task.done = false;
                    return 1;
                }
            }
            return 0;
        }
    }
}
