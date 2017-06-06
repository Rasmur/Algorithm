using System;
using System.Collections;
using System.IO;

namespace btl.generic
{
    public delegate double GAFunction(double[] values);

    /// <summary>
    /// Genetic Algorithm class
    /// </summary>
    public class GA
    {
        private double mutationRate;
        private int populationSize;
        private int generationSize;
        private int genomeSize;
        private double totalFitness;

        private ArrayList m_thisGeneration;
        private ArrayList m_nextGeneration;
        private ArrayList m_fitnessTable;

        static private GAFunction getFitness;

        public GA()
        {
            mutationRate = 0.05;
            populationSize = 10;
            generationSize = 200;
        }
        
        /// <summary>
        /// Method which starts the GA executing.
        /// </summary>
        public void Go()
        {
            if (getFitness == null)
                throw new ArgumentNullException("Необходимо задать функцию приспособленности");
            if (genomeSize == 0)
                throw new IndexOutOfRangeException("Размер генома установлен неверно");

            //  Create the fitness table.
            m_fitnessTable = new ArrayList();
            m_thisGeneration = new ArrayList(generationSize);
            m_nextGeneration = new ArrayList(generationSize);
            Genome.MutationRate = mutationRate;


            CreateGenomes();
            RankPopulation();

            
            for (int i = 0; i < generationSize; i++)
            {
                CreateNextGeneration();
                RankPopulation();
            }
            
        }

        /// <summary>
        /// After ranking all the genomes by fitness, use a 'roulette wheel' selection
        /// method.  This allocates a large probability of selection to those with the 
        /// highest fitness.
        /// </summary>
        /// <returns>Random individual biased towards highest fitness</returns>
        private int RouletteSelection()
        {
            Random random = new Random();

            double randomFitness = random.NextDouble() * totalFitness;
            int? index = null;
            int mid;
            int first = 0;
            int last = populationSize - 1;
            mid = (last - first) / 2;

            //т.к. бинарный поиск в ArrayList только для точных значений, то делаем его вручную
            while (index == null && first <= last)
            {
                if (randomFitness < (double)m_fitnessTable[mid])
                {
                    last = mid;
                }
                else if (randomFitness > (double)m_fitnessTable[mid])
                {
                    first = mid;
                }
                mid = (first + last) / 2;
                //  lies between i and i+1
                if ((last - first) == 1)
                    index = last;
            }
            return (int)index;
        }

        /// <summary>
        /// Классифицировать популяцию и сортировать в порядке пригодности.
        /// </summary>
        private void RankPopulation()
        {
            ///общая приспособленность поколения
            totalFitness = 0;
            for (int i = 0; i < populationSize; i++)
            {
                Genome g = ((Genome)m_thisGeneration[i]);
                g.Fitness = FitnessFunction(g.Genes());
                totalFitness += g.Fitness;
            }
            m_thisGeneration.Sort(new GenomeComparer());

            //  сортировка в порядке пригодности
            double fitness = 0.0;
            m_fitnessTable.Clear();
            for (int i = 0; i < populationSize; i++)
            {
                fitness += ((Genome)m_thisGeneration[i]).Fitness;
                m_fitnessTable.Add((double)fitness);
            }
        }

        /// <summary>
        /// Create the *initial* genomes by repeated calling the supplied fitness function
        /// </summary>
        private void CreateGenomes()
        {
            for (int i = 0; i < populationSize; i++)
            {
                Genome g = new Genome(genomeSize);
                m_thisGeneration.Add(g);
            }
        }

        private void CreateNextGeneration()
        {
            m_nextGeneration.Clear();
            Genome g = null;

            //самый худший из текущей популяции
            g = (Genome)m_thisGeneration[0];

            for (int i = 0; i < populationSize; i += 2)
            {
                int parentIndex1 = RouletteSelection();
                int parentIndex2 = RouletteSelection();
                Genome parent1, parent2, child1, child2;
                parent1 = ((Genome)m_thisGeneration[parentIndex1]);
                parent2 = ((Genome)m_thisGeneration[parentIndex2]);

                parent1.Crossover(ref parent2, out child1, out child2);

                child1.Mutate();
                child2.Mutate();

                m_nextGeneration.Add(child1);
                m_nextGeneration.Add(child2);
            }
            
            SelectBestIndividual();
        }

        private void SelectBestIndividual()
        {
            ArrayList bestGeneration = new ArrayList();
            bestGeneration.AddRange(m_thisGeneration);
            bestGeneration.AddRange(m_nextGeneration);
            bestGeneration.Sort(new GenomeComparer());

            bestGeneration.RemoveRange(0, populationSize);

            m_thisGeneration.Clear();

            m_thisGeneration.AddRange(bestGeneration);
        }

        public GAFunction FitnessFunction
        {
            get
            {
                return getFitness;
            }
            set
            {
                getFitness = value;
            }
        }


        //  Properties
        public int PopulationSize
        {
            get
            {
                return populationSize;
            }
            set
            {
                populationSize = value;
            }
        }

        public int Generations
        {
            get
            {
                return generationSize;
            }
            set
            {
                generationSize = value;
            }
        }

        public int GenomeSize
        {
            get
            {
                return genomeSize;
            }
            set
            {
                genomeSize = value;
            }
        }
        
        public void GetBest(out double[] values, out double fitness)
        {
            Genome g = ((Genome)m_thisGeneration[populationSize - 1]);
            values = new double[g.Length];
            g.GetValues(ref values);
            fitness = (double)g.Fitness;
        }       
    }
}