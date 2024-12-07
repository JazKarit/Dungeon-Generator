using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


class Map
{

    // private int [,] cells;
    //private List<IComponentGeometry> components = new();
    public Dictionary<Guid,IComponentGeometry> components = new();

    private Graph graph = new();
    private List<Door> doors = new();
    
    private List<Door> walls = new();

    private List<(int,int)> cells = new();

    private Dictionary<(int,int),Guid> cellToComponent = new();

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

    public Guid GetComponentIndexAt((int x, int z) cell)
    {
        if (cellToComponent.ContainsKey(cell))
        {
            return cellToComponent[cell];
        }
        return Guid.Empty;
    }

    public List<Door> GetAvailableDoorwaysWithoutDoor()
    {
        List<Door> doorways = new List<Door>();
        foreach(IComponentGeometry component in this.components.Values)
        {
            List<Door> cDoorways = component.GetDoorwaysWithoutDoors();
            foreach (Door door in cDoorways)
            {
                (int x, int z) cell = door.GetDestinationCell();

                // Ensure that destination cell is empty and that there is no door in the spot
                if (!CellCollidesWithMap(cell) && !DoorOrWallCollidesWithMap(door))
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
        foreach(IComponentGeometry component in this.components.Values)
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


    private bool DoorExistsInMap(Door door)
    {
        return ExistDoorInMap(new List<Door> {door});
    }

    private bool ExistDoorInMap(List<Door> cDoors)
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

    private bool DoorOrWallCollidesWithMap(Door door)
    {
        return DoorsAndWallsCollideWithMap(new List<Door> {door});
    }

    private bool DoorsAndWallsCollideWithMap(List<Door> cDoorsAndWalls)
    {
        foreach (Door door in cDoorsAndWalls)
        {
            foreach (Door mapDoor in doors)
            {
                if (door.IsEqual(mapDoor))
                {
                    return true;
                }
            }
            foreach (Door mapWall in walls)
            {
                if (door.IsEqual(mapWall))
                {
                    return true;
                }
            }
        }
        return false;
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
            
            foreach (Door mapWall in walls)
            {
                if (door.IsEqual(mapWall))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool WallsCollideWithMap(List<Door> walls)
    {
        foreach (Door wall in walls)
        {
            foreach (Door mapDoor in doors)
            {
                if (wall.IsEqual(mapDoor))
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
        var cDoorsAndWalls = component.GetDoors();
        cDoorsAndWalls.AddRange(component.GetWalls());
        var cCells = component.GetGlobalCellsCovered();
        
        //check that the cells don't collide with existing cells
        //check that doors don't conflict with walls and doors
        //check that walls con't conflict with doors
        //check CreatesInvalidPuzzleConnection
            ////if corridor, check that the corridor does not connect supercorridors that are already connected via puzzle room 
            ////if puzzleroom, check that the puzzleroom start and end are not the same supercorridor 
        // UnityEngine.Debug.Log(!CellsCollideWithMap(cCells));
        // UnityEngine.Debug.Log(!DoorsCollideWithMap(component.GetDoors()));
        // UnityEngine.Debug.Log(!WallsCollideWithMap(component.GetDoors()));
        // UnityEngine.Debug.Log(!CreatesInvalidPuzzleConnection(component));
        return !CellsCollideWithMap(cCells) && !DoorsCollideWithMap(component.GetDoors()) && !WallsCollideWithMap(component.GetDoors()) && !CreatesInvalidPuzzleConnection(component);
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
            if (cmpt is not null)
            {
                adjCmpts.Add(cmpt);
            }
        }

        return adjCmpts;
    }

    private List<Guid> GetAdjacentComponentIndexesWithoutDoor(IComponentGeometry component)
    {
        var adjCmpts = new List<Guid>();
        foreach (var doorway in component.GetDoorwaysWithoutDoors())
        {
            // Only return cmpnts connected with a doorway without a door
            if (!DoorOrWallCollidesWithMap(doorway)){
                var dest = doorway.GetDestinationCell();
                var cmpt = GetComponentIndexAt(dest);
                if (cmpt != Guid.Empty)
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

    private List<Guid> GetAdjacentComponentIndexesWithDoor(IComponentGeometry component)
    {
        var adjCmpts = new List<Guid>();
        foreach (var doorway in component.GetDoorwaysWithoutDoors())
        {
            // Only return cmpnts connected  through doors
            if (DoorExistsInMap(doorway)){
                var dest = doorway.GetDestinationCell();
                var cmpt = GetComponentIndexAt(dest);
                if (cmpt != Guid.Empty)
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
            if (cmpt != Guid.Empty)
            {
                adjCmpts.Add(cmpt);
            }
        }

        return adjCmpts;
    }

    private List<Guid> GetAdjacentComponentIndexesWithoutDoor(Guid componentInd)
    {
        return GetAdjacentComponentIndexesWithoutDoor(components[componentInd]);
    }

    public bool CreatesInvalidPuzzleConnection(IComponentGeometry component)
    {
        //if corridor, check that the corridor does not connect supercorridors that are already connected via puzzle room 
        if(component.GetType() == ComponentType.corridor)
        {
            return CorridorConnectsPuzzleConnectedSuperCorridors(component);
        }
        //if puzzleroom, check that the puzzleroom start and end are not the same supercorridor 
        if(component.GetType() == ComponentType.puzzleRoom)
        {
            return PuzzleConnectsToSameSuperCorridor(component);
        }
        return CorridorConnectsPuzzleConnectedSuperCorridors(component) || PuzzleConnectsToSameSuperCorridor(component);
    }

    //if corridor, check that the corridor does not connect supercorridors that are already connected via puzzle room 
    private bool CorridorConnectsPuzzleConnectedSuperCorridors(IComponentGeometry component)
    {
        var adjComponents = GetAdjacentComponents(component);

        var adjSuperCorridors = new List<SuperCorridor>();

        foreach (var adjComponent in adjComponents)
        {
            //UnityEngine.Debug.Log(adjComponent);
            //UnityEngine.Debug.Log(adjComponents);
            if (adjComponent.GetType() == ComponentType.superCorridor)
            {
                adjSuperCorridors.Add((SuperCorridor)adjComponent);
            }
        }
        for (int i =0; i<adjSuperCorridors.Count-1;i++)
        {
            for (int k = i+1; k<adjSuperCorridors.Count;k++)
            {
                foreach (var neighbor in adjSuperCorridors[i].Neighbors)
                {
                    if(neighbor.Id == adjSuperCorridors[k].Id) return true;
                }
            }
        }

        return false;
        
        /*
        var connectedComponents = GetConnectedComponents(component);
        var componentsAcrossADoor = new List<Guid>();

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
        */
    }

    //if puzzleroom, check that the puzzleroom start and end are not the same supercorridor 
    public bool PuzzleConnectsToSameSuperCorridor(IComponentGeometry component)
    {          
        var doorways = component.GetDoorwaysWithDoors();
        var doorwayDestCell1 = doorways[0].GetDestinationCell();
        var doorwayDestCell2 = doorways[1].GetDestinationCell();
        if(!cellToComponent.ContainsKey(doorwayDestCell1)) return false;
        if(!cellToComponent.ContainsKey(doorwayDestCell2)) return false;
        if(cellToComponent[doorwayDestCell1] == cellToComponent[doorwayDestCell2]) return true;
        return false;


        /*
        var doors = component.GetDoorwaysWithDoors();
        var adjCmpts = new List<IComponentGeometry>();
        foreach (var door in doors)
        {
            var dest = door.GetDestinationCell();
            var cmpt = GetComponentAt(dest);
            if (cmpt is not null)
            {
                var connections = GetConnectedComponents(cmpt);
                foreach (var connection in connections)
                {
                    // If we alraedy saw a connection of the component, they both connect to the puzzle we added
                    if (adjCmpts.Contains(connection))
                    {
                        return true;
                    }
                }
                adjCmpts.Add(cmpt);
            }
        }
        
        return false;
        */
    }

    public List<IComponentGeometry> GetConnectedComponents(IComponentGeometry component)
    {
        // var connectedComponents = new List<IComponentGeometry> {component};

        // var indecies = GetConnectedComponentIndecies(component);

        // foreach (Guid index in indecies)
        // {
        //     connectedComponents.Add(components[index]);
        // }

        // // TODO: find out why this is neccesary
        // return connectedComponents.Distinct().ToList();

        return new List<IComponentGeometry>();
    }

    private List<Guid> GetConnectedComponentIndecies(IComponentGeometry component)
    {
        // var connectedComponents = new List<Guid>();
        // var newConnections = GetAdjacentComponentIndexesWithoutDoor(component);

        // foreach (var connection in newConnections)
        // {
        //     if (!connectedComponents.Contains(connection))
        //     {
        //         connectedComponents.Add(connection);
        //     }
        // }
        
        // foreach (var connection in newConnections)
        // {
        //     var extendedConnection = GetConnectedComponents(connection, connectedComponents);
        //     foreach (var c in extendedConnection)
        //     {
        //         if (!connectedComponents.Contains(c))
        //         {
        //             connectedComponents.Add(connection);
        //         }
        //     }
        // }

        //return connectedComponents;

        return new List<Guid>();
    }

    private List<int> GetConnectedComponents(int currCmpt, List<int> connectedComponents)
    {
        // var newConnections = GetAdjacentComponentIndexesWithoutDoor(currCmpt);
        // var connectionsToAdd = new List<int>();

        // foreach (var connection in newConnections)
        // {
        //     if (connectedComponents.Contains(connection))
        //     {
        //         continue;   
        //     }
        //     connectionsToAdd.Add(connection);
        // }

        // connectedComponents.AddRange(connectionsToAdd);
        
        // foreach (var connection in connectionsToAdd)
        // {
        //     connectedComponents.AddRange(GetConnectedComponents(connection, connectedComponents));
        // }

        // return connectedComponents;

        return new List<int> ();
    }

    public bool AddComponent(IComponentGeometry component)
    {
        //for now either expecting a IComponentGeometry of type corridor or puzzleRoom

        
        
        

        if (!ValidityCheck(component))
        {
            return false;
        }
        



        if (component.GetType() == ComponentType.puzzleRoom)
        {
            (var endCell_x, var endCell_z) = ((RoomPuzzle)component).GetGlobalEndLocation().GetDestinationCell(); 
            var newCell = new CorridorCell(endCell_x,endCell_z);
            AddComponent(newCell);
            if (!ValidityCheck(component))
            {
                return false;
            }

            
            var doorways = component.GetDoorwaysWithDoors();
            var doorwayDestCell1 = doorways[0].GetDestinationCell();
            var doorwayDestCell2 = doorways[1].GetDestinationCell();
            if (cellToComponent.ContainsKey(doorwayDestCell1) && cellToComponent.ContainsKey(doorwayDestCell2))
            {
                graph.AddEdge(cellToComponent[doorwayDestCell1],cellToComponent[doorwayDestCell2]);            
            }
            else
            {
                return false;
            }
        }

        var cDoors = component.GetDoors();
        var cCells = component.GetGlobalCellsCovered();
        var cWalls = component.GetWalls();
        doors.AddRange(cDoors);
        cells.AddRange(cCells);
        walls.AddRange(cWalls);

        if (component.GetType() == ComponentType.corridor)
        {
            List<SuperCorridor> adjSuperCorridors = new List<SuperCorridor>();
            foreach (Door doorway in component.GetDoorways())
            {
                var newCell = doorway.GetDestinationCell();
                IComponentGeometry adjComponent = GetComponentAt(newCell);
                
                if (adjComponent is not null && adjComponent.GetType() == ComponentType.superCorridor && !adjSuperCorridors.Contains(adjComponent))
                {
                    adjSuperCorridors.Add((SuperCorridor)adjComponent);
                }
            }
                


            //if its not part of a supercorridor (no corridors adjacent, just a door), make a new supercorridor
            if (adjSuperCorridors.Count == 0)
            {
                component = new SuperCorridor(component);
                graph.AddNode((SuperCorridor)component);
            }
            //if it touches 1 supercorridor, it becomes part of that supercorridor
            else if (adjSuperCorridors.Count == 1)
            {
                adjSuperCorridors[0].AddComponent(component);
                component = adjSuperCorridors[0];
            }
             //if it connects 2+ supercorridors, merge the corridors
            else
            {
                adjSuperCorridors[0].AddComponent(component);
                for(int i = 1; i < adjSuperCorridors.Count; i++)
                {
                    adjSuperCorridors[0].AddComponent((adjSuperCorridors[i]));
                    components.Remove(adjSuperCorridors[i].Id);


                    graph.RemoveNode(adjSuperCorridors[i].Id);

                    //would want to merge nodes and remove isolated nodes 
                }
                component = adjSuperCorridors[0];
            }
        }

        foreach ((int,int) cCell in component.GetGlobalCellsCovered())
        {
            cellToComponent[cCell] = component.Id;
        }

        components[component.Id] = component;

        //Debug.Log(components.Values.Count(c => c.GetType() == ComponentType.puzzleRoom));
        return true;
    }

    public void Render()
    {

        foreach (IComponentGeometry component in components.Values)
        {
            List<GameObject> gameObjs = new List<GameObject>();
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


    // Planner must add corridor cells off of puzzles

    public void RandomExpansion(int iterations)
    {
        // TODO: Maintain mincut 3 as graph is grown?
        for (int i = 0; i < iterations; i++)
        {
            int r;
            List<Door> empytDoorways = GetAvailableDoorwaysWithoutDoor();

            if(empytDoorways.Count > 0)
            {
                r = UnityEngine.Random.Range(0,empytDoorways.Count);
                bool [,] filledCells = {{true}};
                var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,0,Direction.E), filledCells, new List<Door> { new Door(0,0,Direction.N),new Door(0,0,Direction.S)});
                
                
                // bool [,] filledCells = {{true,false},{true,true}};
                // var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,1,Direction.E), filledCells);
                //bool [,] filledCells = {{true,true,false,true,true,true,true},{true,true,true,true,false,true,true}};
                //var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,6,Direction.E), filledCells);
                
                puzzleRoom.PlaceStartAtGlobalLocation(empytDoorways[r]);

                AddComponent(puzzleRoom);
                //AddComponent(new PuzzleDoor(empytDoorways[r]));


            }
        
            List<Door> doorways = GetAvailableDoorways();

            if (doorways.Count > 0)
            {
                r = UnityEngine.Random.Range(0,doorways.Count);
                var cell = new CorridorCell();
                cell.PlaceStartAtGlobalLocation(doorways[r]);

                
                AddComponent(cell);
            }
        }
    }

     public void EST(int iterations)
    {
        // TODO: Maintain mincut 3 as graph is grown?
        for (int i = 0; i < iterations; i++)
        {
            int r;
            List<Door> empytDoorways = GetAvailableDoorwaysWithoutDoor();

            if(empytDoorways.Count > 0)
            {
                r = UnityEngine.Random.Range(0,empytDoorways.Count);
                bool [,] filledCells = {{true}};
                var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,0,Direction.E), filledCells, new List<Door> { new Door(0,0,Direction.N),new Door(0,0,Direction.S)});
                
                
                // bool [,] filledCells = {{true,false},{true,true}};
                // var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,1,Direction.E), filledCells);
                //bool [,] filledCells = {{true,true,false,true,true,true,true},{true,true,true,true,false,true,true}};
                //var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,6,Direction.E), filledCells);
                
                puzzleRoom.PlaceStartAtGlobalLocation(empytDoorways[r]);
                AddComponent(puzzleRoom);
                //AddComponent(new PuzzleDoor(empytDoorways[r]));


            }
        
            List<Door> doorways = GetAvailableDoorways();

            if (doorways.Count > 0)
            {
                r = UnityEngine.Random.Range(0,doorways.Count);
                var cell = new CorridorCell();
                cell.PlaceStartAtGlobalLocation(doorways[r]);
                AddComponent(cell);
            }
        }
    }
}