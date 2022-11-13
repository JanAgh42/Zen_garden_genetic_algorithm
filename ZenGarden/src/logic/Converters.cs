using System.Runtime.Serialization.Formatters.Binary;
using ZenGarden.src.constants;
using ZenGarden.src.models;

namespace ZenGarden.src.logic
{
    static class Converters
    {
        // returns the chosen perimeters ID based on the given gene
        public static int GetPerim(int perims, int gene)
        {
            double interval = 100.0 / (double) perims;

            for(int x = 1; x <= perims; x++)
            {
                if(gene <= Math.Round(x * interval)) {
                    return x - 1;
                }
            }
            return 0;
        }

        // choses the appropriate translation for the monk
        public static (int, int) GetMove(int width, int height, (int X, int Y) coords)
        {
            if (coords.Y == 0) {
                return Translations._MoveDown;
            }
            else if (coords.Y == height - 1) {
                return Translations._MoveUp;
            }
            else if (coords.X == 0) {
                return Translations._MoveRight;
            }
            else if (coords.X == width - 1) {
                return Translations._MoveLeft;
            }
            return (0, 0);
        }

        // choses the appropriate color for the console output
        public static ConsoleColor GetColor(LeafColors color)
        {
            switch(color)
            {
                case LeafColors.YELLOW:
                    return ConsoleColor.DarkYellow;
                case LeafColors.ORANGE:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.Red;
            }
        }

        // returns the chosen leaf color based on data loaded from the input files
        public static LeafColors GetLeafColor(string leaf)
        {
            switch (leaf) {
                case "Y":
                    return LeafColors.YELLOW;
                case "O":
                    return LeafColors.ORANGE;
                default:
                    return LeafColors.RED;
            }
        }

        // creates a deepclone of any given object
        public static T DeepClone<T>(T obj)
        {
            var memoryStream = new MemoryStream();
            var formatter = new BinaryFormatter();

            #pragma warning disable
            formatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;

            return (T) formatter.Deserialize(memoryStream);
            #pragma warning restore
        }
    }
}