using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private int i = 0;
    private Map map;

    //test
    //test2
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
       // bool [,] filledCells2 = {{true,true},{true,false}};

        // var puzzleRoom2 = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,1,Direction.E), filledCells2);
        // puzzleRoom2.PlaceStartAtGlobalLocation(new Door(0,0,Direction.E));

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

        // var puzzleRoom3 = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,1,Direction.E), filledCells2);
        // puzzleRoom3.PlaceStartAtGlobalLocation(new Door(-1,3,Direction.S));
        // map.AddComponent(puzzleRoom3);


        //map.Render();

        
      // var cmpts = map.GetConnectedComponents(map.GetComponentAt((0,0)));

    //    foreach (var cmpt in cmpts)
    //    {
    //         var cells = cmpt.GetGlobalCellsCovered();
    //         foreach (var cell in cells)
    //         {
    //             Debug.Log(cell);
    //         }
    //    }

    }

    // Update is called once per frame
    void Update()
    {
        i++;
        // if(i == 0)
        // {
        //     var puzzleRoom2 = new RoomPuzzle(new Door(0,0,Direction.W), new Door(0,1,Direction.E), filledCells2);
        //     puzzleRoom2.PlaceStartAtGlobalLocation(new Door(0,0,Direction.E));
        //     map.AddComponent(puzzleRoom2);
        // }
    //     if(i > 100)
    //     map.AddComponent(new CorridorCell(0,1));
    //     if(i > 200)
    //     map.AddComponent(new CorridorCell(1,2));
    //     if(i > 300)
    //     map.AddComponent(new CorridorCell(0,2));
    //     if(i > 400)
    //     map.AddComponent(new CorridorCell(2,2));
    //     if(i > 500)
    //     map.AddComponent(new CorridorCell(3,2));
    //     if(i > 600)
    //     map.AddComponent(new CorridorCell(3,1));
    //     if(i > 700)
    //     map.AddComponent(new CorridorCell(3,0));
    //     if(i > 800)
    //     map.AddComponent(new CorridorCell(-1,3));
    //     if(i > 900)
    //     map.AddComponent(new CorridorCell(-1,0));
    //     if(i > 1000)
    //     map.AddComponent(new CorridorCell(-1,3));
    //     if(i > 1100)
    //     map.AddComponent(new CorridorCell(0,3));
    //    map.Render();
    //    foreach(var component in map.components.Values)
    //    {
    //     if (component.GetType() == ComponentType.superCorridor)
    //     {
    //         foreach(var cube in ((SuperCorridor)component).cubes)
    //         {
    //             cube.SetActive(false);
    //             cube.GetComponent<Renderer>().material.color = ((SuperCorridor)component).color;
    //         }
    //     }
     //  }
       if (i%10 == 0)// && i < 1000)
       {
          map.EST(1); 
          map.Render();
       }
    //    if (i % 1000 == 0)
    //    {
    //     map.graph.PrintGraph();
    //    }
    }
}
