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
            Monk = new Monk(width + height + stones.Count);
        }

        public void ComputeFitness()
        {
            foreach (Chromosome entity in Monk.Chromosomes)
            {
                if (entity.HasFitness) {
                    continue;
                }
                double fitness = 0;
                int order = 1, perimSum = 0;

                var perims = Garden.GenerateGarden();
                
                foreach (int gene in entity.Genes)
                {
                    var perim = Converters.GetPerim(Garden.Width, Garden.Height, perims, gene);
                    var chosenPerim = Garden.GetPerimCoords(perim);
                    var move = Converters.GetMove(Garden.Width, Garden.Height, chosenPerim);

                    perimSum += perim;

                    Monk.UpdateCoords(chosenPerim);
                    Monk.MoveMonkForward(move);

                    if (!Validation.IsLegalPos(Garden.Width, Garden.Height, Garden.GardenPortions[Monk.Y, Monk.X])) {
                        Monk.MoveMonkBackwards(move);
                        continue;
                    }
                    var pathDetails = GeneratePath(move, order++);

                    fitness += pathDetails.fitness;

                    if (pathDetails.end) {
                        break;
                    }
                }
                entity.FitnessValue = Math.Round(fitness + 1.0 / (double) (order * 10) + perimSum * 0.00001, 5);

                if (entity.FitnessValue > Monk.MaxFitness) {
                    Monk.MaxFitness = entity.FitnessValue;
                    Garden.CreateCopy();
                }
            }
            Monk.SortByFitnessDescending();
            Monk.PerformMutations();
        }

        private (int fitness, bool end) GeneratePath((int X, int Y) move, int order)
        {
            int numOfPortions = 0;
            bool deadEnd = false;

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