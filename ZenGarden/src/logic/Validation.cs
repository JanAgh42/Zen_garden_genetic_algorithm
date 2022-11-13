using ZenGarden.src.models;

namespace ZenGarden.src.logic
{
    static class Validation
    {
        // checks if the monk can move to a given spot
        public static bool IsLegalPos(int width, int height, GardenPortion newPos)
        {
            bool isEmpty = newPos.IsEmpty();
            bool isCollectableLeaf = newPos.IsLeaf() ? ((Leaf) newPos).IsCollectable : false;

            return isEmpty || isCollectableLeaf;
        }

        // checks if the monk is still inside the gardens bounds
        public static bool IsInsideBounds(int width, int height, (int X, int Y) position)
        {
            return position.X > 0 && position.X < width - 1 && position.Y > 0 && position.Y < height - 1;
        }

        // checks if the monks current position is an obstacle or not
        public static bool IsObstacle(GardenPortion newPos)
        {
            bool isCollectableLeaf = newPos.IsLeaf() ? ((Leaf)newPos).IsCollectable : true;

            return newPos.IsStone() || newPos.IsRaked() || !isCollectableLeaf;
        }
    }
}