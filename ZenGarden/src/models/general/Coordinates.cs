namespace ZenGarden.src.models
{
    [Serializable]
    class Coordinates
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public Coordinates((int, int) coords)
        {
            UpdateCoords(coords);
        }

        public void UpdateCoords((int xCoord, int yCoord) coords)
        {
            X = coords.xCoord;
            Y = coords.yCoord;
        }

        public (int, int) GetCoords => (X, Y);
    }
}