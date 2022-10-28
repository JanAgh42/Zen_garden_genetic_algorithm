using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    class PerimeterPortion : GardenPortion
    {
        public int Number { get; private set; }

        public PerimeterPortion((int, int) coords, int number) : base(coords, GardenLabels.PERIM)
        {
            Number = number;
        }
    }
}