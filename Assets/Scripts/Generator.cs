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
     // Debug.Log(GenerateBosses(4, 100));
      var bossCoords = GenerateBosses(5, 90, 30);
      map = new Map(bossCoords, 3, (-100, 100), 80);
       // map = new Map(new List<(int,int)> {(0,0), (30,30), (60,0), (0,60), (60,60)}, 3, (-10, 70));
       // map.AddComponent(new PuzzleDoor(new Door(0,0,Direction.N)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.S)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.E)));
        // map.AddComponent(new PuzzleDoor(new Door(0,1,Direction.W)));
        
        //map.AddComponent(new CorridorCell(0,0));
        //map.AddComponent(new CorridorCell(0,1));
        //map.AddComponent(new CorridorCell(-1,-8));

        // bool [,] filledCells = {{true,true,false,true,true,true,true},{true,true,true,true,false,true,true}};

        // var puzzleRoom = new RoomPuzzle(new Door(0,0,Direction.W), new Door(1,6,Direction.E), filledCells);
        // puzzleRoom.PlaceStartAtGlobalLocation(new Door(0,0,Direction.S));

        // map.AddComponent(puzzleRoom);

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
       if (i < 2)
       {
          //map.KPIECE(1000,i); 
          
          
          // for(int j=0; j < 10; j++)          
          // {
          //   map.RRT_KPIECE(10000, 1, decayParam: 0.05f, ratio: 0.8f, useEST: true, goalSampleChance: 0.05f); 
          //   map.RemoveDeadEnds(2);
          // }
          map.RRT_KPIECE(25000, 1, decayParam: 0.05f, ratio: 0.8f, useEST: false, goalSampleChance: 0.1f); 

          //map.Render();
        //   (int x, int y) fcc = map.FindClosestCell(0,1000);
        //   UnityEngine.Debug.Log($"cloest to 0,1000 {fcc}");
        //   fcc = map.FindClosestCell(500,500);
        //   UnityEngine.Debug.Log($"cloest to 500,500 {fcc}");
      }
       if (i == 2)
       {
        //map.graph.PrintGraph();\
        //map.graph.PrintSelfLoops();
        map.TrimSelfLoops();
        //UnityEngine.Debug.Log("trimmed self loops");
        //map.graph.PrintSelfLoops();
        for(int j=0; j < 100; j++)
        {
          map.RemoveDeadEnds(3);
        }
        map.Render();
       }
    }



    List<(int x, int z)> GenerateBosses(int numBossRooms, int radius,int minDistBtwnBoss)
    {
        //int[,] bossCoords = new int[4,2];
        List<(int x, int z)> bossCoords = new List<(int x, int z)>();
        int bossX = 0;
        int bossZ = 0;
        int minDist = 0;
        int maxDist = radius;
        //int minDistBtwnBoss = 10;
        //Random rnd = new Random();

        for(int k=0;k<numBossRooms;k++)
        {
            
            bossX = Random.Range(minDist, maxDist);
            bossX = bossX * (Random.Range(0, 2) * 2 - 1); //ctrl - vs +
            
            bossZ = Random.Range(minDist, maxDist);
            bossZ = bossZ * (Random.Range(0, 2) * 2 - 1); //ctrl - vs +

            //bossCoords[k].x=bossX;
            //bossCoords[k].z=bossZ;
            bool valid = true;
            //validity check
            for(int j=0;j<bossCoords.Count;j++)
            {
                if(j==k)
                {

                }
                else
                {
                    if(Mathf.Abs(bossCoords[j].x-bossX)<= minDistBtwnBoss && Mathf.Abs(bossCoords[j].z-bossZ) <= minDistBtwnBoss ) //if pair of coords already exist
                    {
                        k--; //re-generate this set of coords
                        valid = false;
                    }
                }
            }
            if(valid)
            {
              bossCoords.Add((bossX,bossZ));
            }
        }

        // for(int j=0;j<4;j++)
        // {
        //     RenderRoom(bossCoords[j].x, bossCoords[j].z, 3, 3, Color.white);
        // }    
        return bossCoords;
    }
    
  // List<(int x, int z)> GenerateBosses(int numBossRooms, int radius)
  //   {
  //       var bossCoords = new List<(int x, int z)>();
  //       int bossX = 0;
  //       int bossZ = 0;
  //       int minDistBtwnBoss = 20;

  //       bool ValidBossCoord(int bossX, int bossZ)
  //       {
  //           foreach(var coord in bossCoords)
  //           {
  //               if(Mathf.Abs(bossX - bossCoords.x) < minDistBtwnBoss
  //                  || Mathf.Abs(bossZ - bossCoords.z) < minDistBtwnBoss)
  //               {
  //                 return false;
  //               }
  //           }
  //           return true;
  //       }

  //       for(int i=0;i<numBossRooms;i++)
  //       {
  //           do {
  //             bossX = Random.Range(-radius, radius);
  //             bossZ = Random.Range(-radius, radius);
  //           }
  //           while (!ValidBossCooord(bossX, bossZ, bossCoords));
  //           bossCoords.Add((bossX, bossZ));
  //       }
  //   }
}
