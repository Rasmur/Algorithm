using System;
using System.Collections;
using btl.generic;

namespace btl.generic
{
    /// <summary>
    /// Summary description for Genome.
    /// </summary>
    public class Genome
    {
        public double[] m_genes;
        private int m_length;
        private double m_fitness;
        static Random m_random = new Random();

        private static double mutationRate;

        public Genome(int length, bool createGenes = true)
        {
            m_length = length;
            m_genes = new double[length];
            if (createGenes)
                CreateGenes();
        }

        public Genome(ref double[] genes)
        {
            m_length = genes.GetLength(0);
            m_genes = new double[m_length];
            for (int i = 0; i < m_length; i++)
                m_genes[i] = genes[i];
        }


        private void CreateGenes()
        {
            for (int i = 0; i < m_length; i++)
                m_genes[i] = m_random.NextDouble();
        }

        /// <summary>
        /// скрещивание
        /// </summary>
        /// <param name="genome2"></param>
        /// <param name="child1"></param>
        /// <param name="child2"></param>
		public void Crossover(ref Genome genome2, out Genome child1, out Genome child2)
        {
            int pos = (int)(m_random.NextDouble() * (double)m_length);
            child1 = new Genome(m_length, false);
            child2 = new Genome(m_length, false);
            for (int i = 0; i < m_length; i++)
            {
                if (i < pos)
                {
                    child1.m_genes[i] = m_genes[i];
                    child2.m_genes[i] = genome2.m_genes[i];
                }
                else
                {
                    child1.m_genes[i] = genome2.m_genes[i];
                    child2.m_genes[i] = m_genes[i];
                }
            }
        }


        public void Mutate()
        {
            for (int pos = 0; pos < m_length; pos++)
            {
                if (m_random.NextDouble() < mutationRate)
                    m_genes[pos] = (m_genes[pos] + m_random.NextDouble()) / 2.0;
            }
        }

        public double[] Genes()
        {
            return m_genes;
        }

        public void GetValues(ref double[] values)
        {
            for (int i = 0; i < m_length; i++)
                values[i] = m_genes[i];
        }

        public double Fitness
        {
            get
            {
                return m_fitness;
            }
            set
            {
                m_fitness = value;
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
                return m_length;
            }
        }
    }
}