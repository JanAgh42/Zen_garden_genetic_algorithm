using ZenGarden.src.constants;

namespace ZenGarden.src.models
{
    class GardenPortion : Coordinates
    {
        public GardenLabels Label { get; set; }

        public GardenPortion((int, int) coords, GardenLabels label = GardenLabels.EMPTY) : base(coords) {
            Label = label;
        }
    }
}