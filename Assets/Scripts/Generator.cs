using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private Map map;

    void Start()
    {
        map = new Map();
       // map.AddComponent(new PuzzleDoor(new Door(0,0,Direction.N)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.S)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.E)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.W)));
       // map.AddComponent(new CorridorCell(0,0));

        bool [,] filledCells = {{true,false},{true,true}};

        var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,1,Direction.E), filledCells);
        puzzleRoom.PlaceStartAtGlobalLocation(new Door(0,0,Direction.S));

        map.AddComponent(puzzleRoom);
        map.Render();
        
    }

    // Update is called once per frame
    void Update()
    {
        map.RandomExpansion(1);
        map.Render();
    }
}
