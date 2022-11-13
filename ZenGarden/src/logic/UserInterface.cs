using ZenGarden.src.models;
using System.Linq;

namespace ZenGarden.src.logic
{
    class UserInterface
    {
        public GeneticAlgorithm Algorithm { get; private set; } = null!;

        public UserInterface()
        {
            InitializeExecution();
        }

        private void InitializeExecution()
        {
            string choice;

            do
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("12_10_6 12_10_6x 15_15_10 6_6_2 20_13_14 7_13_6 11_15_9 10_8_5");
                Console.WriteLine("-------------------------");
                Console.Write("Enter filename without '.txt' or 'exit': ");

                choice = Console.ReadLine() ?? "6_6_2";

                switch (choice)
                {
                    case "12_10_6":
                    case "12_10_6x":
                    case "15_15_10":
                    case "6_6_2":
                    case "7_13_6":
                    case "20_13_14":
                    case "11_15_9":
                    case "10_8_5":
                        AutoTesting(choice); break;
                    case "exit": break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }

            } while(choice != "exit");
        }

        private void AutoTesting(string filename)
        {
            var stones = new List<(int, int)>();
            var leaves = new List<Leaf>();

            int totalLeaves = 0, collectedLeaves = 0;

            bool leafSwitch = false;

            var info = filename.Split("_");

            int width = Convert.ToInt32(info[0]);
            int height = Convert.ToInt32(info[1]);

            Console.WriteLine("-------------------------");
            Console.Write("Enter number of generations: ");

            int generations = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("-------------------------");
            Console.Write("Enter number of runs (1 if you want graphical representation): ");

            int runs = Convert.ToInt32(Console.ReadLine());

            var maxFitnesses = new List<double>(new double[generations]);

            foreach (string line in System.IO.File.ReadLines(@"./src/inputs/" + filename + ".txt"))
            {
                if (line == "-") {
                    leafSwitch = true;
                    continue;
                }
                var tokens = line.Split("_");
                var coords = (Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]));

                if (leafSwitch) {
                    leaves.Add(new Leaf(coords, Converters.GetLeafColor(tokens[2])));
                    totalLeaves++;
                }
                else {
                    stones.Add(coords);
                }
            }

            for (int counter = 0; counter < runs; counter++)
            {
                var fitness = new List<string>();

                Algorithm = new GeneticAlgorithm(width + 2, height + 2, stones, leaves);

                Algorithm.Garden.GenerateGarden();
                
                if (runs == 1) {
                    Counter.Get().Start();
                    Algorithm.Garden.CreateCopy();
                    Algorithm.Garden.PrintGarden();
                }
                else {
                    Console.WriteLine(counter + ". generation");
                }

                fitness.Add(Algorithm.ComputeFitness().ToString());
                maxFitnesses[0] += Convert.ToDouble(fitness.LastOrDefault());

                for (int x = 1; x < generations; x++)
                {
                    Algorithm.Monk.CreateNewGeneration();
                    fitness.Add(Algorithm.ComputeFitness().ToString());
                    if (runs != 1) {
                        maxFitnesses[x] += Convert.ToDouble(fitness.LastOrDefault());
                    }
                }

                if (runs == 1) {
                    Counter.Get().Stop();
                    File.WriteAllLines("fitness.txt", fitness);
                }
            }
            
            if (runs == 1) {
                collectedLeaves = totalLeaves;

                foreach (var ch in Algorithm.Garden.FinalPortions)
                {
                    if (ch.IsLeaf())
                    {
                        collectedLeaves--;
                    }
                }

                Console.WriteLine("***********************************");
                Algorithm.Garden.PrintGarden();
                Console.WriteLine("***********************************");
                Console.WriteLine("Number of generations: " + generations);
                Console.WriteLine("Fitness value: " + Algorithm.Monk.MaxFitness);
                Console.WriteLine("Raked portions: " + (int)Algorithm.Monk.MaxFitness + "/" + Algorithm.Garden.NumOfEmptyPortions);
                Console.WriteLine("Collected leaves: " + collectedLeaves + "/" + totalLeaves);
                Console.WriteLine("Solution efficiency: " + Math.Round((((int)Algorithm.Monk.MaxFitness) / Algorithm.Garden.NumOfEmptyPortions) * 100, 2) + "%");
                Console.WriteLine("Elapsed time: " + Counter.Get().GetMilliseconds() + "ms");
                Console.WriteLine("***********************************");
            }
            else {
                var fitness = new List<string>();

                for (int counter = 0; counter < generations; counter++)
                {
                    maxFitnesses[counter] = maxFitnesses[counter] / runs;
                    fitness.Add(maxFitnesses[counter].ToString());
                }
                File.WriteAllLines("fitness.txt", fitness);
            }
        }
    }
}