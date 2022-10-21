using ZenGarden.src.models;

namespace ZenGarden
{
    class Program
    {
        static void Main(string[] args)
        {
            Chromosome c = new Chromosome();

            foreach(int gene in c.Genes){
                Console.WriteLine(gene);
            }
        }
    }
}