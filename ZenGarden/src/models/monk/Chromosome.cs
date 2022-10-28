namespace ZenGarden.src.models
{
    class Chromosome
    {
        public List<int> Genes { get; private set; } = null!;

        public int NumOfGenes { get; private set; }
        public double FitnessValue { get; set; }

        private readonly int _upperBound = 50;
        private readonly int _lowerBound = 0;

        private readonly Random _random;

        public Chromosome(int genes = 6)
        {
            Genes = new List<int>(genes);
            _random = new Random();
            NumOfGenes = genes;

            GenerateGenes();
        }

        private void GenerateGenes()
        {
            for(int count = 0; count < NumOfGenes; count++)
            {
                Genes.Add(_random.Next(_lowerBound, _upperBound + 1));
            }
        }

        public void MutateGenes()
        {
            
        }
    }
}