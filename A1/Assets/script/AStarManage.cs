using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

/// <summary>
/// aStar Management
/// </summary>
public class AStarManage
{
    private static AStarManage instance;
    private AStarManage()
    {
    }
    public static AStarManage GetInstance()
    {
        
        if (instance == null)
            instance = new AStarManage();
        return instance;
        
    }

    //map height
    private int mapH;
    //map width
    private int mapW;

    //concerned nodes for map
    public AStarNode[,] nodes;
    //open list
    private List<AStarNode> openList = new List<AStarNode>() ;
    //close list
    private List<AStarNode> closeList = new List<AStarNode>() ; 

    /// <summary>
    /// initMap function
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>"
    /// <returns></return>
    public void InitMapInfo(int w, int h)
    {
        this.mapH = h;
        this.mapW = w;
        nodes = new AStarNode[w, h];
        for (int i = 0; i < w; ++i)
        {
            for(int j = 0;j < h; ++j)
            {
                AStarNode node = new AStarNode(i, j, Random.Range(0, 100) < 20 ? Node_Type.Stop : Node_Type.Walk);
                nodes[i, j] = node;
            }
        }
    }
    public List<AStarNode> FindPath(Vector2 startPos,Vector3 endPos)
    {
        //pass two node to function
        //1.node must in map
        if (startPos.x < 0 || startPos.x >= mapW || startPos.y < 0 || startPos.y >= mapH ||
            endPos.x < 0 || endPos.x >= mapW || endPos.y < 0 || endPos.y >= mapH)
        {
            Debug.Log("startPos or endPos is outside map");
            return null;
        }

        //2.walk or stop
        //ilegal return null
        AStarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode end = nodes[(int)endPos.x, (int)endPos.y];
        if(start.type == Node_Type.Stop || end.type == Node_Type.Stop)
        {
            Debug.Log("startPos or endPos is stop");
            return null;
        }
        //clear close and openlist
        closeList.Clear();
        openList.Clear();
        //put StartPos into closeList
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);


        //from startPos find node around it and put into openlist
        //topleft top topright........
        while (true)
        {
            FIndNearlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            FIndNearlyNodeToOpenList(start.x , start.y - 1, 1.0f, start, end);
            FIndNearlyNodeToOpenList(start.x +1, start.y - 1, 1.4f, start, end);
            FIndNearlyNodeToOpenList(start.x - 1, start.y, 1.0f, start, end);
            FIndNearlyNodeToOpenList(start.x +1, start.y , 1.0f, start, end);
            FIndNearlyNodeToOpenList(start.x - 1, start.y +1, 1.4f, start, end);
            FIndNearlyNodeToOpenList(start.x, start.y + 1, 1.0f, start, end);
            FIndNearlyNodeToOpenList(start.x +1, start.y + 1, 1.4f, start, end);

            //
             if (openList.Count == 0)
            {
                Debug.Log("no path");
                return null;
            }
            openList.Sort(SortOpenList);

        //
            closeList.Add(openList[0]);
            start = openList[0];
            openList.RemoveAt(0);
            if(start == end)
            {
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while(end.father != null)
                {
                    path.Add(end.father);
                        end = end.father;
                }
                path.Reverse();
                return path;
            }
        }
        
    }
    private int SortOpenList(AStarNode a,AStarNode b)
    {
        if (a.f > b.f)
            return 1;
        else if(a.f == b.f)
            return 1;
        else
            return -1;
    }
    private void FIndNearlyNodeToOpenList(int x, int y,float g,AStarNode father, AStarNode end)
    {
        if (x < 0 || x >= mapW || y < 0 || y >= mapH)
        {
            return;
        }
        AStarNode node = nodes[x, y];
        if (node == null || node.type == Node_Type.Stop||
            closeList.Contains(node)||
            openList.Contains(node))
        {
            return;
        }
        node.father = father;
        node.g = father.g + g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;
        openList.Add(node);
    }
}
