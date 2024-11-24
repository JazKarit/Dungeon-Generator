using System;
using System.Collections.Generic;
using UnityEngine;

class PuzzleDoor : IComponentGeometry
{
    private Door location;
    private bool rendered;

    private int index = -1;

    private List<IComponentGeometry> adjacentComponents;

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

    public List<Door> GetDoorwaysWithDoors()
    {
        return new List<Door> {location};
    }

    public List<Door> GetWalls()
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

    public void SetIndex(int i)
    {
        index = i;
    }

    public int GetIndex()
    {
        return index;
    }
}