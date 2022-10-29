using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    [Serializable]
    class Leaf : GardenPortion
    {
        public LeafColors Color { get; private set; }
        
        public bool IsCollectable { get; private set; }

        public Leaf((int, int) coords, LeafColors color) : base(coords, GardenLabels.LEAF)
        {
            IsCollectable = false;
            Color = color;
        }

        public void TurnCollectable()
        {
            IsCollectable = !IsCollectable;
        }
    }
}