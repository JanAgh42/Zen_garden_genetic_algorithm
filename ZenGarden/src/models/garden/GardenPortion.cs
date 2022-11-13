using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    [Serializable]
    class GardenPortion : Coordinates
    {
        public GardenLabels Label { get; set; }

        public GardenPortion((int, int) coords, GardenLabels label = GardenLabels.EMPTY) : base(coords)
        {
            Label = label;
        }

        // methods used to determine which derived type does the given object belong to
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

        public bool IsPerim()
        {
            return this is PerimeterPortion;
        }

        public bool IsEmpty()
        {
            return !IsLeaf() && !IsStone() && !IsRaked() && !IsPerim();
        }
    }
}