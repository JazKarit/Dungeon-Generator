using System;
using System.Collections.Generic;
using UnityEngine;

public enum ComponentCellType
{
    empty = 0,
    filled = 1
};

public enum Direction
{
    E,
    N,
    W,
    S
};

interface IComponentGeometry
{
    public void PlaceStartAtGlobalLocation(Door location);

    public List<(int, int)> GetGlobalCellsCovered();

    public List<Door> GetDoors();

    public List<Door> GetDoorways();

    public List<Door> GetDoorwaysWithoutDoors();

    public List<Door> GetEntrances();

    public List<Door> GetExits();

    public void Render();
}