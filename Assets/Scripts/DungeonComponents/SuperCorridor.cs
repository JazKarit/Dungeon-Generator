using System.Collections.Generic;
using static System.Exception;
using System.Linq;
using UnityEngine;
using System;


public class SuperCorridor : IComponentGeometry, INode
{ 
   //for IComponentGeometry
    private List<CorridorCell> cells;
    private List<(int x, int z)> globalCellsCovered;
    private List<(int x, int z)> renderedCells;
    private List<Door> doors;
    private List<Door> doorways;
    private List<Door> doorwaysWithoutDoors;
    private List<Door> doorwaysWithDoors;
    private List<Door> walls;
    private List<Door> entrances;
    private List<Door> exits;

    private List<Door> expandableDoorways;

    private List<Door> expandableDoorwaysWithoutDoors;

    private List<Door> unexpandableDoorwaysForCells;

    private List<Door> unexpandableDoorwaysForPuzzleRooms;

    public List<GameObject> cubes;
    private int index;

    private (float x, float z) avgPos;

    public Color color;
   
   //for INode
    public Guid Id {get; set;} // Unique identifier for the node
    public List<INode> Neighbors {get; set;} // Connections to other nodes

    //for KPIECE
    public int IterationCreated {get; set;}

    private int numExpansions = 0;

    public SuperCorridor(IComponentGeometry firstComponent)
    {
        globalCellsCovered = new List<(int, int)>();
        doors= new List<Door> ();
        doorways= new List<Door> ();
        doorwaysWithoutDoors= new List<Door> ();
        doorwaysWithDoors= new List<Door> ();
        walls= new List<Door> ();
        entrances= new List<Door> ();
        exits= new List<Door> ();
        expandableDoorways = new List<Door> ();
        expandableDoorwaysWithoutDoors = new List<Door> ();
        unexpandableDoorwaysForCells = new List<Door> ();
        unexpandableDoorwaysForPuzzleRooms = new List<Door> ();
        IterationCreated = 1;
        


        index= -1;

        Id = Guid.NewGuid();
        Neighbors = new  List<INode>();
        cubes = new List<GameObject>();

        color = Color.HSVToRGB(UnityEngine.Random.Range(0f,1f), 0.3f, 0.4f);
        //color = Color.white;
        this.AddComponent(firstComponent);
        renderedCells = new List<(int x, int z)>();
    }

    public void AddComponent(IComponentGeometry nextComponent)
    {        
        if (nextComponent.Id == Id)
        {
            return;
        }
        globalCellsCovered.AddRange(nextComponent.GetGlobalCellsCovered());
        doors.AddRange(nextComponent.GetDoors());
        doorways.AddRange(nextComponent.GetDoorways());
        doorwaysWithoutDoors.AddRange(nextComponent.GetDoorwaysWithoutDoors());
        doorwaysWithDoors.AddRange(nextComponent.GetDoorwaysWithDoors());
        walls.AddRange(nextComponent.GetWalls());
        entrances.AddRange(nextComponent.GetEntrances());
        exits.AddRange(nextComponent.GetExits());
        if(nextComponent is INode n)
        {
            IterationCreated = Mathf.Min(IterationCreated,n.IterationCreated);
        }
        
        

        // Find where the new component connects to our current one,
        // These are no longer expandable
        List<Door> doorwaysToRemove = new List<Door>();
        foreach (var doorway in expandableDoorways)
        {
            foreach (var cDoorway in nextComponent.GetExpandableDoorways())
            {
                if (doorway == cDoorway)
                {
                    doorwaysToRemove.Add(doorway);
                }
            }
        }

        expandableDoorways.AddRange(nextComponent.GetExpandableDoorways());
        expandableDoorwaysWithoutDoors.AddRange(nextComponent.GetExpandableDoorwaysWithoutDoors());

        foreach (var doorway in doorwaysToRemove)
        {
            RemoveExpandableDoorway(doorway);
        }

        if (nextComponent.GetType() == ComponentType.superCorridor)
        {
            Neighbors.AddRange(((SuperCorridor)nextComponent).Neighbors);
            cubes.AddRange(((SuperCorridor)nextComponent).cubes);
            renderedCells.AddRange(((SuperCorridor)nextComponent).renderedCells);
        }
        avgPos = ((float)globalCellsCovered.Average(item => (double)item.x),(float)globalCellsCovered.Average(item => (double)item.z));

    }


    
    public void PlaceStartAtGlobalLocation(Door location)
    {
        Debug.Log("Cannot change Super corridor start");
    }

    public List<(int, int)> GetGlobalCellsCovered()
    {
        return globalCellsCovered;
    }



    public List<Door> GetDoors()
    {
        return doors;
    }

    public List<Door> GetDoorways()
    {
        return doorways;
    }

    //public List<Door> GetExpandableDoors();

    public List<Door> GetDoorwaysWithoutDoors()
    {
        return doorwaysWithoutDoors;
    }
    public List<Door> GetDoorwaysWithDoors()
    {
        return doorwaysWithDoors;
    }

    public List<Door> GetWalls()
    {
        return walls;
    }

    public List<Door> GetEntrances()
    {
        return entrances;
    }

    public List<Door> GetExits()
    {
        return exits;
    }

    // public bool RemoveDoorway(Door remDoorway)
    // {
    //     // remove it from doorwaysWithoutDoors as well if it didn't have a door
    //     for (doorway in doorwaysWithoutDoors)
    //     {
    //         if(doorway.IsEqual(remDoorway))
    //         {
    //             doors.Remove(door);
    //         }
    //     }

    //     for (doorway in doorways)
    //     {
    //         if(doorway.IsEqual(remDoorway))
    //         {
    //             doors.Remove(doorway);
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    //TODO: optimize to hold this data in a better way
    public List<Door> GetAvailableDoorways()
    {
        List<Door> doorways = new List<Door>();
        foreach (Door doorway in GetDoorways())
        {
            (int x, int z) cell = doorway.GetDestinationCell();

            // Ensure that destination cell is empty
            if (!globalCellsCovered.Contains(cell))
            {
                doorways.Add(doorway);
            }
        }
        return doorways;
    }

    //TODO: optimize to hold this data in a better way
    public List<Door> GetAvailableDoorwaysWithoutDoor()
    {
        foreach (Door door in GetDoorwaysWithoutDoors())
        {
            (int x, int z) cell = door.GetDestinationCell();

            // Ensure that destination cell is empty and that there is no door in the spot
            if (!globalCellsCovered.Contains(cell) && !doors.Contains(door) && !walls.Contains(door))
            {
                doorways.Add(door);
            }
        }
        return doorways;
    }

    public void Render()
    {
        foreach(var cell in globalCellsCovered)
        {
            if(!renderedCells.Contains(cell))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(cell.x, 0.5f, cell.z);
                cube.transform.localScale = new Vector3(0.9f, 1, 0.9f);
                cube.GetComponent<Renderer>().material.color = color;
                renderedCells.Add(cell);
            }
        }
    }

    public void SetIndex(int i)
    {
        this.index = i;
    }

    public int GetIndex()
    {
        return this.index;
    }

    public ComponentType GetType()
    {
        return ComponentType.superCorridor;
    }

    public void AddNeighbor(INode neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
        }
    }

    public int GetNumberOfExpansions()
    {
        return numExpansions;
    }
    public void AddExpansion()
    {
        numExpansions++;
    }

    // TODO: see if this is good or not
    public float GetCoverage()
    {
        return Mathf.Log(GetGlobalCellsCovered().Count + 1);
    }

    public override string ToString()
    {
        float avgX = (float)globalCellsCovered.Average(item => (double)item.x);
        float avgZ = (float)globalCellsCovered.Average(item => (double)item.z);

        return $"Super Corridor: ({avgX},{avgZ}) - {globalCellsCovered.Count} cells"; 
    }

    public (float x, float z)  AvgPos()
    {

        return avgPos; 
    }

    public string ToStringWithNeighbors()
    {
        var neighbors = string.Join(", ", Neighbors.Select(n => n.ToString()));
        return $"Node {ToString()}: [{neighbors}]";
    }

    public List<Door> GetExpandableDoorways()
    {
        List<Door> doorways = new List<Door>();
        List<Door> doorwaysToRemove = new List<Door>();
        foreach (Door doorway in expandableDoorways)
        {
            (int x, int z) cell = doorway.GetDestinationCell();

            // Ensure that destination cell is empty
            if (!globalCellsCovered.Contains(cell))
            {
                doorways.Add(doorway);
            }
        }
        
        // Remove failures while we're here
        foreach (Door doorway in doorwaysToRemove)
        {
            expandableDoorwaysWithoutDoors.RemoveAll(d => d.IsEqual(doorway));
            expandableDoorways.RemoveAll(d => d.IsEqual(doorway));
        }
        
        return doorways;
    }

    // public List<Door> GetExpandableDoorways(ComponentType type)
    // {
    //     if (type == ComponentType.co)
    // }

    public List<Door> GetExpandableDoorwaysWithoutDoors()
    {
        List<Door> doorways = new List<Door>();
        List<Door> doorwaysToRemove = new List<Door>();
        foreach (Door doorway in expandableDoorwaysWithoutDoors)
        {
            (int x, int z) cell = doorway.GetDestinationCell();

            // Ensure that destination cell is empty
            if (!globalCellsCovered.Contains(cell))
            {
                doorways.Add(doorway);
            }
        }
        
        // Remove failures while we're here
        foreach (Door doorway in doorwaysToRemove)
        {
            expandableDoorwaysWithoutDoors.RemoveAll(d => d.IsEqual(doorway));
            expandableDoorways.RemoveAll(d => d.IsEqual(doorway));
        }
        
        return doorways;
    }

    public void RemoveExpandableDoorway(Door door)
    {
        expandableDoorways.RemoveAll(d => d.IsEqual(door));
        expandableDoorwaysWithoutDoors.RemoveAll(d => d.IsEqual(door));
    }

    // public void RemoveExpandableDoorway(Door door, ComponentType type)
    // {
    //     if (type == ComponentType.corridor || type == ComponentType.superCorridor)
    //     {
    //         unexpandableDoorwaysForCells.Add(door);
    //     }
    //     else if (type == ComponentType.puzzleRoom)
    //     {
    //         unexpandableDoorwaysForPuzzleRooms.Add(door);
    //     }
    // }
}


