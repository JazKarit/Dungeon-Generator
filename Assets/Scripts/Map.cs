using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


class Map
{

    // private int [,] cells;
    //private List<IComponentGeometry> components = new();
    public Dictionary<Guid,IComponentGeometry> components = new();

    public Graph graph = new();
    private List<Door> doors = new();
    
    private List<Door> walls = new();

    private List<(int,int)> cells = new();

    private Dictionary<(int,int),Guid> cellToComponent = new();

    //private List<Door> expandableDoorways;

    //private List<bool> expandableDoorwayHasDoor;

    //private List<List<int>> adjGraph;

    //private List<List<int>> adjDoors;
    
    public Map(int seedX, int seedY)
    {
        AddComponent(new CorridorCell(seedX,seedY));
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
        return !CellsCollideWithMap(cCells) && !DoorsCollideWithMap(component.GetDoors()) && !WallsCollideWithMap(component.GetDoors()) && !CreatesInvalidPuzzleConnection(component, true);
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

    /// <summary>
    /// Check if the new component would create a unneccesary puzzle. 
    /// </summary>
    /// <param name="component"></param>
    /// <param name="dissallowSecondNeighbors">Make function more strict so two puzzles cannot connect the same two nodes
    /// </param>
    /// <returns></returns>
    public bool CreatesInvalidPuzzleConnection(IComponentGeometry component, bool dissallowSecondNeighbors)
    {
                //if corridor, check that the corridor does not connect supercorridors that are already connected via puzzle room 
        if(component.GetType() == ComponentType.corridor)
        {
            return CorridorConnectsPuzzleConnectedSuperCorridors(component, dissallowSecondNeighbors);
        }
        //if puzzleroom, check that the puzzleroom start and end are not the same supercorridor 
        if(component.GetType() == ComponentType.puzzleRoom)
        {
            return PuzzleConnectsToSameSuperCorridor(component, dissallowSecondNeighbors);
        }
        return CorridorConnectsPuzzleConnectedSuperCorridors(component, dissallowSecondNeighbors) || PuzzleConnectsToSameSuperCorridor(component, dissallowSecondNeighbors);
    }

    //if corridor, check that the corridor does not connect supercorridors that are already connected via puzzle room 
    private bool CorridorConnectsPuzzleConnectedSuperCorridors(IComponentGeometry component, bool dissallowSecondNeighbors = false)
    {
        var adjComponents = GetAdjacentComponents(component);

        var adjSuperCorridors = new List<SuperCorridor>();

        
        foreach (var adjComponent in adjComponents)
        {
            bool alreadyAddedSuperCorridor = false;
            //UnityEngine.Debug.Log(adjComponent);
            //UnityEngine.Debug.Log(adjComponents);
            if (adjComponent.GetType() == ComponentType.superCorridor )
            {
                foreach (var sc in adjSuperCorridors)
                {
                    if(sc.Id == ((SuperCorridor)adjComponent).Id)
                    {
                        alreadyAddedSuperCorridor = true;
                        continue;
                    }
                }
                if(!alreadyAddedSuperCorridor)
                {
                    adjSuperCorridors.Add((SuperCorridor)adjComponent);
                }
            }
        }
        for (int i =0; i<adjSuperCorridors.Count-1;i++)
        {
            for (int k = i+1; k<adjSuperCorridors.Count;k++)
            {
                foreach (var neighbor in adjSuperCorridors[i].Neighbors)
                {
                    if(neighbor.Id == adjSuperCorridors[k].Id) return true;
                    
                    if (dissallowSecondNeighbors)
                    {
                        foreach (var secondNeighbor in neighbor.Neighbors)
                        {
                            if(secondNeighbor.Id == adjSuperCorridors[k].Id) return true;
                        }
                    }
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
    public bool PuzzleConnectsToSameSuperCorridor(IComponentGeometry component, bool dissallowSecondNeighbors = false)
    {          
        var doorways = component.GetDoorwaysWithDoors();
        var doorwayDestCell1 = doorways[0].GetDestinationCell();
        var doorwayDestCell2 = doorways[1].GetDestinationCell();
        if(!cellToComponent.ContainsKey(doorwayDestCell1)) return false;
        if(!cellToComponent.ContainsKey(doorwayDestCell2)) return false;
        var c1 = cellToComponent[doorwayDestCell1];
        var c2 = cellToComponent[doorwayDestCell2];
        if(c1 == c2) return true;

        if(dissallowSecondNeighbors)
        {
            //The super corridors already have a puzzle between them
            foreach(var neighbor in graph.GetNode(c1).Neighbors)
            {
                if(neighbor.Id == c2) return true;
            }
        }
        
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

    public bool AddComponent(IComponentGeometry component, int iteration = 1)
    {
        //for now either expecting a IComponentGeometry of type corridor or puzzleRoom

        
        
        

        if (!ValidityCheck(component))
        {
            return false;
        }
        



        if (component.GetType() == ComponentType.puzzleRoom)
        {
            Door endDoor = ((RoomPuzzle)component).GetGlobalEndLocation();

            // Auto add a cell on the other end of the puzzle room
            (var endCell_x, var endCell_z) = endDoor.GetDestinationCell(); 
            var newCell = new CorridorCell(endCell_x,endCell_z);
            bool success = AddComponent(newCell, iteration);
            if (success)
            {
                newCell.RemoveExpandableDoorway(endDoor.GetMirrorDoor());
                component.RemoveExpandableDoorway(endDoor);
            }
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
                ((INode)component).IterationCreated = iteration;
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

    /// <summary>
    /// Expand nodes. Chance to expand is inv. prop. to neibors.
    /// Chance to add a cell is (1/numCells) ^ (1/2) -> so 4 cells
    /// is equal chance to add puzzle room or cell
    /// </summary>
    /// <param name="iterations"></param>
    public void EST(int iterations)
    {
        
        // TODO: Maintain mincut 3 as graph is grown?
        for (int i = 0; i < iterations; i++)
        {
            List<INode> nodes = graph.GetAllNodes().ToList();
            float [] weights = new float[nodes.Count];

            for (int j = 0; j < nodes.Count; j++)
            {
                weights[j] = 1f / (1f + nodes[j].Neighbors.Count);
            }

            int expandIndex = WeightedRandom.GetWeightedRandomIndex(weights);
            
            var expandNode = nodes[expandIndex];

            var success = false;
            if(expandNode is SuperCorridor component)
            {
                int numCells = component.GetGlobalCellsCovered().Count;  
                
                float r = UnityEngine.Random.value;

                if (r <  Mathf.Exp(-0.04f*numCells))
                {
                    // Add Cell
                    var doorways = component.GetExpandableDoorways();
                    if (doorways.Count > 0)
                    {
                        var ind = UnityEngine.Random.Range(0,doorways.Count);
                        var cell = new CorridorCell();
                        cell.PlaceStartAtGlobalLocation(doorways[ind]);
                        success = AddComponent(cell);
                        if (!success)
                        {
                            var dest = doorways[ind].GetDestinationCell();
                            Debug.Log($"Failed to add cell at {dest} from {doorways[ind]} ");
                        }
                    }
                    
                } 
                else
                {
                    // Add PuzzleRoom

                    // Just changed this
                    var doorways = component.GetExpandableDoorwaysWithoutDoors();
                    if (doorways.Count > 0)
                    {
                        var ind = UnityEngine.Random.Range(0,doorways.Count);
                        bool [,] filledCells = {{true}};
                        var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,0,Direction.E), filledCells, new List<Door> { new Door(0,0,Direction.N),new Door(0,0,Direction.S)});
                        puzzleRoom.PlaceStartAtGlobalLocation(doorways[ind]);
                        success = AddComponent(puzzleRoom);
                        if (success)
                        {
                            component.RemoveExpandableDoorway(doorways[ind]);
                        }
                        if (!success)
                        {
                            var dest = doorways[ind].GetDestinationCell();
                            Debug.Log($"Failed to add puzzle room at {dest} from {doorways[ind]} ");
                        }
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log("Uh oh");
            }
        }
    }

    /// <summary>
    /// Expand nodes. 
    /// </summary>
    /// <param name="iterations"></param>
    public void KPIECE(int iterations, int startIteration)
    {
        
        // TODO: Maintain mincut 3 as graph is grown?
        for (int i = 0; i < iterations; i++)
        {
            List<INode> nodes = graph.GetAllNodes().ToList();
            float [] weights = new float[nodes.Count];

            for (int j = 0; j < nodes.Count; j++)
            {
                // todo add score?
                int iteration = nodes[j].IterationCreated;
                var expansions = nodes[j].GetNumberOfExpansions()+1f;
                var neighbors = nodes[j].Neighbors.Count+1f;
                float coverage =  nodes[j].GetCoverage();
                
                var weight = Mathf.Log(iteration + 1) / (expansions * neighbors * coverage);
                weights[j] = weight;

                //UnityEngine.Debug.Log($"Iteration: {iteration}, Expansions: {expansions}, Neighbors: {neighbors}, Coverage: {coverage}, Weight: {weight}");

            }

            int expandIndex = WeightedRandom.GetWeightedRandomIndex(weights);
            
            var expandNode = nodes[expandIndex];

            var success = false;
            if(expandNode is SuperCorridor component)
            {
                int numCells = component.GetGlobalCellsCovered().Count;  
                
                float r = UnityEngine.Random.value;

                if (r <  Mathf.Exp(-0.004f*numCells))
                {
                    // Add Cell
                    var doorways = component.GetExpandableDoorways();
                    if (doorways.Count > 0)
                    {
                        var ind = UnityEngine.Random.Range(0,doorways.Count);
                        var cell = new CorridorCell();
                        cell.PlaceStartAtGlobalLocation(doorways[ind]);
                        success = AddComponent(cell);
                        component.AddExpansion();
                        //}
                        if (!success)
                        {
                            var dest = doorways[ind].GetDestinationCell();
                            //Debug.Log($"Failed to add cell at {dest} from {doorways[ind]} ");
                        }
                    }
                    
                } 
                else
                {
                    // Add PuzzleRoom

                    // Just changed this
                    var doorways = component.GetExpandableDoorwaysWithoutDoors();
                    if (doorways.Count > 0)
                    {
                        var ind = UnityEngine.Random.Range(0,doorways.Count);
                        bool [,] filledCells = {{true}};
                        var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,0,Direction.E), filledCells, new List<Door> { new Door(0,0,Direction.N),new Door(0,0,Direction.S)});
                        puzzleRoom.PlaceStartAtGlobalLocation(doorways[ind]);
                        success = AddComponent(puzzleRoom, i + startIteration);
                        component.AddExpansion();
                        if (success)
                        {
                            Debug.Log("Added Puzzle Room");
                            component.RemoveExpandableDoorway(doorways[ind]);
                        }
                        if (!success)
                        {
                            var dest = doorways[ind].GetDestinationCell();
                            //Debug.Log($"Failed to add puzzle room at {dest} from {doorways[ind]} ");
                        }
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log("Uh oh");
            }
        }
    }

    public void RRT(int iterations, int startIteration)
    {  
        for(int i = 0; i<iterations; i++)
        {
        
            //generate random destination
            var range = 80;
            int randX = UnityEngine.Random.Range(-range,range);
            int randZ = UnityEngine.Random.Range(-range,range);
            UnityEngine.Debug.Log($"randX {randX}");
            UnityEngine.Debug.Log($"randZ {randZ}");

            //find closest cell
            (int closeX, int closeZ) =FindClosestCell(randX, randZ);
            var nodeGuid = cellToComponent[(closeX,closeZ)];
            var expandNode =components[nodeGuid];

            //determine expansion direction
            var direction = Direction.W;
            var xDiff = randX - closeX;
            var zDiff = randZ - closeZ;
            if(Mathf.Abs(xDiff)>Mathf.Abs(zDiff)) //expand in x direction
            {
                if(xDiff > 0) //expand +X dir
                {
                    direction = Direction.E;
                }
                else //expand -X dir
                {
                    direction = Direction.W;
                }
            }
            else //expand in z direction 
            {
                if(zDiff > 0) //expand +Z dir
                {
                    direction = Direction.N;
                }
                else //expand -Z dir
                {
                    direction = Direction.S;
                }
            }


            //direction randomizer
            int randDir = UnityEngine.Random.Range(0 ,4);
            direction = (Direction)randDir;

            Door door2expand = new Door(closeX,closeZ, direction);

            var success = false;
            if(expandNode is SuperCorridor component)
            {
                int numCells = component.GetGlobalCellsCovered().Count;  
                
                float r = UnityEngine.Random.value;

                if (r <  Mathf.Exp(-0.004f*numCells))
                {

                            var cell = new CorridorCell();
                            cell.PlaceStartAtGlobalLocation(door2expand);
                            success = AddComponent(cell);
                            component.AddExpansion();

                } 
                else
                {
                    // Add PuzzleRoom

                    bool [,] filledCells = {{true}};
                    var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,0,Direction.E), filledCells, new List<Door> { new Door(0,0,Direction.N),new Door(0,0,Direction.S)});
                    puzzleRoom.PlaceStartAtGlobalLocation(door2expand);
                    success = AddComponent(puzzleRoom, i + startIteration);
                    component.AddExpansion();
                    if (success)
                    {
                        Debug.Log("Added Puzzle Room");
                        component.RemoveExpandableDoorway(door2expand);
                    }

                }
            }
            else
            {
                UnityEngine.Debug.Log("Uh oh");
            }
        }

    }


    public void RRT_KPIECE(int iterations, int startIteration)
    {
        for (int i = 0; i < iterations; i++)
        {
            var rand = UnityEngine.Random.value;
            if (rand > 0.8)
            {
                RRT(1, i + startIteration);
            }
            else
            {
                KPIECE(1, i + startIteration);
            }
        }
    }
    

    public (int x, int z)  FindClosestCell(int x, int z)
    {
        //public Dictionary<Guid,IComponentGeometry> components = new();
        SuperCorridor closest = null;
        float minDistance = float.MaxValue;
        (float,float) target = (x,z);

        foreach (var component in components.Values)
        {
            if(component.GetType() == ComponentType.superCorridor)
            {
                float distance = GetDistance(((SuperCorridor)component).AvgPos(), target);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = (SuperCorridor)component;
                }
            }

        }

        List<(int x, int z)> closestCells = closest.GetGlobalCellsCovered();
        int closestX = 0;
        int closestZ = 0;
        minDistance = float.MaxValue;
        foreach (var closestCell in closestCells)
        {
            var nextX = (float)closestCell.x;
            var nextZ = (float)closestCell.z;
            float distance = GetDistance((nextX,nextZ), target);
            if (distance < minDistance)
                {
                    minDistance = distance;
                    closestX = closestCell.x;
                    closestZ = closestCell.z;
                }
        }

        return (closestX,closestZ); 
    }


    static float GetDistance((float x, float y) point1, (float x, float y) point2)
    {
        float dx = point1.x - point2.x;
        float dy = point1.y - point2.y;
        //return Math.Sqrt(dx * dx + dy * dy); //euclidean
        return Mathf.Abs(dx) + Mathf.Abs(dy); //manhattan
    }
}