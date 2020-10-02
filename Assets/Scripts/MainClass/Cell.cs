using UnityEngine;
namespace XAstar
{
    public class Cell
    {
        public Vector2 Position;

        public int F;
        public int G;
        public int H;

        public bool IsWalkable { get; private set; }

        public Cell ParentCell;


        public Cell(Vector2 position)
        {
            Position = position;
        }

        public void SetWalkablity(bool isWalk)
        {
            IsWalkable = isWalk;
        }
    }
}