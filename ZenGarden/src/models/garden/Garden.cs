using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    class Garden
    {
        public GardenPortion[,] GardenPortions { get; private set; } = null!;
        public List<Leaf> Leaves { get; private set; } = null!;
        
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Garden(int width, int height, List<(int, int)> stones)
        {
            Width = width;
            Height = height;

            GardenPortions = new GardenPortion[height, width];

            GenerateGarden(stones);
        }

        private void GenerateGarden(List<(int, int)> stones)
        {
            foreach((int X, int Y) stone in stones)
             {
                GardenPortions[stone.Y - 1, stone.X - 1] = new Stone(stone);
             }

            for (int x = 0; x < GardenPortions.GetLength(0); x++)
            {
                for (int y = 0; y < GardenPortions.GetLength(1); y++)
                {
                    GardenPortions[x, y] = GardenPortions[x, y] ?? new GardenPortion((x, y));
                }
            }
        }

        public void PrintGarden()
        {
            for (int x = 0; x < GardenPortions.GetLength(0); x++)
            {
                for (int y = 0; y < GardenPortions.GetLength(1); y++)
                {
                    Console.Write((char) GardenPortions[x, y].Label + " ");
                }
                Console.WriteLine();
            }
        }
    }
}