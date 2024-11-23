using System.Collections.Generic;
using System.Linq;
using UnityEngine;


class Map
{

    // private int [,] cells;
    private List<IComponentGeometry> components = new();

    private List<Door> doors = new();

    private List<(int,int)> cells = new();

    private Dictionary<(int,int),int> cellToComponent = new();

    //private List<Door> expandableDoorways;

    //private List<bool> expandableDoorwayHasDoor;

    //private List<List<int>> adjGraph;

    //private List<List<int>> adjDoors;
    
    public Map()
    {
        
    }

    public IComponentGeometry GetComponentAt((int x, int z) cell)
    {
        if (cellToComponent.ContainsKey(cell))
        {
            return components[cellToComponent[cell]];   
        }
        return null;
    }

    public int GetComponentIndexAt((int x, int z) cell)
    {
        if (cellToComponent.ContainsKey(cell))
        {
            return cellToComponent[cell];
        }
        return -1;
    }

    public List<Door> GetAvailableDoorwaysWithoutDoor()
    {
        List<Door> doorways = new List<Door>();
        foreach(IComponentGeometry component in this.components)
        {
            List<Door> cDoorways = component.GetDoorwaysWithoutDoors();
            foreach (Door door in cDoorways)
            {
                (int x, int z) cell = door.GetDestinationCell();

                // Ensure that destination cell is empty and that there is no door in the spot
                if (!CellCollidesWithMap(cell) && !DoorCollidesWithMap(door))
                {
                    doorways.Add(door);
                }
            }
        }
        return doorways;
    }

    // public List<Door> GetAvailableDoorwaysWithDoor()
    // {
    //     List<Door> doors = new List<Door>();
    //     foreach(IComponentGeometry component in this.components)
    //     {
    //         doors.AddRange(component.GetAvailableDoorwaysWithDoor());
    //     }
    //     return doors;
    // }

    public List<Door> GetAvailableDoorways()
    {
        List<Door> doorways = new List<Door>();
        foreach(IComponentGeometry component in this.components)
        {
            List<Door> cDoorways = component.GetDoorways();
            foreach (Door door in cDoorways)
            {
                (int x, int z) cell = door.GetDestinationCell();

                // Ensure that destination cell is empty
                if (!CellCollidesWithMap(cell))
                {
                    doorways.Add(door);
                }
            }
        }
        return doorways;
    }

    private bool CellCollidesWithMap((int x, int z) cell)
    {
        return CellsCollideWithMap(new List<(int, int)> {cell});
    }

    private bool CellsCollideWithMap(List<(int x, int z)> cCells)
    {
        foreach ((int x, int z) cell in cCells)
        {
            foreach ((int x, int z) mapCell in cells)
            {
                if (cell.x == mapCell.x && cell.z == mapCell.z)
                {
                    return true;
                }
            }
        }
        return false;
    }


    private bool DoorCollidesWithMap(Door door)
    {
        return DoorsCollideWithMap(new List<Door> {door});
    }

    private bool DoorsCollideWithMap(List<Door> cDoors)
    {
        foreach (Door door in cDoors)
        {
            foreach (Door mapDoor in doors)
            {
                if (door.IsEqual(mapDoor))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool ValidityCheck(IComponentGeometry component)
    {
        // TODO Add blocking doors
        var cDoors = component.GetDoors();
        var cCells = component.GetGlobalCellsCovered();
        
        return !CellsCollideWithMap(cCells) && !DoorsCollideWithMap(cDoors) && !ConnectedComponentsConnectAcrossPuzzle(component);
    }


    // private void SetAdjacentComponents(IComponentGeometry component)
    // {
    //     var cDoors = component.GetDoors();
    //     var cCells = component.GetGlobalCellsCovered();

    //     var adjDoors = new List<Door>();

    //     foreach (Door door in cDoors)
    //     {
    //         for (int i = 0; i < expandableDoorways.Length(); i++)
    //         {
    //             if(door.IsEqual(expandableDoorways[i]))
    //             {
    //                 //List<int,int> connectCell = expandableDoorways.GetStartCell();
    //                 //cells.getIndexOf(connectCell)
    //                 expandableDoorways.RemoveAt[i];
    //                 i--;
    //             }
    //         }
    //         foreach (Door exDoor in expandableDoorways)
    //         {
                
    //         }
    //     }
    // }

    

    public List<IComponentGeometry> GetAdjacentComponents(IComponentGeometry component)
    {
        var adjCmpts = new List<IComponentGeometry>();
        foreach (var doorway in component.GetDoorways())
        {
            var dest = doorway.GetDestinationCell();
            var cmpt = GetComponentAt(dest);
            adjCmpts.Add(cmpt);
        }

        return adjCmpts;
    }

    private List<int> GetAdjacentComponentIndexesWithoutDoor(IComponentGeometry component)
    {
        var adjCmpts = new List<int>();
        foreach (var doorway in component.GetDoorwaysWithoutDoors())
        {
            // Only return cmpnts connected with a doorway without a door
            if (!DoorCollidesWithMap(doorway)){
                var dest = doorway.GetDestinationCell();
                var cmpt = GetComponentIndexAt(dest);
                if (cmpt != -1)
                {
                    var otherDoorways = components[cmpt].GetDoorways();
                    foreach (var otherDoorway in otherDoorways)
                    {
                        if (otherDoorway.IsEqual(doorway))
                        {
                            adjCmpts.Add(cmpt);
                            break;
                        }
                    }
                }
            }
        }

        return adjCmpts;
    }

    private List<int> GetAdjacentComponentIndexesWithDoor(IComponentGeometry component)
    {
        var adjCmpts = new List<int>();
        foreach (var doorway in component.GetDoorwaysWithoutDoors())
        {
            // Only return cmpnts connected  through doors
            if (DoorCollidesWithMap(doorway)){
                var dest = doorway.GetDestinationCell();
                var cmpt = GetComponentIndexAt(dest);
                if (cmpt != -1)
                {
                    adjCmpts.Add(cmpt);
                }
            }
        }

        // Only return cmpnts connected  through doors
        foreach (var doorway in component.GetDoorwaysWithDoors())
        {
            var dest = doorway.GetDestinationCell();
            var cmpt = GetComponentIndexAt(dest);
            if (cmpt != -1)
            {
                adjCmpts.Add(cmpt);
            }
        }

        return adjCmpts;
    }

    private List<int> GetAdjacentComponentIndexesWithoutDoor(int componentInd)
    {
        return GetAdjacentComponentIndexesWithoutDoor(components[componentInd]);
    }

    public bool ConnectedComponentsConnectAcrossPuzzle(IComponentGeometry component)
    {
        var connectedComponents = GetConnectedComponents(component);
        var componentsAcrossADoor = new List<int>();

        var cells = component.GetGlobalCellsCovered();


        foreach (var connection in connectedComponents)
        {
            var newComponentsAcrossADoor = GetAdjacentComponentIndexesWithDoor(connection);

            foreach (var acrossDoorComponent in newComponentsAcrossADoor)
            {
                if (componentsAcrossADoor.Contains(acrossDoorComponent))
                {
                    return true;
                }
                componentsAcrossADoor.Add(acrossDoorComponent);
            }
        }
        
        return false;
    }

    public List<IComponentGeometry> GetConnectedComponents(IComponentGeometry component)
    {
        var connectedComponents = new List<IComponentGeometry> {component};

        var indecies = GetConnectedComponentIndecies(component);

        foreach (int index in indecies)
        {
            connectedComponents.Add(components[index]);
        }

        // TODO: find out why this is neccesary
        return connectedComponents.Distinct().ToList();
    }

    private List<int> GetConnectedComponentIndecies(IComponentGeometry component)
    {
        var connectedComponents = new List<int>();
        var newConnections = GetAdjacentComponentIndexesWithoutDoor(component);

        foreach (var connection in newConnections)
        {
            if (!connectedComponents.Contains(connection))
            {
                connectedComponents.Add(connection);
            }
        }
        
        foreach (var connection in newConnections)
        {
            var extendedConnection = GetConnectedComponents(connection, connectedComponents);
            foreach (var c in extendedConnection)
            {
                if (!connectedComponents.Contains(c))
                {
                    connectedComponents.Add(connection);
                }
            }
        }

        return connectedComponents;
    }

    private List<int> GetConnectedComponents(int currCmpt, List<int> connectedComponents)
    {
        var newConnections = GetAdjacentComponentIndexesWithoutDoor(currCmpt);
        var connectionsToAdd = new List<int>();

        foreach (var connection in newConnections)
        {
            if (connectedComponents.Contains(connection))
            {
                continue;   
            }
            connectionsToAdd.Add(connection);
        }

        connectedComponents.AddRange(connectionsToAdd);
        
        foreach (var connection in connectionsToAdd)
        {
            connectedComponents.AddRange(GetConnectedComponents(connection, connectedComponents));
        }

        return connectedComponents;
    }

    public bool AddComponent(IComponentGeometry component)
    {

        int componentIndex = components.Count;

        component.SetIndex(componentIndex);

        

        if (!ValidityCheck(component))
        {
            return false;
        }

        var cDoors = component.GetDoors();
        var cCells = component.GetGlobalCellsCovered();

       // SetAdjacentComponents(component);
    



        doors.AddRange(cDoors);
        cells.AddRange(cCells);

        

        // for (int i = 0; i < cDoors.Count; i++)
        // {
        //     getComponentForDoor.Add(componentIndex);
        // }

        foreach ((int,int) cCell in cCells)
        {
            cellToComponent[cCell] = componentIndex;
        }

        components.Add(component);

        return true;
    }

    public void Render()
    {
        foreach (IComponentGeometry component in components)
        {
            component.Render();
        }
    }

    // private void RenderCell(int x, int z)
    // {
    //     GameObject room = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //     room.transform.position = new Vector3(x, 0.5f, z);
    //     room.transform.localScale = new Vector3(0.9f, 1, 0.9f);
    //     room.GetComponent<Renderer>().material.color = Color.white;
    // }

    // private void RenderComponent(IComponentGeometry geometry)
    // {
    //     foreach(Door door in geometry.GetDoors())
    //     {
    //         door.Render(Color.black);
    //     }

    //     foreach((int,int) cell in geometry.GetGlobalCellsCovered())
    //     {
    //         RenderCell(cell.Item1,cell.Item2);
    //     }
    // }

    public void RandomExpansion(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            int r;
            List<Door> empytDoorways = GetAvailableDoorwaysWithoutDoor();

            if(empytDoorways.Count > 0)
            {
                r = Random.Range(0,empytDoorways.Count);
                // bool [,] filledCells = {{true,false},{true,true}};
                // var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,1,Direction.E), filledCells);
                bool [,] filledCells = {{true,true,false,true,true,true,true},{true,true,true,true,false,true,true}};
                var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,6,Direction.E), filledCells);
                puzzleRoom.PlaceStartAtGlobalLocation(empytDoorways[r]);
                AddComponent(puzzleRoom);
                //AddComponent(new PuzzleDoor(empytDoorways[r]));
            }
        
            List<Door> doorways = GetAvailableDoorways();

            if (doorways.Count > 0)
            {
                r = Random.Range(0,doorways.Count);
                var cell = new CorridorCell();
                cell.PlaceStartAtGlobalLocation(doorways[r]);
                AddComponent(cell);
            }
        }
    }
}