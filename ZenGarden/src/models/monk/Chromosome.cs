namespace ZenGarden.src.models
{
    [Serializable]
    class Chromosome
    {
        public List<int> Genes { get; private set; } = null!;

        public int NumOfGenes { get; private set; }
        public double FitnessValue { get; set; } = -1;

        private readonly int _upperBound = 100;
        private readonly int _lowerBound = 0;

        private readonly Random _random;

        public Chromosome(bool generate, int genes = 6)
        {
            _random = new Random();
            Genes = new List<int>(genes);

            NumOfGenes = genes;

            if (generate) {
                GenerateGenes();
            }
        }

        // creates a Chromosome deepcopy
        public Chromosome CreateCopy(bool copyFitness)
        {
            return new Chromosome(false, NumOfGenes) {
                FitnessValue = copyFitness ? this.FitnessValue : -1,
                Genes = this.Genes
            };
        }

        // choose genes to mutate and perform mutations
        public void MutateGenes()
        {
            for (int counter = 0; counter < _random.Next(1, 4); counter++)
            {
                int genePos = _random.Next(0, NumOfGenes);
                int mutVal = _random.Next(-4, 5);

                if (mutVal == 0) {
                    mutVal++;
                }

                if (Genes[genePos] + mutVal <= _upperBound && Genes[genePos] - mutVal >= _lowerBound) {
                    Genes[genePos] += mutVal;
                }
            }
        }

        // generate genes with random values
        private void GenerateGenes()
        {
            for (int count = 0; count < NumOfGenes; count++)
            {
                Genes.Add(_random.Next(_lowerBound, _upperBound + 1));
            }
        }

        public bool HasFitness => FitnessValue > -1;
    }
}