using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// a star ndoe class
/// </summary>

public enum Node_Type
{
    //node that can go
    Walk,
    //node that can not go
    Stop
}
public class AStarNode
{
    //node coordinate
    public int x;
    public int y;

    //f=g+h
    public float f;
    //distance to start node
    public float g;
    //distance to destination
    public float h;

    //father node
    public AStarNode father;

    //node type
    public Node_Type type;
    /// <summary>
    /// struct function
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    public AStarNode(int x, int y, Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}
