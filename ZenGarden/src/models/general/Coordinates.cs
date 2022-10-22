namespace ZenGarden.src.models
{
    class Coordinates
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public Coordinates((int xCoord, int yCoord) coords)
        {
            X = coords.xCoord;
            Y = coords.yCoord;
        }

        public (int, int) GetCoords => (X, Y);
    }
}