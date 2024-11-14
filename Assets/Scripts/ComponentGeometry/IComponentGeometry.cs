public enum CellType
{
    empty = 0,
    filled = 1,
    entrance = 2,
    exit = 3,
};

interface IComponentGeometry
{
    // Assume that start is coming from (0,0) and going to the right
    // Assume orientation is 0

    // End is the cell which can be connected to
    public (int, int) End;

    public (int, int) BoundingBox;

    public CellType[,] Cells;
}

class PuzzleDoor : IComponentGeometry
{
    public (int, int) End
    {
        get 
        {
            return (1,0);
        }
    }
}