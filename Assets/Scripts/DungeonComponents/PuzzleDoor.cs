using System;
using System.Collections.Generic;
using UnityEngine;

class PuzzleDoor : IComponentGeometry
{
    private Door location;
    private bool rendered;

    public PuzzleDoor(Door location) 
    {
        this.location = location;
    }

    public void PlaceStartAtGlobalLocation(Door location)
    {
        this.location = location;
    }

    public List<(int, int)> GetGlobalCellsCovered()
    {
        return new List<(int, int)>();
    }

    public List<Door> GetDoors()
    {
        return new List<Door> {location};
    }

    public List<Door> GetDoorways()
    {
        return new List<Door> {location};
    }

    public List<Door> GetDoorwaysWithoutDoors()
    {
        return new List<Door>();
    }

    public List<Door> GetEntrances()
    {
        return new List<Door> {location};
    }

    public List<Door> GetExits()
    {
        return new List<Door> {location};
    }

    public void Render()
    {
        if (!rendered)
        {
            this.location.Render(Color.black);
            rendered = true;
        }
    }
}