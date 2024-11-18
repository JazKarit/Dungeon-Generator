// public class RoomPuzzleGeometry
// {
//     public RoomPuzzleGeometry( int[,] cells, int x, int z)
//     {
//         this.cells = cells.Clone() as CellType[,];
//         this.x = x;
//         this.z = z;
//     }
//     // 0 = empty, 1 = filled, 2 = entrance, 3 = exit'
//     // entrance and exit can't be in the corner
//     public CellType [,] cells;

//     // number of ccw rotations to get to correct orientation
//     public int rotation = 0;

//     public int x;
//     public int z;

//     public (int, int) GetFirstLocationOfCellType(CellType cellType)
//     {
//         for (int i = 0; i < cells.GetLength(0); i++)
//         {
//             for (int j = 0; j < cells.GetLength(1); j++)
//             {
//                 return (i, j);
//             }
//         }
//         return (-1, -1);
//     }
// }