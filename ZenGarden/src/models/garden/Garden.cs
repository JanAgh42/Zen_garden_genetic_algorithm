using ZenGarden.src.constants;
using ZenGarden.src.logic;

namespace ZenGarden.src.models
{
    [Serializable]
    class Garden
    {
        public GardenPortion[,] GardenPortions { get; private set; } = null!;
        public GardenPortion[,] FinalPortions { get; private set; } = null!;
        public List<LeafColors> LeafTypes { get; private set; } = null!;
        public List<Leaf> Leaves { get; private set; } = null!;
        public List<(int X, int Y)> Stones { get; private set; } = null!;
        
        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly Random _random = null!;

        public Garden(int width, int height, List<(int, int)> stones, List<Leaf> leaves)
        {
            Width = width;
            Height = height;
            Stones = stones;
            Leaves = leaves;

            _random = new Random();
        }

        private void RemoveLeaf((int X, int Y) coords)
        {
            var foundLeaf = Leaves.Find(leaf => leaf.X == coords.X && leaf.Y == coords.Y); 
            
            if (foundLeaf != null) {
                Leaves.Remove(foundLeaf);

                if (!AnyLeavesLeft() && LeafTypes.Count > 0) {
                    LeafTypes.RemoveAt(0);
                    this.TurnLeavesCollectable();
                }
            }
        }

        private void TurnLeavesCollectable()
        {
            Leaves.FindAll(leaf => leaf.Color == LeafTypes[0]).ForEach(leaf => leaf.TurnCollectable());
        }

        private bool AnyLeavesLeft()
        {
            var foundLeaves = Leaves.FindAll(leaf => leaf.Color == LeafTypes[0]);

            return foundLeaves.Count > 0;
        }

        public void CreateCopy()
        {
            FinalPortions = Converters.DeepClone<GardenPortion[,]>(GardenPortions);
        }

        public void RakeGardenPortion((int X, int Y) coords, int order)
        {
            if (GardenPortions[coords.Y, coords.X].IsLeaf()) {
                RemoveLeaf(coords);
            }
            GardenPortions[coords.Y, coords.X] = new RakedPortion(coords, order);
        }

        public (int X, int Y) GetPerimCoords(int perim)
        {
            var perimCoords = (0, 0);

            foreach (GardenPortion portion in GardenPortions)
            {
                if (portion.IsPerim() && ((PerimeterPortion)portion).Number == perim) {
                    perimCoords = ((PerimeterPortion) portion).GetCoords;
                    break;
                }
            }
            return perimCoords;
        }

        public (int, int) FindNewMove((int X, int Y) coords)
        {
            var availableMoves = new List<(int, int)>();

            foreach (var move in Translations.AllMoves())
            {
                int newX = coords.X + move.X;
                int newY = coords.Y + move.Y;

                if (Validation.IsInsideBounds(Width, Height, (newX, newY)) && 
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
            LeafTypes = new() { LeafColors.YELLOW, LeafColors.ORANGE, LeafColors.RED };

            this.TurnLeavesCollectable();

            Stones.ForEach(stone => GardenPortions[stone.Y, stone.X] = new Stone(stone));
            Leaves.ForEach(leaf => GardenPortions[leaf.Y, leaf.X] = leaf);

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
            for (int y = 1; y < FinalPortions.GetLength(0) - 1; y++)
            {
                for (int x = 1; x < FinalPortions.GetLength(1) - 1; x++)
                {
                    if (FinalPortions[y, x].IsRaked()) {
                        int numOfDigits = ((RakedPortion)FinalPortions[y, x]).RakeOrder.ToString().Length;
                        for (int z = numOfDigits; z < 2; z++)
                        {
                            Console.Write("0");
                        }
                        Console.Write(((RakedPortion) FinalPortions[y, x]).RakeOrder + " ");
                    }
                    else if (FinalPortions[y, x].IsLeaf()) {
                        Console.Write((char) FinalPortions[y, x].Label);
                        Console.Write((char) ((Leaf) FinalPortions[y, x]).Color + " ");
                    }
                    else {
                        Console.Write((char)FinalPortions[y, x].Label);
                        Console.Write((char) FinalPortions[y, x].Label + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}