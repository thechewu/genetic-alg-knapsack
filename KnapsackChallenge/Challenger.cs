using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackChallenge
{
    class Challenger
    {
        int MAX_WEIGHT;
        int NUM_ITEMS;
        int MAX_VALUE = 0;
        int POPULATION = 1000;
        int GENERATIONS = 500;
        int CONVERGENCE = 0;
        int ELITE_1, ELITE_2;
        int CROSSOVER = 95;

        int[] fitness;
        int[] volume;
        int[] indexes;
        int[][] chromosomes;
        int[][] children;

        Random RANDOM_GENERATOR = new Random();

        Item[] items;

        /// <summary>
        /// Instantiates and sets all variables.
        /// </summary>
        /// <param name="size">Backpack size</param>
        /// <param name="weights">Array of weights</param>
        /// <param name="values">Array of values</param>
        public Challenger(int size, int[] weights, int[] values)
        {
            this.MAX_WEIGHT = size;
            this.NUM_ITEMS = values.Length;

            SetPopulation();
            SetGenerations();

            items = new Item[NUM_ITEMS];
            fitness = new int[POPULATION];
            volume = new int[POPULATION];
            indexes = new int[POPULATION];
            chromosomes = new int[POPULATION][];
            children = new int[POPULATION][];

            ResetIndexes();

            for (int i = 0; i < NUM_ITEMS; i++)
            {
                items[i] = new Item(values[i], weights[i]);
            }
        }

        /// <summary>
        /// Larger data sets require larger population size to find optimal solution consistently.
        /// </summary>
        void SetPopulation()
        {
            POPULATION = (int)(250 * (Math.Pow(Math.Log(NUM_ITEMS), 2)));
            POPULATION = (POPULATION % 2 == 0) ? POPULATION : POPULATION + 1;
        }

        /// <summary>
        /// Larger data sets require more generations to find optimal solution consistently.
        /// </summary>
        void SetGenerations()
        {
            GENERATIONS = (int)(100 * Math.Log(NUM_ITEMS));
        }

        /// <summary>
        /// Sets the index array to default values.
        /// </summary>
        void ResetIndexes()
        {
            for (int i = 0; i < POPULATION; i++)
            {
                indexes[i] = i;
            }
        }

        /// <summary>
        /// Generates a random starting population.
        /// </summary>
        void GeneratePopulation()
        {
            for (int i = 0; i < POPULATION; i++)
            {
                chromosomes[i] = new int[NUM_ITEMS];
                children[i] = new int[NUM_ITEMS];

                for (int j = 0; j < NUM_ITEMS; j++)
                {
                    chromosomes[i][j] = RANDOM_GENERATOR.Next(0, 2);
                }
            }
        }

        /// <summary>
        /// Calls Evaluation on all chromosomes.
        /// </summary>
        void Fitness()
        {
            for (int i = 0; i < POPULATION; i++)
            {
                Evaluate(i);
            }
        }

        /// <summary>
        /// Evaluates a single chromosome.
        /// </summary>
        /// <param name="index">Chromosome to evaluate</param>
        void Evaluate(int index)
        {
            int current_value = 0;
            int current_weight = 0;
            for (int j = 0; j < NUM_ITEMS; j++)
            {
                current_value += (chromosomes[index][j] == 1 ? items[j].value : 0);
                current_weight += (chromosomes[index][j] == 1 ? items[j].weight : 0);
            }
            if (current_weight <= MAX_WEIGHT)
            {
                fitness[index] = current_value;
                volume[index] = current_weight;
            }
            else
            {
                int rand = RANDOM_GENERATOR.Next(0, NUM_ITEMS);
                chromosomes[index][rand] = 1 - chromosomes[index][rand];
                Evaluate(index);
            }
        }

        /// <summary>
        /// Fitness index sorting and preservation of fittest chromosomes.
        /// </summary>
        void NextGeneration()
        {
            int[] temp = new int[POPULATION];
            ResetIndexes();

            Array.Copy(fitness, temp, POPULATION);
            Array.Sort(temp, indexes);

            ELITE_1 = indexes[POPULATION - 1];
            ELITE_2 = indexes[POPULATION - 2];

            Crossover();
        }

        /// <summary>
        /// Group selection on ascending array.
        /// </summary>
        /// <returns>Weighted random parent.</returns>
        int Selection()
        {
            int group = RANDOM_GENERATOR.Next(0, 100);

            int parent = RANDOM_GENERATOR.Next(0, POPULATION / 4);

            if (group < 95)
            {
                parent = RANDOM_GENERATOR.Next(POPULATION / 4, POPULATION / 2);
            }
            if (group < 80)
            {
                parent = RANDOM_GENERATOR.Next(POPULATION / 2, (int)(POPULATION * 0.75));
            }
            if (group < 50)
            {
                parent = RANDOM_GENERATOR.Next((int)(POPULATION * 0.75), POPULATION);
            }

            return indexes[parent];
        }

        /// <summary>
        /// Creation of next generation with crossover and mutation.
        /// </summary>
        void Crossover()
        {
            Array.Copy(chromosomes[ELITE_1], children[0], NUM_ITEMS);
            Array.Copy(chromosomes[ELITE_2], children[1], NUM_ITEMS);

            int cutoff = (int)(POPULATION * (CROSSOVER / 100.00));

            for (int i = 2; i < cutoff; i += 2)
            {
                int point = RANDOM_GENERATOR.Next(0, NUM_ITEMS);

                int parent1 = Selection();
                int parent2 = Selection();

                Array.Copy(chromosomes[parent1], children[i], NUM_ITEMS);
                for (int j = point; j < NUM_ITEMS; j++)
                {
                    children[i][j] = chromosomes[parent2][j];
                }

                Array.Copy(chromosomes[parent2], children[i + 1], NUM_ITEMS);
                for (int j = point; j < NUM_ITEMS; j++)
                {
                    children[i + 1][j] = chromosomes[parent1][j];
                }
            }

            for (int i = cutoff; i < POPULATION; i++)
            {
                Array.Copy(chromosomes[i], children[i], NUM_ITEMS);
            }

            Array.Copy(children, chromosomes, POPULATION);

            for (int i = 2; i < POPULATION; i++)
            {
                Mutate(i);
            }

        }

        /// <summary>
        /// 0.1% Chance of Mutation.
        /// </summary>
        /// <param name="index">Chromosome to mutate</param>
        void Mutate(int index)
        {
            for (int i = 0; i < NUM_ITEMS; i++)
            {
                if (i == ELITE_1 || i == ELITE_2)
                {
                    continue;
                }
                int mutator = RANDOM_GENERATOR.Next(0, 1000);
                if (mutator == 1)
                {
                    chromosomes[index][i] = 1 - chromosomes[index][i];
                }
            }
        }

        /// <summary>
        /// Determines fittest chromosome and checks convergence of the population.
        /// </summary>
        void Convergence()
        {
            MAX_VALUE = 0;
            for (int i = 0; i < POPULATION; i++)
            {
                if (fitness[i] >= fitness[MAX_VALUE])
                {
                    MAX_VALUE = i;
                }
            }

            int converging = 0;
            for (int i = 0; i < POPULATION; i++)
            {
                if (fitness[MAX_VALUE] == fitness[i])
                {
                    converging++;
                }
            }

            CONVERGENCE = converging;
        }

        /// <summary>
        /// Prints the current population.
        /// </summary>
        void ViewPopulation()
        {
            for (int i = 0; i < POPULATION; i++)
            {
                Console.WriteLine(String.Join(",", chromosomes[i]));
            }
        }

        /// <summary>
        /// Runs the entire simulation.
        /// </summary>
        public void Go()
        {
            GeneratePopulation();

            for (int i = 0; i < GENERATIONS; i++)
            {
                NextGeneration();
                Fitness();
                Convergence();
                if (CONVERGENCE >= POPULATION * 0.9)
                {
                    GENERATIONS = i;
                    break;
                }
            }

            Console.WriteLine("Population: " + POPULATION);
            Console.WriteLine("Generations: " + GENERATIONS);
            Console.WriteLine("Convergence: " + Math.Round((100.00 * CONVERGENCE) / POPULATION, 2) + "%");
            Console.WriteLine("Max value: " + fitness[MAX_VALUE]);
            Console.WriteLine("Max weight: " + volume[MAX_VALUE]);
            Console.WriteLine("Item Sequence: " + String.Join(",", chromosomes[MAX_VALUE]));
            Console.WriteLine();
        }
    }
}
