using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    [Serializable]
    class RakedPortion : GardenPortion
    {
        public RakedPortion Previous { get; set; } = null!;

        public int RakeOrder { get; private set; }

        public RakedPortion((int, int) coords, int order) : base(coords, GardenLabels.RAKED)
        {
            RakeOrder = order;
        }
    }
}