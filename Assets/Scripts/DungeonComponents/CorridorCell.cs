using System;
using System.Collections.Generic;
using UnityEngine;

public class CorridorCell : IComponentGeometry
{
    private (int x, int z) location;
    private bool isRendered;

    public CorridorCell()
    {

    }

    public CorridorCell(int x, int z) 
    {
        this.location = (x,z);
    }

    public void PlaceStartAtGlobalLocation(Door doorLocation)
    {
        this.location = doorLocation.GetDestinationCell();
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
}

