using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    class Stone : GardenPortion
    {
        public Stone ((int, int) coords) : base(coords, GardenLabels.STONE) {}
    }
}