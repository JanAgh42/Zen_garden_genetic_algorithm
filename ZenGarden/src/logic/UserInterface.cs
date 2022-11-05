using ZenGarden.src.models;

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
                Console.WriteLine("12_10_6 15_15_10 6_6_2");
                Console.WriteLine("-------------------------");
                Console.Write("Enter filename without '.txt' or 'exit': ");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "12_10_6":
                    case "15_15_10":
                    case "6_6_2":
                    case "9_13_5":
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

            bool leafSwitch = false;

            var info = filename.Split("_");

            int width = Convert.ToInt32(info[0]);
            int height = Convert.ToInt32(info[1]);

            Console.WriteLine("-------------------------");
            Console.Write("Enter number of generations: ");

            int generations = Convert.ToInt32(Console.ReadLine());

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
                }
                else {
                    stones.Add(coords);
                }
            }

            Algorithm = new GeneticAlgorithm(width + 2, height + 2, stones, leaves);

            Algorithm.Garden.GenerateGarden();
            Algorithm.Garden.CreateCopy();
            Algorithm.Garden.PrintGarden();

            Algorithm.ComputeFitness();

            for (int x = 0; x < generations; x++)
            {
                Algorithm.Monk.CreateNewGeneration();
                Algorithm.ComputeFitness();
            }

            Console.WriteLine("***********************************");
            Algorithm.Garden.PrintGarden();
            Console.WriteLine("***********************************");
            Console.WriteLine("Number of generations: " + generations);
            Console.WriteLine("Fitness value: " + Algorithm.Monk.MaxFitness);
            Console.WriteLine("***********************************");
        }
    }
}