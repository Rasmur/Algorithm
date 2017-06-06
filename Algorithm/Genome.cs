using System;
using System.Collections;

namespace Algorithm
{
    /// <summary>
    /// Summary description for Genome.
    /// </summary>
    public class Genome
    {
        public int[] genes;
        private double fitness;
        
        private static double mutationRate;

        public Genome()
        {
            RandomGenes(0, Program.tasks.Count / 2, Program.tasks.Count);
            RandomGenes(Program.tasks.Count / 2, Program.tasks.Count, Program.workers.Count);
        }

        public void RandomGenes(int i, int end, int length)
        {
            Random random = new Random();
            string notEqual = "";

            for (; i < end; i++)
            {
                genes[i] = random.Next(1, length);

                if (notEqual.Contains(random.Next(1, length).ToString()))
                {
                    i--;
                }
                else
                {
                    notEqual += genes[i].ToString();
                }
            }
        }
        
        /// <summary>
        /// скрещивание
        /// </summary>
        /// <param name="genome2"></param>
        /// <param name="child1"></param>
        /// <param name="child2"></param>
		public void Crossover(ref Genome genome2, out Genome child1, out Genome child2)
        {
            Random random = new Random();

            int pos = (int)(m_random.NextDouble() * (double)length);
            child1 = new Genome(length, false);
            child2 = new Genome(length, false);
            for (int i = 0; i < length; i++)
            {
                if (i < pos)
                {
                    child1.genes[i] = genes[i];
                    child2.genes[i] = genome2.genes[i];
                }
                else
                {
                    child1.genes[i] = genome2.genes[i];
                    child2.genes[i] = genes[i];
                }
            }
        }

        public void Mutate()
        {
            for (int pos = 0; pos < length; pos++)
            {
                if (m_random.NextDouble() < mutationRate)
                    genes[pos] = (genes[pos] + (int)m_random.NextDouble()) / 2;
            }
        }

        public int[] Genes()
        {
            return genes;
        }

        public void GetValues(ref double[] values)
        {
            for (int i = 0; i < length; i++)
                values[i] = genes[i];
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

        public int Length
        {
            get
            {
                return length;
            }
        }
    }
}