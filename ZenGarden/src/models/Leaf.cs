using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    class Leaf : GardenPortion
    {
        public LeafColors Color { get; set; }

        public Leaf((int, int) coords) : base(coords, GardenLabels.LEAF) { }
    }
}