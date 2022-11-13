using ZenGarden.src.models;
using ZenGarden.src.constants;

namespace ZenGarden.src.logic
{
    class GeneticAlgorithm
    {
        public Garden Garden { get; set; }
        public Monk Monk { get; set; }

        public GeneticAlgorithm(int width, int height, List<(int, int)> stones, List<Leaf> leaves)
        {
            Garden = new Garden(width, height, stones, leaves);
            Monk = new Monk((width - 2) + (height - 2) + stones.Count);
        }

        public double ComputeFitness()
        {
            // iterating over every chromosome of the current generation
            foreach (Chromosome entity in Monk.Chromosomes)
            {
                if (entity.HasFitness) {
                    continue;
                }
                double portionFitness = 0;
                int order = 1, perimSum = 0;

                var perims = Garden.GenerateGarden();
                
                // iterating over every gene of the current chromosome
                foreach (int gene in entity.Genes)
                {
                    // determining the entry point and exact translation for the monk
                    var perim = Converters.GetPerim(perims, gene);
                    var chosenPerim = Garden.GetPerimCoords(perim);
                    var move = Converters.GetMove(Garden.Width, Garden.Height, chosenPerim);

                    perimSum += perim;

                    // moving the monk to the selected entry point
                    Monk.UpdateCoords(chosenPerim);
                    Monk.MoveMonkForward(move);

                    // if it is not a legal position, skip to next iteration
                    if (!Validation.IsLegalPos(Garden.Width, Garden.Height, Garden.GardenPortions[Monk.Y, Monk.X])) {
                        Monk.MoveMonkBackwards(move);
                        continue;
                    }
                    // if legal, generate a path starting from it
                    var pathDetails = GeneratePath(move, order++);

                    portionFitness += pathDetails.fitness;

                    // if the monk reaches a dead end, end execution
                    if (pathDetails.end) {
                        break;
                    }
                }
                // save the resulting fitness in the chromosome object
                entity.FitnessValue = Math.Round(portionFitness + 1.0 / (double) (order * 10) + perimSum * 0.00001, 5);

                if (entity.FitnessValue > Monk.MaxFitness) {
                    Monk.MaxFitness = entity.FitnessValue;
                    Garden.CreateCopy();
                }
            }
            // sort chromosomes by their fitness value and perform mutations on some of them
            Monk.SortByFitnessDescending();
            //Monk.PerformMutations();

            return Monk.MaxFitness;
        }

        // generates a path from start to finish
        private (int fitness, bool end) GeneratePath((int X, int Y) move, int order)
        {
            int numOfPortions = 0;
            bool deadEnd = false;

            // Move monk until he reaches the perimeter of the garden again
            while (Validation.IsInsideBounds(Garden.Width, Garden.Height, Monk.GetCoords))
            {
                if (Validation.IsLegalPos(Garden.Width, Garden.Height, Garden.GardenPortions[Monk.Y,Monk.X])) {
                    Garden.RakeGardenPortion(Monk.GetCoords, order);
                    numOfPortions++;
                    Monk.MoveMonkForward(move);
                }
                else if (Validation.IsObstacle(Garden.GardenPortions[Monk.Y, Monk.X])) {
                    Monk.MoveMonkBackwards(move);

                    move = Garden.FindNewMove(Monk.GetCoords);

                    // if the monk is stuck in a dead end, end execution
                    if (move == (0, 0)) {
                        deadEnd = true;
                        break;
                    }
                    Monk.MoveMonkForward(move);
                }
            }
            return (numOfPortions, deadEnd);
        }
    }
}