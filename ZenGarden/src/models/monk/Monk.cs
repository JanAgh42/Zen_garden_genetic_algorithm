using ZenGarden.src.logic;

namespace ZenGarden.src.models
{
    class Monk : Coordinates
    {
        public List<Chromosome> Chromosomes { get; private set; } = null!;

        public double MaxFitness { get; set; } = 0;

        private readonly Random _random;
        private readonly int _numOfChromosomes = 70;
        private readonly int _numOfGenes;

        public Monk (int genes = 6) : base ((0, 0))
        {
            _random = new Random();
            Chromosomes = new List<Chromosome>(_numOfChromosomes);

            _numOfGenes = genes;

            GenerateChromosomes(genes);
        }

        public void CreateNewGeneration()
        {
            var newChromosomes = new List<Chromosome>(_numOfChromosomes);

            for (int counter = 0; counter < 10; counter++)
            {
                newChromosomes.Add(Chromosomes[0].CreateCopy(true));

                Chromosomes.RemoveAt(0);
                Chromosomes.RemoveAt(_numOfChromosomes - ((counter + 1) * 2));
            }

            for (int counter = 0; counter < (_numOfChromosomes - 10) / 2; counter++)
            {
                var firstWinner = 0;
                var secondWinner = 0;

                if (counter < (_numOfChromosomes - 10) / 4) {
                    firstWinner = TournamentSelection();
                    secondWinner = TournamentSelection();
                }
                else {
                    firstWinner = RouletteSelection();
                    secondWinner = RouletteSelection();
                }

                var children = PerformCrossover(firstWinner, secondWinner);

                newChromosomes.Add(children.first);
                newChromosomes.Add(children.second);
            }
            Chromosomes = newChromosomes;
        }

        public void SortByFitnessDescending()
        {
            Chromosomes = Chromosomes.OrderByDescending(chromosome => chromosome.FitnessValue).ToList();
        }

        public void MoveMonkForward((int xCoord, int yCoord) translation)
        {
            X += translation.xCoord;
            Y += translation.yCoord;
        }

        public void MoveMonkBackwards((int xCoord, int yCoord) translation)
        {
            X -= translation.xCoord;
            Y -= translation.yCoord;
        }

        public void PerformMutations()
        {
            for (int counter = 1; counter <= 10; counter++)
            {
                var chosenChromosome = _random.Next(1, _numOfChromosomes);

                Chromosomes[chosenChromosome].MutateGenes();
            }
        }

        private void GenerateChromosomes(int genes)
        {
            for (int count = 0; count < _numOfChromosomes; count++)
            {
                Chromosomes.Add(new Chromosome(true, genes));
            }
        }

        private (Chromosome first, Chromosome second) PerformCrossover(int first, int second)
        {
            var firstEntity = Chromosomes[first].CreateCopy(false);
            var secondEntity = Chromosomes[second].CreateCopy(false);

            var crossoverBegin = _random.Next(0, _numOfGenes / 2);
            var crossoverEnd = _random.Next(_numOfGenes / 2, _numOfGenes);

            for (int counter = crossoverBegin; counter <= crossoverEnd; counter++)
            {
                var temp = firstEntity.Genes[counter];
                firstEntity.Genes[counter] = secondEntity.Genes[counter];
                secondEntity.Genes[counter] = temp;
            }
            return (firstEntity, secondEntity);
        }

        private int TournamentSelection()
        {
            var participants = new List<int>(5);

            for (int counter = 0; counter < 5; counter++)
            {
                participants.Add(_random.Next(0, _numOfChromosomes - 20));
            }

            int tournamentWinner = participants[0];

            foreach (int index in participants)
            {
                if (Chromosomes[index].FitnessValue > Chromosomes[tournamentWinner].FitnessValue) {
                    tournamentWinner = index;
                }
            }
            return tournamentWinner;
        }

        private int RouletteSelection()
        {
            var participants = new List<int>(5);
            var probabilities = new List<double>(5);

            double totalFitness = 0;
            double tempFitness = 0;

            for (int counter = 0; counter < 5; counter++)
            {
                participants.Add(_random.Next(0, _numOfChromosomes - 20));
                totalFitness += Chromosomes[participants[counter]].FitnessValue;
            }

            int rouletteWinner = participants[0];

            foreach (int index in participants)
            {
                probabilities.Add(Math.Round(Chromosomes[index].FitnessValue / totalFitness, 4));
            }

            double selection = Math.Round(_random.NextDouble(), 4);

            for (int counter = 0; counter < 5; counter++)
            {
                tempFitness += probabilities[counter];

                if (selection <= tempFitness) {
                    rouletteWinner = participants[counter];
                    break;
                }
            }
            return rouletteWinner;
        }
    }
}