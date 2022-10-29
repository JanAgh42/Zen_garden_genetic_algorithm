using System.Runtime.Serialization.Formatters.Binary;
using ZenGarden.src.constants;

namespace ZenGarden.src.logic
{
    static class Converters
    {
        public static int GetPerim(int wi, int he, int perims, int gene)
        {
            double interval = 50.0 / (double) perims;

            for(int x = 1; x <= perims; x++)
            {
                if(gene <= Math.Round(x * interval)) {
                    return x - 1;
                }
            }
            return 0;
        }

        public static (int, int) GetMove(int width, int height, (int X, int Y) coords)
        {
            if (coords.Y == 0){
                return Translations._MoveDown;
            }
            else if(coords.Y == height - 1) {
                return Translations._MoveUp;
            }
            else if(coords.X == 0) {
                return Translations._MoveRight;
            }
            else if(coords.X == width - 1) {
                return Translations._MoveLeft;
            }
            return (0, 0);
        }

        public static T DeepClone<T>(T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                formatter.Serialize(memoryStream, obj);
                memoryStream.Position = 0;

                return (T) formatter.Deserialize(memoryStream);
            }
        }
    }
}