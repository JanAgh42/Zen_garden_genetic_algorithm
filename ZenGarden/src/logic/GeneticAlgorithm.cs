using ZenGarden.src.models;

namespace ZenGarden.src.logic
{
    class GeneticAlgorithm
    {
        public Garden Garden { get; set; }
        public Monk Monk { get; set; }

        public GeneticAlgorithm(int width, int height, List<(int, int)> stones)
        {
            Garden = new Garden(width, height, stones);
            Monk = new Monk(width + height + stones.Count);
        }

        public void ComputeFitness()
        {
            foreach (Chromosome entity in Monk.Chromosomes)
            {
                double fitness = 0;
                int order = 1;

                int perims = Garden.GenerateGarden();
                Garden.PrintGarden();
                foreach (int gene in entity.Genes)
                {
                    int perim = Converters.GetPerim(Garden.Width, Garden.Height, perims, gene);

                    var chosenPerim = Garden.GetPerimCoords(perim);
                    var move = Converters.GetMove(Garden.Width, Garden.Height, chosenPerim);

                    Monk.UpdateCoords(chosenPerim);
                    Monk.MoveMonkForward(move);

                    Console.WriteLine(perim);
                    Console.WriteLine(chosenPerim);
                    Console.WriteLine(move);
                    Console.WriteLine(Monk.GetCoords);

                    if(!Validation.IsLegalPos(Garden.Width, Garden.Height, Garden.GardenPortions[Monk.Y, Monk.X])) {
                        Monk.MoveMonkBackwards(move);
                        continue;
                    }
                    fitness += GeneratePath(move, order++);
                    if(order > 2)
                        break;   
                }
                Garden.PrintGarden();
                break;
                
            //         foreach (int gene in entity.Genes)
            //     {
            //         var startPos = Converters.GetPos(Garden.Width, Garden.Height, Garden.Stones.Count, gene);
            //         var startPortion = Garden.GardenPortions[startPos.coords.Item2, startPos.coords.Item1];
            //         if(!Validation.IsLegalPos(Garden.Width, Garden.Height, startPortion)) {
            //             Console.WriteLine(startPos.coords.Item2 + " " + startPos.coords.Item1);
            //             continue;
            //         }
            //         // Console.WriteLine(startPos.coords);
            //         // Console.WriteLine(startPos.coords.Item2);
            //         // Console.WriteLine(startPos.coords.Item1);
            //         // Console.WriteLine(startPortion.GetCoords);
            //         Monk.UpdateCoords(startPos.coords);

            //         //fitness += GeneratePath(startPos.move, ++order);
            //     }

            //     entity.FitnessValue = fitness;
            //     Garden.PrintGarden();
            //     break;
            }
        }

        private int GeneratePath((int X, int Y) move, int order)
        {
            int numOfPortions = 0;

            while(Validation.IsInsideBounds(Garden.Width, Garden.Height, Monk.GetCoords))
            {
                if(Validation.IsLegalPos(Garden.Width, Garden.Height, Garden.GardenPortions[Monk.Y,Monk.X])) {
                    Garden.RakeGardenPortion(Monk.GetCoords, order);
                    numOfPortions++;
                    Monk.MoveMonkForward(move);
                }
                else if(Validation.IsObstacle(Garden.GardenPortions[Monk.Y, Monk.X])) {
                    Monk.MoveMonkBackwards(move);

                    var nextMove = Garden.FindNewMove(Monk.GetCoords);

                    if(nextMove == (0, 0)) {
                        break;
                    }
                    Monk.MoveMonkForward(nextMove);
                }
            }
            return numOfPortions;
        }
    }
}