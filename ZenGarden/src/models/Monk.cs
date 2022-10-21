namespace ZenGarden.src.models
{
    class Monk : Coordinates
    {
        public List<Chromosome> Chromosomes { get; private set; } = null!;
        public int NumOfChromosomes { get; private set; }

        public Monk ((int, int) coords, int chromosomes) : base (coords)
        {
            NumOfChromosomes = chromosomes;
        }

        public void MoveMonk((int xCoord, int yCoord) translation)
        {
            XCoord += translation.xCoord;
            YCoord += translation.yCoord;
        }
    }
}