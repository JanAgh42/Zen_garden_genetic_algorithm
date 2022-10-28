using System;
using ZenGarden.src.constants;
using ZenGarden.src.logic;

namespace ZenGarden.src.models
{
    class Garden
    {
        public GardenPortion[,] GardenPortions { get; private set; } = null!;
        public List<Leaf> Leaves { get; private set; } = null!;
        public List<(int, int)> Stones { get; private set; } = null!;
        
        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly Random _random = null!;

        public Garden(int width, int height, List<(int, int)> stones)
        {
            Width = width;
            Height = height;
            Stones = stones;

            _random = new Random();
        }

        public void RakeGardenPortion((int X, int Y) coords, int order)
        {
            GardenPortions[coords.Y, coords.X] = new RakedPortion(coords, order);
        }

        public (int X, int Y) GetPerimCoords(int perim)
        {
            var perimCoords = (0, 0);

            foreach (GardenPortion portion in GardenPortions)
            {
                if(portion.IsPerim() && ((PerimeterPortion)portion).Number == perim) {
                    perimCoords = ((PerimeterPortion) portion).GetCoords;
                    break;
                }
            }
            return perimCoords;
        }

        public (int, int) FindNewMove((int X, int Y) coords)
        {
            var availableMoves = new List<(int, int)>();

            foreach(var move in Translations.AllMoves())
            {
                int newX = coords.X + move.X;
                int newY = coords.Y + move.Y;

                if(Validation.IsInsideBounds(Width, Height, (newX, newY)) && 
                Validation.IsLegalPos(Width, Height, GardenPortions[newY, newX])) {
                    availableMoves.Add(move);
                }
            }
            return availableMoves.Count > 0 ? availableMoves[_random.Next(availableMoves.Count)] : (0, 0);
        }

        public int GenerateGarden()
        {
            int perimCount = 0;

            GardenPortions = new GardenPortion[Height, Width];

            foreach((int X, int Y) stone in Stones)
            {
                GardenPortions[stone.Y, stone.X] = new Stone(stone);
            }

            for (int y = 0; y < GardenPortions.GetLength(0); y++)
            {
                for (int x = 0; x < GardenPortions.GetLength(1); x++)
                {
                    if (((y == 0 || y == Height - 1) && (x > 0 && x < Width - 1)) ||
                        ((x == 0 || x == Width - 1) && (y > 0 && y < Height - 1))) {
                        GardenPortions[y, x] = new PerimeterPortion((x, y), perimCount++);
                    }
                    else {
                        GardenPortions[y, x] = GardenPortions[y, x] ?? new GardenPortion((x, y));
                    }
                }
            }
            return perimCount + 1;
        }

        public void PrintGarden()
        {
            for (int y = 0; y < GardenPortions.GetLength(0); y++)
            {
                for (int x = 0; x < GardenPortions.GetLength(1); x++)
                {
                    if(GardenPortions[y, x].IsRaked()) {
                        Console.Write(((RakedPortion) GardenPortions[y, x]).RakeOrder + " ");
                    }
                    else {
                        Console.Write((char) GardenPortions[y, x].Label + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}