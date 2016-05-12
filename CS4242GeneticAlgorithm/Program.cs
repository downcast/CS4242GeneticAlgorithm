using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4242GeneticAlgorithm
{
    class Program
    {
        static Random rand;
        static StringBuilder[] population;
        static List<Tuple<int, StringBuilder>> eval;
        static List<Tuple<int, StringBuilder>> crossed;

        static void Main(string[] args)
        {
            rand = new Random();
            population = new StringBuilder[6];
            eval = new List<Tuple<int, StringBuilder>>();
            crossed = new List<Tuple<int, StringBuilder>>();
            bool valid = false;
            int num = 0;

            generatePopulation(6);

            while (!valid)
            {
                try
                {
                    Console.WriteLine("How many generations do you want?");
                    num = Convert.ToInt32(Console.ReadLine());
                    valid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid number. Try again.");
                }
            }
            
            

            for (int i = 0; i < num; i++)
            {
                evaluatePopulation();
                crossOver();
                mutate();

                Console.WriteLine("Generation: " + i + " : " + evaluatePopulation());
            }

            Console.ReadLine();
        }

        private static int evaluatePopulation()
        {
            int max = 0;

            eval.Clear();
            for (int i = 0; i < population.Length; i++)
            {
                int temp = (int)Math.Pow(Convert.ToInt32(population[i].ToString(), 2), 2);
                if (max < temp) { max = temp; }
                eval.Add(Tuple.Create(temp, population[i]));
            }
            return max;
        }

        /// <summary>
        /// Swaps the last 2 characters of the string in groups of 2 starting with the highest evaluated values
        /// </summary>
        private static void crossOver()
        {
            var ordered = eval.OrderByDescending(tuple => tuple.Item1);
            crossed.Clear();

            foreach (Tuple<int, StringBuilder> t in ordered) { crossed.Add(t); }
            
            String temp;
            for (int i = 0; i < crossed.Count; i += 2)
            {
                temp = crossed[i].Item2.ToString().Substring(3);

                crossed[i].Item2.Remove(3, 2);
                crossed[i].Item2.Append(crossed[i + 1].Item2.ToString().Substring(3));

                crossed[i + 1].Item2.Remove(3, 2);
                crossed[i + 1].Item2.Append(temp);
            }
        }

        /// <summary>
        /// Randomly decides whether or not it will mutate a string, then randomly decideds which character it will flip
        /// </summary>
        private static void mutate()
        {
            foreach (Tuple<int, StringBuilder> t in crossed)
            {
                int num = rand.Next(0, 5);
                if (num % 2 == 0)
                {
                    if (t.Item2[num].Equals('1')) { t.Item2[num] = '0'; }
                    else { t.Item2[num] = '1'; }
                }
                
            }

            // Sets the population to the mutated one
            for (int i = 0; i < population.Length; i++) { population[i] = crossed[i].Item2; }
        }

        #region initialization

        /// <summary>
        /// Creates the initial population of string
        /// </summary>
        /// <param name="count"></param>
        private static void generatePopulation(int count)
        {
            for (int i = 0; i < count; i++)
            {
                StringBuilder builder = new StringBuilder(5);

                for (int j = 0; j < 5; j++) { builder.Append(generateBit()); }
            
                population[i] = builder;
            }
        }

        /// <summary>
        /// Create a char (either '0' or '1') base on random number
        /// </summary>
        /// <returns></returns>
        private static char generateBit()
        {
            int num = rand.Next(0, 2);

            if (num == 0) { return '0'; }
            else if (num == 1) { return '1'; }
            else { Console.WriteLine("NOOOOOOOOO!"); return 'x'; }
        }
        #endregion
    }
}
