namespace ZenGarden.src.constants
{
    static class Translations
    {
        public static readonly (int xCoord, int yCoord) _MoveDown = (1, 0);
        public static readonly (int xCoord, int yCoord) _MoveUp = (-1, 0);
        public static readonly (int xCoord, int yCoord) _MoveLeft = (0, -1);
        public static readonly (int xCoord, int yCoord) _MoveRight = (0, 1);

        public static List<(int, int)> AllMoves ()
        {
            return new List<(int xCoord, int yCoord)> {
                _MoveDown,
                _MoveUp,
                _MoveLeft,
                _MoveRight
            };
        }
    }
}