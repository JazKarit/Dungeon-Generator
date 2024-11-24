using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private int i = 0;
    private Map map;

    void Start()
    {
        map = new Map();
       // map.AddComponent(new PuzzleDoor(new Door(0,0,Direction.N)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.S)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.E)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.W)));
        
        map.AddComponent(new CorridorCell(0,0));
        //map.AddComponent(new CorridorCell(0,1));
        map.AddComponent(new CorridorCell(-1,-8));

        bool [,] filledCells = {{true,true,false,true,true,true,true},{true,true,true,true,false,true,true}};

        var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,6,Direction.E), filledCells);
        puzzleRoom.PlaceStartAtGlobalLocation(new Door(0,0,Direction.S));

        map.AddComponent(puzzleRoom);

        // TODO: check diff directions different orientations
        bool [,] filledCells2 = {{true,true},{true,false}};

        var puzzleRoom2 = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,1,Direction.E), filledCells2);
        puzzleRoom2.PlaceStartAtGlobalLocation(new Door(0,0,Direction.E));

        // map.AddComponent(puzzleRoom2);
        // map.AddComponent(new CorridorCell(0,1));
        // map.AddComponent(new CorridorCell(0,2));
        // map.AddComponent(new CorridorCell(1,2));
        // map.AddComponent(new CorridorCell(2,2));
        // map.AddComponent(new CorridorCell(3,2));
        // map.AddComponent(new CorridorCell(3,1));
        // map.AddComponent(new CorridorCell(3,0));
        // map.AddComponent(new CorridorCell(-1,3));
        // map.AddComponent(new CorridorCell(-1,0));
        // map.AddComponent(new CorridorCell(-1,3));
        // map.AddComponent(new CorridorCell(0,3));

        var puzzleRoom3 = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,1,Direction.E), filledCells2);
        puzzleRoom3.PlaceStartAtGlobalLocation(new Door(-1,3,Direction.S));
        map.AddComponent(puzzleRoom3);


        map.Render();

        
       var cmpts = map.GetConnectedComponents(map.GetComponentAt((0,0)));

       foreach (var cmpt in cmpts)
       {
            var cells = cmpt.GetGlobalCellsCovered();
            foreach (var cell in cells)
            {
                Debug.Log(cell);
            }
       }

    }

    // Update is called once per frame
    void Update()
    {
      
       i++;
       if (i%20 == 0)
       {
          map.RandomExpansion(1);
          map.Render();
       }
    }
}
