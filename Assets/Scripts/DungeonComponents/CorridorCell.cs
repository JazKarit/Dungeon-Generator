using System;
using System.Collections.Generic;
using UnityEngine;

public class CorridorCell : IComponentGeometry
{
    private (int x, int z) location;
    private bool isRendered;
    private ComponentType cType = ComponentType.corridor;

    private int index = -1;

    private List<IComponentGeometry> adjacentComponents;

    private List<Door> unexpandableDoorways;

       //for INode
    public Guid Id { get; set; } // Unique identifier for the node

    public List<IComponentGeometry> GetAdjacentComponents()
    {
        return adjacentComponents;
    }

    public void AddAdjComponent(IComponentGeometry component)
    {
        adjacentComponents.Add(component);
    }

    public void RemoveAdjComponent(int index)
    {
        adjacentComponents.RemoveAt(index);
    }

    public CorridorCell()
    {
        Id = Guid.NewGuid();
        unexpandableDoorways = new List<Door>();
    }

    public CorridorCell(int x, int z) 
    {
        this.location = (x,z);
        Id = Guid.NewGuid();
        unexpandableDoorways = new List<Door>();
    }

    public void PlaceStartAtGlobalLocation(Door doorLocation)
    {
        this.location = doorLocation.GetDestinationCell();
        unexpandableDoorways.Add(doorLocation.GetMirrorDoor());
    }

    public List<(int, int)> GetGlobalCellsCovered()
    {
        return new List<(int, int)> {location};
    }

    public List<Door> GetDoors()
    {
        return new List<Door> ();
    }

    public List<Door> GetDoorways()
    {
        List<Door> doors = new List<Door>();
        doors.Add(new Door(this.location.x, this.location.z, Direction.E));
        doors.Add(new Door(this.location.x, this.location.z, Direction.N));
        doors.Add(new Door(this.location.x, this.location.z, Direction.W));
        doors.Add(new Door(this.location.x, this.location.z, Direction.S));
        return doors;
    }

    public List<Door> GetDoorwaysWithoutDoors()
    {
        return GetDoorways();
    }

    public List<Door> GetDoorwaysWithDoors()
    {
        return new List<Door>();
    }

    public List<Door> GetWalls()
    {
        return new List<Door>();
    }

    public List<Door> GetEntrances()
    {  
        return GetDoorways();
    }

    public List<Door> GetExits()
    {
        return GetDoorways();
    }

    public void Render()
    {
        if (!isRendered)
        {
            GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cell.transform.position = new Vector3(location.x, 0.5f, location.z);
            cell.transform.localScale = new Vector3(0.9f, 1, 0.9f);
            cell.GetComponent<Renderer>().material.color = Color.white;
            isRendered = true;
        }
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public int GetIndex()
    {
        return index;
    }

    public ComponentType GetType()
    {
        return cType;
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
        List<Door> doorways = GetDoorwaysWithoutDoors();
        foreach (Door door in unexpandableDoorways)
        {
            doorways.RemoveAll(d => d.IsEqual(door));
        }
        return doorways;
    }

    public void RemoveExpandableDoorway(Door door)
    {
        unexpandableDoorways.Add(door);
    }
}

