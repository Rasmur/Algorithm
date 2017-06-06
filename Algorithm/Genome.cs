using System;
using System.Collections;
using System.Collections.Generic;

namespace Algorithm
{
    /// <summary>
    /// Summary description for Genome.
    /// </summary>
    public class Genome
    {
        public int[] genes = new int[Program.tasks.Count * 2];
        private int fitness;
        
        private static double mutationRate;

        public Genome()
        {
            Random random = new Random();
            
            int i;

            List<int> was = new List<int>();

            for (int j = 0; j < Program.tasks.Count; j++)
            {
                was.Add(j + 1);
            }

            for (i = 0; i < Program.tasks.Count; i++)
            {
                genes[i] = random.Next(was[0],was[was.Count - 1]);

                if (was.Contains(genes[i]))
                {
                    was.Remove(genes[i]);
                }
            }

            for (; i < Program.tasks.Count * 2; i++)
            {
                genes[i] = random.Next(1, Program.workers.Count + 1);
            }
        }
        
        /// <summary>
        /// скрещивание
        /// </summary>
        /// <param name="genome2"></param>
        /// <param name="child1"></param>
        /// <param name="child2"></param>
		public void Crossover(ref Genome parent2, out Genome child1, out Genome child2)
        {
            Random random = new Random();
            int length = Program.tasks.Count * 2;

            int pos = (int)(random.NextDouble() * length);
            child1 = new Genome();
            child2 = new Genome();

            for (int i = 0; i < length; i++)
            {
                if (i < pos)
                {
                    child1.genes[i] = genes[i];
                    child2.genes[i] = parent2.genes[i];
                }
                else
                {
                    child1.genes[i] = parent2.genes[i];
                    child2.genes[i] = genes[i];
                }
            }
        }

        public void Mutate()
        {
            Random random = new Random();

            for (int pos = 0; pos < Program.tasks.Count * 2; pos++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    genes[pos] = (random.Next(-genes[pos], Program.tasks.Count)) / 2;
                }
            }
        }

        public int[] Genes()
        {
            return genes;
        }
        
        public int Fitness
        {
            get
            {
                return fitness;
            }
            set
            {
                fitness = value;
            }
        }

        public static double MutationRate
        {
            get
            {
                return mutationRate;
            }
            set
            {
                mutationRate = value;
            }
        }
    }
}