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
            // for(int y = -1; y <= he; y++)
            // {
            //     for(int x = -1; x <= wi; x++)
            //     {
            //         if (((y == -1 || y == he) && (x > -1 && x < wi)) ||
            //             ((x == -1 || x == wi) && (y > -1 && y < he))) {
            //             counter++;
            //         }
            //         if(counter == interval) {
            //             if(y == -1) {
            //                 move = Translations._MoveDown;
            //                 y = 0;
            //             }
            //             else if(y == he) {
            //                 move = Translations._MoveUp;
            //                 y = he - 1;
            //             }
            //             else if(x == -1) {
            //                 move = Translations._MoveRight;
            //                 x = 0;
            //             }
            //             else if(x == wi) {
            //                 move = Translations._MoveLeft;
            //                 x = wi - 1;
            //             }
            //             return ((x, y), move);
            //         }
            //     }
            // }
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
    }
}