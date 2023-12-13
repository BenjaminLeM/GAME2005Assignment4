using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public int ShapeType = 0;
    //ShapeType = 0 sphere
    //ShapeType = 1 aabb
    //ShapeType = 2 plane
    //ShapeType = 3 half-plane
    public int ObjectType = 0;
    //ObjectType = 0 nothing
    //ObjectType = 1 Bird
    //ObjectType = 2 Pig

    public int PigToughness = 10;

    public int GetShape()
    {
        return ShapeType;
    }
}