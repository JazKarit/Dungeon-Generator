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

public enum ComponentType
{
    puzzleRoom,
    corridor,
    puzzleDoor,
    superCorridor,
    bossRoom
}

public interface IComponentGeometry
{
    public Guid Id { get;  } // Unique identifier for the node

    public void PlaceStartAtGlobalLocation(Door location);

    public List<(int x, int z)> GetGlobalCellsCovered();

    public List<Door> GetDoors();

    public List<Door> GetDoorways();

    public List<Door> GetExpandableDoorways();

    public List<Door> GetExpandableDoorwaysWithoutDoors();

    public void RemoveExpandableDoorway(Door door);

    /// <summary>
    /// Only remove the doorway from expansion for a certain ComponentType.
    /// </summary>
    /// <param name="door">Doorway to remove</param>
    /// <param name="type">Type of component to disallow</param>
    //public void RemoveExpandableDoorway(Door door, ComponentType type);

    public List<Door> GetDoorwaysWithoutDoors();
    public List<Door> GetDoorwaysWithDoors();

    public List<Door> GetWalls();

    public List<Door> GetEntrances();

    public List<Door> GetExits();

    public void Render();



    //public void SetIndex(int i);

    //public int GetIndex();

    public ComponentType GetType();
}