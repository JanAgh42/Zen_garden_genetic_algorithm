using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    class RakedPortion : GardenPortion
    {
        public RakedPortion((int, int) coords) : base(coords, GardenLabels.RAKED) {}
    }
}