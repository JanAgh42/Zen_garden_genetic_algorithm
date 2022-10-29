using ZenGarden.src.logic;
using ZenGarden.src.models;

namespace ZenGarden
{
    class Program
    {
        static void Main(string[] args)
        {
            var stones = new List<(int, int)>();
            stones.Add((2, 3));
            stones.Add((3, 5));
            stones.Add((5, 4));
            stones.Add((6, 2));
            stones.Add((10, 7));
            stones.Add((9, 7));

            GeneticAlgorithm a = new GeneticAlgorithm(14, 12, stones);

            a.ComputeFitness();

            for (int x = 0; x < 200; x++)
            {
                a.Monk.CreateNewGeneration();
                a.ComputeFitness();
            }

            Console.WriteLine("--------");

            a.Garden.PrintGarden();
            Console.WriteLine(a.Monk.MaxFitness);
        }
    }
}