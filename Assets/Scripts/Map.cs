using System.Collections.Generic;
using UnityEngine;


class Map
{

    // private int [,] cells;
    private List<IComponentGeometry> components = new();

    private List<Door> doors = new();

    private List<(int,int)> cells = new();
    
    public Map()
    {
        
    }

    public List<Door> GetAvailableDoorwaysWithoutDoor()
    {
        List<Door> doorways = new List<Door>();
        foreach(IComponentGeometry component in this.components)
        {
            List<Door> cDoorways = component.GetDoorwaysWithoutDoors();
            foreach (Door door in cDoorways)
            {
                (int x, int z) cell = door.GetDestinationCell();

                // Ensure that destination cell is empty and that there is no door in the spot
                if (!CellCollidesWithMap(cell) && !DoorCollidesWithMap(door))
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
        foreach(IComponentGeometry component in this.components)
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

    private bool DoorCollidesWithMap(Door door)
    {
        return DoorsCollideWithMap(new List<Door> {door});
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
        }
        return false;
    }

    public bool AddComponent(IComponentGeometry component)
    {
        var cDoors = component.GetDoors();
        var cCells = component.GetGlobalCellsCovered();

        if (CellsCollideWithMap(cCells) || DoorsCollideWithMap(cDoors))
        {
            return false;
        }

        doors.AddRange(cDoors);
        cells.AddRange(cCells);
        components.Add(component);

        return true;
    }

    public void Render()
    {
        foreach (IComponentGeometry component in components)
        {
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

    public void RandomExpansion(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            int r;
            List<Door> empytDoorways = GetAvailableDoorwaysWithoutDoor();

            if(empytDoorways.Count > 0)
            {
                r = Random.Range(0,empytDoorways.Count);
                bool [,] filledCells = {{true,false},{true,true}};
                var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,1,Direction.E), filledCells);
                puzzleRoom.PlaceStartAtGlobalLocation(empytDoorways[r]);
                AddComponent(puzzleRoom);
                //AddComponent(new PuzzleDoor(empytDoorways[r]));
            }
        
            List<Door> doorways = GetAvailableDoorways();

            if (doorways.Count > 0)
            {
                r = Random.Range(0,doorways.Count);
                var cell = new CorridorCell();
                cell.PlaceStartAtGlobalLocation(doorways[r]);
                AddComponent(cell);
            }
        }
    }
}