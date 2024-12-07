using System.Collections.Generic;
using UnityEngine;
using System;

class RoomPuzzle : IComponentGeometry
{
    private Door localStartPosition;
    private Door localEndPosition;

    public bool [,] filledCells;

    private Direction mapDirectionLocalE;
    private ComponentType cType = ComponentType.puzzleRoom;

    private (int x, int z) globalPos;

    private bool isRendered = false;

    private List<IComponentGeometry> adjacentComponents;

    private int index = -1;

    private List<Door> walls;

   //for INode
    public Guid Id { get; set; } // Unique identifier for the node

    private Color color;

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

    public RoomPuzzle(Door localStartPosition, Door localEndPosition, bool [,] filledCells, List<Door> walls = null) 
    {
        this.localStartPosition = localStartPosition;
        this.localEndPosition = localEndPosition;
        this.filledCells = filledCells;
        if (walls is null)
        {
            this.walls = new List<Door>();
        }
        else
        {
            this.walls = walls;
        }

        Id = Guid.NewGuid();
    }

    private (int, int) GetTransform(int x, int z)
    {
        int rot = (int)this.mapDirectionLocalE;
        if (rot == 0)
        {
            return (z, x);
        }
        else if (rot == 1)
        {
            return (x, z);
        }
        else if (rot == 2)
        {
            return (-z, -x);
        }
        else if (rot == 3)
        {
            return (-x, -z);
        }
        else
        {
            throw new System.Exception("Invalid rotation");
        }
    }

    private (int x, int z) GetGlobalCoordinates(int x, int z)
    {
        (int x, int z) localOriginToPointVector = GetTransform(x,z);
        return (this.globalPos.x + localOriginToPointVector.x, this.globalPos.z + localOriginToPointVector.z);
    }

    public void PlaceStartAtGlobalLocation(Door entranceLocation)
    {
        int globalEntranceDir = (int)entranceLocation.direction;
        int localEntranceDir = (int)this.localStartPosition.GetMirrorDoor().direction;
        this.mapDirectionLocalE = (Direction)((globalEntranceDir + 4 - localEntranceDir) % 4);
        
        (int x, int z) globalStartCell = entranceLocation.GetDestinationCell();

        (int x, int z) localOriginToStartVector = GetTransform(this.localStartPosition.x, this.localStartPosition.z);
        
        this.globalPos = (globalStartCell.x - localOriginToStartVector.x, globalStartCell.z - localOriginToStartVector.z);
    }

    public List<(int, int)> GetGlobalCellsCovered()
    {
        List<(int x, int z)> cells = new();
        for (int x = 0; x < filledCells.GetLength(0); x++)
        {
            for (int z = 0; z < filledCells.GetLength(1); z++)
            {
                if(filledCells[x,z])
                {
                    cells.Add(GetGlobalCoordinates(x,z));
                }
            }
        }
        return cells;
    }

    private Door GetGlobalDoorLocation(Door localDoor)
    {
        (int x, int z) globalStartCell = GetGlobalCoordinates(localDoor.GetStartCell().x,localDoor.GetStartCell().z);
        return new Door(globalStartCell.x,globalStartCell.z,(Direction)(((int)localDoor.direction + (int)this.mapDirectionLocalE) % 4));
    }

    private Door GetGlobalStartLocation()
    {
        (int x, int z) globalStartCell = GetGlobalCoordinates(localStartPosition.GetStartCell().x,localStartPosition.GetStartCell().z);
        return new Door(globalStartCell.x,globalStartCell.z,(Direction)(((int)localStartPosition.direction + (int)this.mapDirectionLocalE) % 4));
    }

    public Door GetGlobalEndLocation()
    {
        (int x, int z) globalEndCell = GetGlobalCoordinates(localEndPosition.GetStartCell().x,localEndPosition.GetStartCell().z);
        return new Door(globalEndCell.x,globalEndCell.z,(Direction)(((int)localEndPosition.direction + (int)this.mapDirectionLocalE) % 4));
    }

    public List<Door> GetDoors()
    {
        return new List<Door> {GetGlobalStartLocation(), GetGlobalEndLocation()};
    }

    public List<Door> GetDoorways()
    {
        return new List<Door> {GetGlobalStartLocation(), GetGlobalEndLocation()};
    }

    public List<Door> GetDoorwaysWithoutDoors()
    {
        return new List<Door>();
    }

    public List<Door> GetDoorwaysWithDoors()
    {
        return GetDoorways();
    }

    public List<Door> GetEntrances()
    {
        return new List<Door> {GetGlobalStartLocation()};
    }

    public List<Door> GetExits()
    {
        return new List<Door> {GetGlobalEndLocation()};
    }

    public List<Door> GetWalls()
    {
        var globalWalls = new List<Door>();
        foreach (var wall in this.walls)
        {
            globalWalls.Add(GetGlobalDoorLocation(wall));
        }
        return globalWalls;
    }

    public void Render()
    {
        if (!isRendered)
        {
            Color color = Color.HSVToRGB(UnityEngine.Random.Range(0f,1f), 0.7f, 0.7f);
            foreach ((int x, int z) cellCoord in GetGlobalCellsCovered())
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.position = new Vector3(cellCoord.x, 0.5f, cellCoord.z);
                cell.transform.localScale = new Vector3(0.9f, 1, 0.9f);
                cell.GetComponent<Renderer>().material.color = color;
            }

            GetGlobalStartLocation().Render(Color.green);
            GetGlobalEndLocation().Render(Color.red);
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
}