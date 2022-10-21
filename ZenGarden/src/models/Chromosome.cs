namespace ZenGarden.src.models
{
    class Chromosome
    {
        public List<int> Genes { get; private set; } = null!;
        public int NumOfGenes { get; private set; }

        private readonly int _upperBound = 10;
        private readonly int _lowerBound = -10;

        private Random random;

        public Chromosome(int genes = 6)
        {
            Genes = new List<int>();
            random = new Random();
            NumOfGenes = genes;

            generateGenes();
        }

        private void generateGenes()
        {
            for(int count = 0; count < NumOfGenes; count++)
            {
                Genes.Add(random.Next(_lowerBound, _upperBound + 1));
            }
        }
    }
}