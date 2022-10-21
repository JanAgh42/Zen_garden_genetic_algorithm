namespace ZenGarden.src.models
{
    class Coordinates
    {
        public int XCoord { get; protected set; }
        public int YCoord { get; protected set; }

        public Coordinates((int xCoord, int yCoord) coords)
        {
            XCoord = coords.xCoord;
            YCoord = coords.yCoord;
        }

        public (int, int) GetCoords => (XCoord, YCoord);
    }
}