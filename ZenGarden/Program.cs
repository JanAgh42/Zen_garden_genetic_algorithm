using ZenGarden.src.logic;
using ZenGarden.src.models;

namespace ZenGarden
{
    class Program
    {
        static void Main(string[] args)
        {
            var stones = new List<(int, int)>();
            stones.Add((1, 2));
            stones.Add((2, 4));
            stones.Add((5, 3));

            GeneticAlgorithm a = new GeneticAlgorithm(10, 15, stones);
            // a.Garden.GenerateGarden();
            // a.Garden.PrintGarden();
            // Console.WriteLine("a");
            a.ComputeFitness();
            // Console.WriteLine(Converters.GetPos(10, 15, 21, 15));
            // Garden G = new Garden(10, 15, stones);
            // G.GenerateGarden();
            // G.PrintGarden();
            // Console.WriteLine(G.GetPerimCoords(12));
            // a.Garden.PrintGarden();
            // bool x = Validation.IsLegalPos(a.Garden.Width, a.Garden.Height, a.Garden.GardenPortions[3, 0]);
            // bool y = a.Garden.GardenPortions[1, 0].IsStone();
            // for(int d = 0; d < a.Garden.GardenPortions.GetLength(0); d++) {
            //     for (int c = 0; c < a.Garden.GardenPortions.GetLength(1); c++)
            //     {
            //         if(a.Garden.GardenPortions[d, c].IsStone()) {
            //             Console.WriteLine(d + " " + c);
            //         }
            //     }
            // }
            // Console.WriteLine(x);
            // Console.WriteLine(y);

        }
    }
}