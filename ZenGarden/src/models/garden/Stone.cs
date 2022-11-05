using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    [Serializable]
    class Stone : GardenPortion
    {
        public Stone((int, int) coords) : base(coords, GardenLabels.STONE) {}
    }
}