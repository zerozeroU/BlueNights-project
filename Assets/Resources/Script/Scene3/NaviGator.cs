using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Node(float _posX, float _posZ, bool _isBlock)
    {
        posX = _posX;
        posZ = _posZ;
        IsBlock = _isBlock;
    }
    public Vector3 Pos
    {
        get{ return new Vector3(posX, 0, posZ);}
    }
    public bool IsBlock { get; set; }
    public Node ParentNode { get; set; }

    private float posX, posZ;

    public void SetNodeClosing(Node _parentNode)
    {
        ParentNode = _parentNode;
        if (ParentNode != null){ IsBlock = true; }
    }
}

public class NaviGator : MonoBehaviour
{

    private GameObject startCube;
    private GameObject endCube;
    private GameObject block;
    private List<Node> openList;
    private List<Node> closeList;
    private List<Node> finalList;
    private List<GameObject> BlockList;
    private Node cursor;
    private Node finalNode;
    private Node[,] map;

    [SerializeField]
    GameObject cube;
    List<GameObject> cubeList;


    private void Awake()
    {
        if (gameObject.name == "Start")
        { transform.position = GameManager.Instance.SetPosition(transform.position); }

        startCube = GameObject.Find("Start");
        endCube = GameObject.Find("End");
        openList = new List<Node>();
        closeList = new List<Node>();
        finalList = new List<Node>();

        cubeList= new List<GameObject>();
    }
    void Start()
    {
        SearchRoad();
    }
   
    void Update()
    {
        if(finalList.Count == 0)
        {
            ResearchRoad();
        }
    }
    private void CubeCreator()
    {
        if (cube != null)
        {
            if (cubeList.Count != 0)
            {
                foreach (GameObject list in cubeList)
                {
                    Destroy(list);
                }
                cubeList.Clear();
            }
            if (finalList.Count == 0)
            {
                Debug.Log("finalList.Count == 0");
                return;
            }

            foreach (Node node in finalList)
            {
                GameObject c = Instantiate(cube);
                c.transform.position = node.Pos;
                cubeList.Add(c);
            }
        }
    }
    public void ResearchRoad()
    {
        Initialize();
        SearchRoad();
        Debug.Log("ResearchRoad");
    }
    private void SearchRoad()
    {
        SetBlockList();
        SetMap();
        SetStartNode();
        SetFinalNode();
        SearchDir();
       // CubeCreator();
    }
    private void Initialize()
    {
        openList.Clear();
        closeList.Clear();
        finalList.Clear();
        BlockList.Clear();
    }
    private void FinalNodeCheck()
    {
        while (cursor.ParentNode != null)
        {
            finalList.Add(cursor);
            cursor = cursor.ParentNode;
            if (cursor.ParentNode == null) { finalList.Add(cursor); }
        }
        UnitPrecisePositionCheck();
        finalList.Reverse();
    }
    private void UnitPrecisePositionCheck()
    {
        if (gameObject.tag == "Enemy")
        {
            Vector3 dirPos = finalList[finalList.Count - 2].Pos - finalList[finalList.Count - 1].Pos;

            Direction dir = Direction.Defaut;

            if ((int)dirPos.x == 1)
            {
                dir = Direction.Right;
            }
            else if ((int)dirPos.x == -1)
            {
                dir = Direction.Left;
            }
            else if ((int)dirPos.z == 1)
            {
                dir = Direction.Top;
            }
            else if ((int)dirPos.z == -1)
            {
                dir = Direction.Bottom;
            }

            dirPos = finalList[finalList.Count - 2].Pos - new Vector3(transform.position.x, 0, transform.position.z);

            if (dirPos.x < 1f && dir == Direction.Right)
            {
                finalList.RemoveAt(finalList.Count - 1);
            }
            if (dirPos.x > -1f && dir == Direction.Left)
            {
                finalList.RemoveAt(finalList.Count - 1);
            }
            if (dirPos.z < 1f && dir == Direction.Top)
            {
                finalList.RemoveAt(finalList.Count - 1);
            }
            if (dirPos.z > -1f && dir == Direction.Bottom)
            {
                finalList.RemoveAt(finalList.Count - 1);
            }
            Debug.Log("x : " + dirPos.x + ", z : " + dirPos.z);

            Node myPosNode = new Node(transform.position.x, transform.position.z, true);
            finalList.Add(myPosNode);
        }
    }
    private void CloseNodeCheck()
    {
        closeList.Clear();
        foreach (Node node in openList)
        {
            closeList.Add(node);
        }
        openList.Clear();
    }
    private void OpenNodeCheck(Node node)
    {
        if(node.IsBlock==false)
        {
            node.SetNodeClosing(cursor);
            openList.Add(node);
        }
    }
    private void SearchDir()
    {
        bool hasFinalNode = false;
        bool aroundBlock = false;
        int count = 0;
        while (!hasFinalNode)
        {
            if (count > map.GetLength(0) * map.GetLength(1)) 
            {
                Debug.Log("SearchDir Count : "+count);
                GameManager.Instance.LastBlockDestroy();
                GameManager.Instance.ResearchRoad = true;
                break; 
            }

            foreach (Node node in closeList)
            {
                cursor = node;

                if (cursor.Pos == finalNode.Pos)
                {
                    Debug.Log("cursor.Pos == finalNode.Pos");
                    FinalNodeCheck();
                    hasFinalNode = true;
                }
                else
                {
                    int x = (int)cursor.Pos.x;
                    int z = (int)cursor.Pos.z;

                    if (x + 1 == map[x + 1, z].Pos.x && z == map[x + 1, z].Pos.z)
                    {
                        OpenNodeCheck(map[x + 1, z]);
                    }
                    if (x - 1 == map[x - 1, z].Pos.x && z == map[x - 1, z].Pos.z)
                    {
                        OpenNodeCheck(map[x - 1, z]);
                    }
                    if (x == map[x, z + 1].Pos.x && z + 1 == map[x, z + 1].Pos.z)
                    {
                        OpenNodeCheck(map[x, z + 1]);
                    }

                    if (x == map[x, z - 1].Pos.x && z - 1 == map[x, z - 1].Pos.z)
                    {
                        OpenNodeCheck(map[x, z - 1]);
                    }

                }
            }
            CloseNodeCheck();
            count++;
        }
        if(aroundBlock)
        {
        }
    }
    private void SetMap()
    {
        map = GameManager.Instance.GetMap();
        if (map == null) { Debug.Log("map is Null"); }
    }
    private void SetBlockList()
    {
        BlockList = GameManager.Instance.GetArrBlocks().ToList();
        if (BlockList == null) { Debug.Log("BlockList is Null"); }
    }
    private void SetFinalNode()
    {
        int posX = (int)endCube.transform.position.x;
        int posZ = (int)endCube.transform.position.z;
        finalNode = new Node(posX, posZ, false);
       // Debug.Log("finalNode : " + finalNode.Pos.x + ", " + finalNode.Pos.z);
    }
    private void SetStartNode()
    {
        Vector3 startPos = GameManager.Instance.SetPosition(transform.position);
        Node startNode = new Node((int)startPos.x, (int)startPos.z, true);
       // Debug.Log("startNode : " + startNode.Pos.x + ", " + startNode.Pos.z);
        cursor = startNode;
        closeList.Add(startNode);
    }
  
}
