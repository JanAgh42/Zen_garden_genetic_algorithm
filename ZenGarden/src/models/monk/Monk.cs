using ZenGarden.src.logic;

namespace ZenGarden.src.models
{
    class Monk : Coordinates
    {
        public List<Chromosome> Chromosomes { get; private set; } = null!;

        public double MaxFitness { get; set; } = 0;

        private readonly Random _random;
        private readonly int _numOfChromosomes = 80;
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
                newChromosomes.Add(new Chromosome(true, _numOfGenes));
                newChromosomes.Add(Chromosomes[0].CreateCopy(true));

                Chromosomes.RemoveAt(0);
                Chromosomes.RemoveAt(_numOfChromosomes - ((counter + 1) * 2));
            }

            for (int counter = 0; counter < (_numOfChromosomes - 20) / 2; counter++)
            {
                int firstWinner = 0, secondWinner = 0;

                if (counter < (_numOfChromosomes - 20) / 4) {
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

        // sorts all chromosomes according to their fitness
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

        //selects 20 chromosomes, which will be subjects to mutations
        public void PerformMutations()
        {
            for (int counter = 1; counter <= 20; counter++)
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

        // creates 2 new chromosomes based on their parents
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

        // randomly selects 2 chromosomes and returns the one with the highest fitness value
        private int TournamentSelection()
        {
            var participants = new List<int>(2);

            for (int counter = 0; counter < 2; counter++)
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

        // performs roulette selection and return the chosen chromosome
        private int RouletteSelection()
        {
            var participants = new List<int>(5);
            var probabilities = new List<double>(5);

            double totalFitness = 0, tempFitness = 0;

            // choses 5 chromosomes at random
            for (int counter = 0; counter < 5; counter++)
            {
                participants.Add(_random.Next(0, _numOfChromosomes - 20));
                totalFitness += Chromosomes[participants[counter]].FitnessValue;
            }

            int rouletteWinner = participants[0];

            // counts the probability of selection for each chosen chromosome
            participants.ForEach(index => probabilities.Add(Math.Round(Chromosomes[index].FitnessValue / totalFitness, 4)));

            double selection = Math.Round(_random.NextDouble(), 4);

            // selects the winner 
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