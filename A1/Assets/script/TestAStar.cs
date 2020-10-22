using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    public int beginX = -3;
    public int beginY =  5;

    public int offsetX = 2;
    public int offsetY = -2;

    public int mapW = 5;
    public int mapH = 5;
    private Vector2 beginPos = Vector2.right * -1;
    private Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();
    public Material red;
    public Material yellow;
    public Material green;
    public Material white;
    List<AStarNode> list;
    // Start is called before the first frame update
    void Start()
    {
        AStarManage manage = AStarManage.GetInstance();
        manage.InitMapInfo(mapW, mapH);
        for (int i = 0; i < mapW; ++i)
        {
            for (int j = 0; j < mapH; ++j)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY);
                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj);
                AStarNode node = manage.nodes[i, j];
                if (node.type == Node_Type.Stop)
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray,out info, 1000))
            {
                
                if(beginPos == Vector2.right * -1)
                {

                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = white;
                        }
                    }
                    string[] strs = info.collider.gameObject.name.Split('_');
                    beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                }
                else
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                     Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));

                   list = AStarManage.GetInstance().FindPath(beginPos, endPos);
                    cubes[(int)beginPos.x + "_" + (int)beginPos.y].GetComponent<MeshRenderer>().material = white;
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                        }
                    }
                    
                    beginPos = Vector2.right * -1;
                }
            }
        }
    }
}
