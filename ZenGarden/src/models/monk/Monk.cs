namespace ZenGarden.src.models
{
    class Monk : Coordinates
    {
        public List<Chromosome> Chromosomes { get; private set; } = null!;
        
        private readonly int _numOfChromosomes = 100;

        public Monk (int genes) : base ((-1, -1))
        {
            Chromosomes = new List<Chromosome>(_numOfChromosomes);

            GenerateChromosomes(genes);
        }

        private void GenerateChromosomes(int genes)
        {
            for (int count = 0; count < _numOfChromosomes; count++)
            {
                Chromosomes.Add(new Chromosome(genes));
            }
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
    }
}