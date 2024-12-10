using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

// class BossRoom : IComponentGeometry
// {
        
//     public Guid Id { get;  } // Unique identifier for the node

//     public void PlaceStartAtGlobalLocation(Door location);

//     public List<(int x, int z)> GetGlobalCellsCovered();

//     public List<Door> GetDoors();

//     public List<Door> GetDoorways();

//     public List<Door> GetExpandableDoorways();

//     public List<Door> GetExpandableDoorwaysWithoutDoors();

//     public void RemoveExpandableDoorway(Door door);

//     /// <summary>
//     /// Only remove the doorway from expansion for a certain ComponentType.
//     /// </summary>
//     /// <param name="door">Doorway to remove</param>
//     /// <param name="type">Type of component to disallow</param>
//     //public void RemoveExpandableDoorway(Door door, ComponentType type);

//     public List<Door> GetDoorwaysWithoutDoors();
//     public List<Door> GetDoorwaysWithDoors();

//     public List<Door> GetWalls();

//     public List<Door> GetEntrances();

//     public List<Door> GetExits();

//     public void Render();



//     //public void SetIndex(int i);

//     //public int GetIndex();

//     public ComponentType GetType();
// }


class BossRoom : IComponentGeometry, INode
{
    private (int x, int z) centerPos;

    private List<(int x, int z)> globalCellsCovered;

    private bool isRendered = false;

    private List<IComponentGeometry> adjacentComponents;

    private List<Door> walls;
    private List<Door> doors;

    private List<Door> unexpandableDoorways;

    public List<INode> Neighbors {get; set;}

   //for INode
    public Guid Id { get; set; } // Unique identifier for the node

    private Color color;

    private int numExpansions = 0;

    public int IterationCreated { get; set; }

    
    public BossRoom((int x, int z) centerPos, int radius = 5) 
    {
        Id = Guid.NewGuid();
        IterationCreated = 1;
        color = Color.white;
        this.centerPos = centerPos;
        
        this.unexpandableDoorways = new List<Door>();
        this.doors = new List<Door>();
        this.walls = new List<Door>();
        this.globalCellsCovered = new List<(int x, int z)>();
        this.Neighbors = new List<INode>();
        
        for (int i = centerPos.x - radius; i <= centerPos.x + radius; i++)
        {
            for (int j = centerPos.z - radius; j <= centerPos.z + radius; j++)
            {
                this.globalCellsCovered.Add((i,j));
            }
        }

        for (int i = centerPos.x - radius; i <= centerPos.x + radius; i++)
        {
            if(i == centerPos.x)
            {
                doors.Add(new Door(i, centerPos.z - radius, Direction.S));
                doors.Add(new Door(i, centerPos.z + radius, Direction.N));
            }
            else
            {
                walls.Add(new Door(i, centerPos.z - radius, Direction.S));
                walls.Add(new Door(i, centerPos.z + radius, Direction.N));
            }
        }

        for (int j = centerPos.z - radius; j <= centerPos.z + radius; j++)
        {
            if(j == centerPos.z)
            {
                doors.Add(new Door(centerPos.x - radius, centerPos.z, Direction.W));
                doors.Add(new Door(centerPos.x + radius, centerPos.z, Direction.E));
            }
            else
            {
                walls.Add(new Door(centerPos.x - radius, centerPos.z, Direction.W));
                walls.Add(new Door(centerPos.x + radius, centerPos.z, Direction.E));
            }
        }
        Id = Guid.NewGuid();
    }

    public override string ToString()
    {
        float avgX = (float)GetGlobalCellsCovered().Average(item => (double)item.x);
        float avgZ = (float)GetGlobalCellsCovered().Average(item => (double)item.z);

        return $"Boss Room: ({avgX},{avgZ},{centerPos}) - {GetGlobalCellsCovered().Count} cells"; 
    }

    public void PlaceStartAtGlobalLocation(Door entranceLocation)
    {
        centerPos = (entranceLocation.x, entranceLocation.z);
    }

    public List<(int x, int z)> GetGlobalCellsCovered()
    {
        return globalCellsCovered;
    }

    public List<Door> GetDoors()
    {
        return doors;
    }

    public List<Door> GetDoorways()
    {
        return new List<Door>();    
    }

    public List<Door> GetDoorwaysWithoutDoors()
    {
        return new List<Door>();
    }

    public List<Door> GetDoorwaysWithDoors()
    {
        return doors;
    }

    public List<Door> GetEntrances()
    {
        return doors;
    }

    public List<Door> GetExits()
    {
        return doors;
    }

    public List<Door> GetWalls()
    {
        return walls;
    }

    public void Render()
    {
        if (!isRendered)
        {
            foreach ((int x, int z) cellCoord in GetGlobalCellsCovered())
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.position = new Vector3(cellCoord.x, 0.5f, cellCoord.z);
                cell.transform.localScale = new Vector3(0.9f, 1, 0.9f);
                cell.GetComponent<Renderer>().material.color = color;
            }

            foreach (var door in GetDoors())
            {
                door.Render(Color.green);
            }
        }
    }

    public ComponentType GetType()
    {
        return ComponentType.bossRoom;
    }

    public List<Door> GetExpandableDoorways()
    {
        List<Door> doorways = GetDoorways();
        foreach (Door door in unexpandableDoorways)
        {
            doorways.RemoveAll(d => d.IsEqual(door));
        }
        return doorways;
    }

    public List<Door> GetExpandableDoorwaysWithoutDoors()
    {
        return new List<Door>();
    }

    public void RemoveExpandableDoorway(Door door)
    {
        unexpandableDoorways.Add(door);
    }


    public void AddNeighbor(INode neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
        }
    }

    public void RemoveNeighbor(INode neighbor)
    {
        Neighbors.Remove(neighbor);
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
        return Mathf.Infinity;
    }

    public string ToStringWithNeighbors()
    {
        var neighbors = string.Join(", ", Neighbors.Select(n => n.ToString()));
        return $"Boss Room {ToString()}: [{neighbors}]";
    }
}