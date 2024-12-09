using System;
using System.Collections.Generic;
using UnityEngine;

public class Door
{
    public int x;
    public int z;
    public Direction direction;

    public Door(int x, int z, Direction direction)
    {
        this.x = x;
        this.z = z;
        this.direction = direction;
    }

    public Door GetMirrorDoor()
    {
        Direction direction = Direction.E;
        int x = 0;
        int z = 0;
        if (this.direction == Direction.N)
        {
            direction = Direction.S;
            x = this.x;
            z = this.z + 1;
        }

        if (this.direction == Direction.S)
        {
            direction = Direction.N;
            x = this.x;
            z = this.z - 1;
        }

        if (this.direction == Direction.E)
        {
            direction = Direction.W;
            x = this.x + 1;
            z = this.z;
        }

        if (this.direction == Direction.W)
        {
            direction = Direction.E;
            x = this.x - 1;
            z = this.z;
        }
        return new Door(x,z,direction);
    }

    public bool IsEqual(Door door)
    {
        if (this.x == door.x && this.z == door.z && this.direction == door.direction)
        {
            return true;
        }

        Door mirrorDoor = GetMirrorDoor();

        if (mirrorDoor.x == door.x && mirrorDoor.z == door.z && mirrorDoor.direction == door.direction)
        {
            return true;
        }

        return false;
    }

    public (int x, int z) GetStartCell()
    {
        return (x,z);
    }

    public (int x, int z) GetDestinationCell()
    {
        switch (this.direction)
        {
            case Direction.E:
                return (this.x + 1, this.z);
            case Direction.N:
                return (this.x, this.z + 1);
            case Direction.W:
                return (this.x - 1, this.z);
            case Direction.S:
                return (this.x, this.z - 1);
            default:
                return (0,0);
        }
    }

    public void Render(Color color)
    {
        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.transform.position = new Vector3(this.GetCoordinates().x, 0.5f, this.GetCoordinates().z);
        if (this.direction == Direction.E || this.direction == Direction.W)
        {
            door.transform.localScale = new Vector3(0.1f, 1.1f, 1f);
        }
        else
        {
            door.transform.localScale = new Vector3(1f, 1.1f, 0.1f);
        }
        door.GetComponent<Renderer>().material.color = color;
    }

    public (float x, float z) GetCoordinates()
    {
        switch(this.direction)
        {
            case Direction.E:
                return (x + 0.5f, z);
            case Direction.N:
                return (x, z + 0.5f);
            case Direction.W:
                return (x - 0.5f, z);
            case Direction.S:
                return (x, z - 0.5f);
            default:
                return (0,0);
        }
    }

    public override string ToString()
    {
        return $"Door: ({this.x},{this.z},{this.direction})"; 
    }
}