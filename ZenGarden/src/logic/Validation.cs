using ZenGarden.src.models;

namespace ZenGarden.src.logic
{
    static class Validation
    {
        public static bool IsLegalPos(int width, int height, GardenPortion newPos)
        {
            bool isEmpty = newPos.IsEmpty();
            bool isCollectableLeaf = newPos.IsLeaf() && ((Leaf) newPos).IsCollectable;
            bool isInsideBounds = newPos.X > 0 && newPos.X < width && newPos.Y > 0 && newPos.Y < height;

            return isEmpty || isCollectableLeaf || isInsideBounds;
        }
    }
}