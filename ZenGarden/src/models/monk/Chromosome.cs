namespace ZenGarden.src.models
{
    [Serializable]
    class Chromosome
    {
        public List<int> Genes { get; private set; } = null!;

        public int NumOfGenes { get; private set; }
        public double FitnessValue { get; set; } = -1;

        private readonly int _upperBound = 50;
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

        public Chromosome CreateCopy(bool copyFitness)
        {
            return new Chromosome(false, NumOfGenes) {
                FitnessValue = copyFitness ? this.FitnessValue : -1,
                Genes = this.Genes
            };
        }

        public void MutateGenes()
        {
            for (int counter = 0; counter < _random.Next(1, 3); counter++)
            {
                int genePos = _random.Next(0, NumOfGenes);
                int mutVal = _random.Next(-1, 2);

                if (Genes[genePos] + mutVal <= _upperBound && Genes[genePos] - mutVal >= _lowerBound) {
                    Genes[genePos] += mutVal;
                }
            }
        }

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