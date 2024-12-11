// using System.Collections.Generic;
// using UnityEngine;

// class DungeonPrimative : IComponentGeometry
// {
//     private List<Door> doorways;

//     public bool [,] filledCells;

//     private Direction mapDirectionLocalE;

//     private (int x, int z) globalPos;

//     private bool isRendered = false;

//     private List<IComponentGeometry> adjacentComponents;

//     public List<IComponentGeometry> GetAdjacentComponents()
//     {
//         return adjacentComponents;
//     }

//     public void AddAdjComponent(IComponentGeometry component)
//     {
//         adjacentComponents.Add(component);
//     }

//     public void RemoveAdjComponent(int index)
//     {
//         adjacentComponents.RemoveAt(index);
//     }

//     public DungeonPrimative(Door localStartPosition, Door localEndPosition, bool [,] filledCells) 
//     {
//         this.localStartPosition = localStartPosition;
//         this.localEndPosition = localEndPosition;
//         this.filledCells = filledCells;
//     }

//     private (int, int) GetTransform(int x, int z)
//     {
//         int rot = (int)this.mapDirectionLocalE;
//         if (rot == 0)
//         {
//             return (z, x);
//         }
//         else if (rot == 1)
//         {
//             return (x, z);
//         }
//         else if (rot == 2)
//         {
//             return (-z, -x);
//         }
//         else if (rot == 3)
//         {
//             return (-x, -z);
//         }
//         else
//         {
//             throw new System.Exception("Invalid rotation");
//         }
//     }

//     private (int x, int z) GetGlobalCoordinates(int x, int z)
//     {
//         (int x, int z) localOriginToPointVector = GetTransform(x,z);
//         return (this.globalPos.x + localOriginToPointVector.x, this.globalPos.z + localOriginToPointVector.z);
//     }

//     public void PlaceStartAtGlobalLocation(Door entranceLocation)
//     {
//         int globalEntranceDir = (int)entranceLocation.direction;
//         int localEntranceDir = (int)this.localStartPosition.GetMirrorDoor().direction;
//         this.mapDirectionLocalE = (Direction)((globalEntranceDir + 4 - localEntranceDir) % 4);
        
//         (int x, int z) globalStartCell = entranceLocation.GetDestinationCell();

//         (int x, int z) localOriginToStartVector = GetTransform(this.localStartPosition.x, this.localStartPosition.z);
        
//         this.globalPos = (globalStartCell.x - localOriginToStartVector.x, globalStartCell.z - localOriginToStartVector.z);
//     }

//     public List<(int, int)> GetGlobalCellsCovered()
//     {
//         List<(int x, int z)> cells = new();
//         for (int x = 0; x < filledCells.GetLength(0); x++)
//         {
//             for (int z = 0; z < filledCells.GetLength(1); z++)
//             {
//                 if(filledCells[x,z])
//                 {
//                     cells.Add(GetGlobalCoordinates(x,z));
//                 }
//             }
//         }
//         return cells;
//     }

//     private Door GetGlobalStartLocation()
//     {
//         (int x, int z) globalStartCell = GetGlobalCoordinates(localStartPosition.GetStartCell().x,localStartPosition.GetStartCell().z);
//         return new Door(globalStartCell.x,globalStartCell.z,(Direction)(((int)localStartPosition.direction + (int)this.mapDirectionLocalE) % 4));
//     }

//     private Door GetGlobalEndLocation()
//     {
//         (int x, int z) globalEndCell = GetGlobalCoordinates(localEndPosition.GetStartCell().x,localEndPosition.GetStartCell().z);
//         return new Door(globalEndCell.x,globalEndCell.z,(Direction)(((int)localEndPosition.direction + (int)this.mapDirectionLocalE) % 4));
//     }

//     public List<Door> GetDoors()
//     {
//         return new List<Door> {GetGlobalStartLocation(), GetGlobalEndLocation()};
//     }

//     public List<Door> GetDoorways()
//     {
//         return new List<Door> {GetGlobalStartLocation(), GetGlobalEndLocation()};
//     }

//     public List<Door> GetDoorwaysWithoutDoors()
//     {
//         return new List<Door>();
//     }

//     public List<Door> GetEntrances()
//     {
//         return new List<Door> {GetGlobalStartLocation()};
//     }

//     public List<Door> GetExits()
//     {
//         return new List<Door> {GetGlobalEndLocation()};
//     }

//     public void Render()
//     {
//         if (!isRendered)
//         {
//             Color color = Color.HSVToRGB(Random.Range(0f,1f), 0.7f, 0.7f);
//             foreach ((int x, int z) cellCoord in GetGlobalCellsCovered())
//             {
//                 GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                 cell.transform.position = new Vector3(cellCoord.x, 0.5f, cellCoord.z);
//                 cell.transform.localScale = new Vector3(0.9f, 1, 0.9f);
//                 cell.GetComponent<Renderer>().material.color = color;
//             }

//             GetGlobalStartLocation().Render(Color.green);
//             GetGlobalEndLocation().Render(Color.red);
//             isRendered = true;
//         }
//     }
// }