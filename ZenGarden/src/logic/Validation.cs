using ZenGarden.src.models;

namespace ZenGarden.src.logic
{
    static class Validation
    {
        public static bool IsLegalPos(int width, int height, GardenPortion newPos)
        {
            bool isEmpty = newPos.IsEmpty();
            bool isCollectableLeaf = newPos.IsLeaf() ? ((Leaf) newPos).IsCollectable : false;

            return isEmpty || isCollectableLeaf;
        }

        public static bool IsInsideBounds(int width, int height, (int X, int Y) position)
        {
            return position.X > 0 && position.X < width - 1 && position.Y > 0 && position.Y < height - 1;
        }

        public static bool IsObstacle(GardenPortion newPos)
        {
            bool isCollectableLeaf = newPos.IsLeaf() ? ((Leaf)newPos).IsCollectable : true;

            return newPos.IsStone() || newPos.IsRaked() || !isCollectableLeaf;
        }
    }
}