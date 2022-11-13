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
        public List<Leaf> CurrentLeaves { get; private set; } = null!;

        public List<(int X, int Y)> Stones { get; private set; } = null!;

        public double NumOfEmptyPortions { get; private set; }
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

        // removes leaf at given position and turns leaves of other colors collectable if possible
        private void RemoveLeaf((int X, int Y) coords)
        {
            var foundLeaf = CurrentLeaves.Find(leaf => leaf.GetCoords == coords);

            if (foundLeaf == null) {
                return;
            }
            CurrentLeaves.Remove(foundLeaf);

            if (AnyLeavesLeft() || LeafTypes.Count <= 0) {
                return;
            }
            LeafTypes.RemoveAt(0);
            this.TurnLeavesCollectable();
        }

        private void TurnLeavesCollectable()
        {
            CurrentLeaves.FindAll(leaf => leaf.Color == LeafTypes[0]).ForEach(leaf => leaf.TurnCollectable());
        }

        // checks if there are any remaining leaves of the given color
        private bool AnyLeavesLeft()
        {
            var foundLeaves = CurrentLeaves.FindAll(leaf => leaf.Color == LeafTypes[0]);

            return foundLeaves.Count > 0;
        }

        // creates and stores a clone of the current garden
        public void CreateCopy()
        {
            FinalPortions = Converters.DeepClone<GardenPortion[,]>(GardenPortions);
        }

        // turn the monks current position int RakedPortion
        public void RakeGardenPortion((int X, int Y) coords, int order)
        {
            if (GardenPortions[coords.Y, coords.X].IsLeaf()) {
                RemoveLeaf(coords);
            }
            GardenPortions[coords.Y, coords.X] = new RakedPortion(coords, order);
        }

        // returns the perimeters coordinates based on its ID
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

        // finds a new translation when the monk reaches an obstacle
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

        // fills up the garden array with objects of different types
        public int GenerateGarden()
        {
            int perimCount = 0;
            
            GardenPortions = new GardenPortion[Height, Width];
            
            LeafTypes = new() { LeafColors.YELLOW, LeafColors.ORANGE, LeafColors.RED };
            CurrentLeaves = Converters.DeepClone<List<Leaf>>(Leaves);

            // inserts Stone and Leaf objects into the garden array
            Stones.ForEach(stone => GardenPortions[stone.Y, stone.X] = new Stone(stone));
            CurrentLeaves.ForEach(leaf => GardenPortions[leaf.Y, leaf.X] = leaf);

            NumOfEmptyPortions = (Width - 2) * (Height - 2) - Stones.Count;

            this.TurnLeavesCollectable();

            // fills up the remaining empty spaces in the garden
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

        // draws the final garden in the console and formats the output
        public void PrintGarden()
        {
            for (int y = 1; y < FinalPortions.GetLength(0) - 1; y++)
            {
                for (int x = 1; x < FinalPortions.GetLength(1) - 1; x++)
                {
                    if (FinalPortions[y, x].IsRaked()) {
                        int rakeOrder = ((RakedPortion)FinalPortions[y, x]).RakeOrder;
                        int numOfDigits = rakeOrder.ToString().Length;

                        Console.BackgroundColor = ((ConsoleColor) (rakeOrder % 15) + 1);

                        for (int z = numOfDigits; z < 2; z++)
                        {
                            Console.Write("0");
                        }
                        Console.Write(rakeOrder + " ");
                    }
                    else if (FinalPortions[y, x].IsLeaf()) {
                        Console.ForegroundColor = Converters.GetColor(((Leaf) FinalPortions[y, x]).Color);
                        Console.Write((char) FinalPortions[y, x].Label);
                        Console.Write((char) ((Leaf) FinalPortions[y, x]).Color + " ");
                    }
                    else {
                        Console.Write((char)FinalPortions[y, x].Label);
                        Console.Write((char) FinalPortions[y, x].Label + " ");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }
    }
}