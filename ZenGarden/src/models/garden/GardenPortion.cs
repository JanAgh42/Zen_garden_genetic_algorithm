using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    class GardenPortion : Coordinates
    {
        public GardenLabels Label { get; set; }

        public GardenPortion((int, int) coords, GardenLabels label = GardenLabels.EMPTY) : base(coords)
        {
            Label = label;
        }

        public bool IsLeaf()
        {
            return this is Leaf;
        }

        public bool IsStone()
        {
            return this is Stone;
        }

        public bool IsRaked()
        {
            return this is RakedPortion;
        }

        public bool IsEmpty()
        {
            return this is GardenPortion;
        }
    }
}