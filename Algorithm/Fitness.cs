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
        Genome genome;

        Fitness(Genome genome)
        {
            this.genome = genome;
        }

        public int FitnessFunction(int number = -1)
        {
            Task task;

            if (number != -1 && genome.genes[number] != 0)
            {
                CheckCondition(number);

                if (CountFitness(number) == 0)
                {
                    return 0;
                }
            }
            else
            {
                for (int i = 0; i < genome.genes.Length; i++)
                {
                    if (genome.genes[i] != 0)
                    {
                        task = Program.tasks[genome.genes[i]];
                        CheckCondition(i);

                        if (CountFitness(i) == 0)
                        {
                            return 0;
                        }
                    }
                }

                return totalFitness;
            }
            return 0;
        }

        private void CheckCondition(int number)
        {
            for (int i = 0; i < Conditions.atTheSameTime.Count; i++)
            {
                if (Conditions.atTheSameTime.ContainsKey(genome.genes[number]))
                {
                    FitnessFunction(Conditions.atTheSameTime[genome.genes[number]]);
                }
                if (Conditions.atTheSameTime.ContainsValue(genome.genes[number]))
                {
                    int key = Conditions.atTheSameTime.FirstOrDefault(x => x.Value == genome.genes[number]).Key;
                    FitnessFunction(key);
                }
            }

            for (int i = 0; i < Conditions.necessity.Count; i++)
            {
                if (Conditions.necessity.ContainsKey(genome.genes[number]))
                {
                    FitnessFunction(Conditions.necessity[genome.genes[number]]);
                }
                if (Conditions.necessity.ContainsValue(genome.genes[number]))
                {
                    int key = Conditions.necessity.FirstOrDefault(x => x.Value == genome.genes[number]).Key;
                    FitnessFunction(key);
                }
            }
        }

        private void FindInDictionary(Genome genome, int number, Dictionary<int, int> conditions)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions.ContainsKey(genome.genes[number]))
                {
                    FitnessFunction(conditions[genome.genes[number]]);
                }
                if (conditions.ContainsValue(genome.genes[number]))
                {
                    int key = conditions.FirstOrDefault(x => x.Value == genome.genes[number]).Key;
                    FitnessFunction(key);
                }
            }
        }

        private int CountFitness(int number)
        {
            Task task = Program.tasks[genome.genes[number]];
            Worker worker = new Worker();
            int lastWork = worker.lastWork;

            //чтобы не выходить за рамки дозволенного
            if ((task.duration <= worker.schedule.Length - lastWork) &&
                worker.schedule[task.duration + lastWork - 1] <= task.deadline)
            {
                totalFitness += worker.costPerHour * task.duration;

                //отмечаем, что эту пару задачи-работника мы уже рассмотрели
                genome.genes[number] = 0;
                genome.genes[number + genome.genes.Length / 2] = 0;

                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
