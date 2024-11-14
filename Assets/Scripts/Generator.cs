using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//test comment



public class Generator : MonoBehaviour
{
    void RenderRoom(float x, float z, float width, float length, Color color)
    {
        GameObject room = GameObject.CreatePrimitive(PrimitiveType.Cube);
        room.transform.position = new Vector3(x, 0.5f, z);
        room.transform.localScale = new Vector3(width-0.1f, 1, length-0.1f);
        room.GetComponent<Renderer>().material.color = color;
    }

    void RenderRoom(float x, float z, float width, float length, float hue, float difficulty)
    {
        GameObject room = GameObject.CreatePrimitive(PrimitiveType.Cube);
        room.transform.position = new Vector3(x, 0.5f, z);
        room.transform.localScale = new Vector3(width-0.1f, 1, length-0.1f);
        room.GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 1f, difficulty);
    }

    void RenderMarker(float x, float z, Color color)
    {
        GameObject entranceMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        entranceMarker.transform.position = new Vector3(x, 1.2f, z);
        entranceMarker.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);
        entranceMarker.GetComponent<Renderer>().material.color = color;
    }

    (int, int) getTransform(int x, int z, int rot)
    {
        
        if (rot == 0)
        {
            return (z, x);
        }
        else if (rot == 1)
        {
            return (x, -z);
        }
        else if (rot == 2)
        {
            return (-z, -x);
        }
        else if (rot == 3)
        {
            return (-x, z);
        }
        else
        {
            throw new System.Exception("Invalid rotation");
        }
    }

    void RenderRoomPuzzle(RoomPuzzleGeometry geometry, float hue, float difficulty)
    {
        for (int i = 0; i < geometry.cells.GetLength(0); i++)
        {
            for (int j = 0; j < geometry.cells.GetLength(1); j++)
            {
                if (geometry.cells[i, j] == CellType.empty)
                {
                    continue;
                }
                (int dx, int dz) = getTransform(i, j, geometry.rotation);
                RenderRoom(geometry.x + dx, geometry.z - dz, 1, 1, hue, difficulty);

                if (geometry.cells[i, j] == CellType.entrance)
                {
                    RenderMarker(geometry.x + dx, geometry.z - dz, Color.white);
                }

                if (geometry.cells[i, j] == CellType.exit)
                {
                    RenderMarker(geometry.x + dx, geometry.z - dz, Color.black);
                }
            }
        }
    }
    

    void RenderPuzzleDoor(float x, float z, bool isInXdir, float hue, float difficulty)
    {
        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.transform.position = new Vector3(x, 0.5f, z);
        if (isInXdir)
        {
            door.transform.localScale = new Vector3(0.1f, 1f, 0.9f);
        }
        else
        {
            door.transform.localScale = new Vector3(0.9f, 1f, 0.1f);
        }
        door.GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 1f, difficulty);
    }

    // Only to be used for debugging purposes
    void RenderCorridor(int xStart, int zStart, int xEnd, int zEnd)
    {
        if (zStart == zEnd)
        {
            for (int i = xStart; i <= xEnd; i++)
            {
                RenderRoom(i, zStart, 1, 1, Color.white);
            }
        }
        else if (xStart == xEnd)
        {
            for (int i = zStart; i <= zEnd; i++)
            {
                RenderRoom(xStart, i, 1, 1, Color.white);
            }
        }
        else
        {
            throw new System.Exception("Corridor must be either horizontal or vertical");
        }
        }
    // Start is called before the first frame update
    void Start()
    {
        RenderRoom(0, 0, 3, 3, Color.white);
        RenderRoom(10, 10, 3, 3, 0f, 1f);
        RenderCorridor(2, 0, 10, 0);
        RenderCorridor(10, 1, 10, 8);
        RenderPuzzleDoor(10, 0.5f, false, 0f, 0.5f);


        var geometry = new int[4,4] {
            {0, 2, 0, 0},
            {0, 1, 0, 0},
            {1, 1, 1, 3},
            {0, 1, 1, 1},
        };
        RoomPuzzleGeometry roomPuzzleGeometry = new RoomPuzzleGeometry(geometry, 1, 2);
        roomPuzzleGeometry.rotation = 2;

        RenderRoomPuzzle(roomPuzzleGeometry, 0f, 0.8f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
