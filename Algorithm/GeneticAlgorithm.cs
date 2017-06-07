using System;
using System.Collections;
using System.IO;

namespace Algorithm
{
    public delegate int GAFunction(double[] values);

    /// <summary>
    /// Genetic Algorithm class
    /// </summary>
    public class GA
    {
        private double mutationRate;
        private int populationSize;
        private int generationSize;
        private int genomeSize;
        public double totalFitness;

        public ArrayList thisGeneration;
        private ArrayList fitnessTable;

        static private GAFunction getFitness;

        public GA()
        {
            mutationRate = 0.05;
            populationSize = 10;
            generationSize = 100;
        }

        /// <summary>
        /// Method which starts the GA executing.
        /// </summary>
        public void Go()
        {
            fitnessTable = new ArrayList();
            thisGeneration = new ArrayList(generationSize);
            Genome.MutationRate = mutationRate;

            //Program.tasks.Sort();

            //создаёт популяцию
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

            if (randomFitness == 0)
            {
                return 0;
            }
            //т.к. бинарный поиск в ArrayList только для точных значений, то делаем его вручную
            while (index == null && first <= last)
            {
                if (randomFitness < (double)fitnessTable[mid])
                {
                    last = mid;
                }
                else if (randomFitness > (double)fitnessTable[mid])
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
                Genome g = ((Genome)thisGeneration[i]);
                totalFitness += g.fitness;
            }

            //thisGeneration.Sort(new GenomeComparer());

            double fitness = 0.0;
            fitnessTable.Clear();
            for (int i = 0; i < populationSize; i++)
            {
                fitness += ((Genome)thisGeneration[i]).fitness;
                fitnessTable.Add((double)fitness);
            }
        }

        /// <summary>
        /// Create the *initial* genomes by repeated calling the supplied fitness function
        /// </summary>
        private void CreateGenomes()
        {
            for (int i = 0; i < populationSize; i++)
            {
                Genome g = new Genome(i);

                Fitness fitnessFunction = new Fitness(g);
                
                thisGeneration.Add(g);
            }
        }

        private void CreateNextGeneration()
        {
            Fitness fitnessFunction;
            for (int i = 0; i < populationSize; i += 2)
            {
                int parentIndex1 = RouletteSelection();
                int parentIndex2 = RouletteSelection();
                Genome parent1, parent2, child1, child2;
                parent1 = ((Genome)thisGeneration[parentIndex1]);
                fitnessFunction = new Fitness(parent1);

                parent2 = ((Genome)thisGeneration[parentIndex2]);
                fitnessFunction = new Fitness(parent2);

                parent1.Crossover(ref parent2, out child1, out child2);

                child1.Mutate();
                fitnessFunction = new Fitness(child1);

                child2.Mutate();
                fitnessFunction = new Fitness(child2);

                thisGeneration.Add(child1);
                thisGeneration.Add(child2);
            }
            Console.WriteLine("Wewewew");
            thisGeneration.Sort(new Comparer());
            
            if ((thisGeneration[0] as Genome).fitness == 0)
            {
               SelectTheBest(thisGeneration);
            }
            else
            {
                for (int count = thisGeneration.Count / 2, i = 0; i < count; i++)
                {
                    thisGeneration.RemoveAt(count);
                }
                //thisGeneration.RemoveRange(thisGeneration.Count / 2, thisGeneration.Count-1);

            }
            //thisGeneration.RemoveRange(0, populationSize);
        }

        void SelectTheBest(ArrayList generation)
        {
            object x = new Genome(42);
            ArrayList newList = new ArrayList();
            //generation.LastIndexOf(((Genome)x).fitness = 0);
            int target = generation.LastIndexOf(((Genome)x).fitness = 0);

            for (int i = target; i < generation.Count; i++)
            {
                newList.Add(generation[i]);
                
            }

            for (int i = 0; i < target; i++)
            {
                newList.Add(0);
            }

            generation = newList;
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

        public Genome GetBest(ref double fitness)
        {
            Genome g = ((Genome)thisGeneration[0]);
            fitness = g.fitness;
            return g;
        }

    }
}