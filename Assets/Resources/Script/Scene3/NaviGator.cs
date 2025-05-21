using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private float posX, posZ;

    public Node ParentNode { get; set; }
    public void SetNodeClosing(Node _parentNode)
    {
        ParentNode = _parentNode;
        if (ParentNode != null){ IsBlock = true; }
    }
}

public class NaviGator : MonoBehaviour
{
    public float Speed;

    private int mapLength_X, mapLength_Z;
    private Node[,] searchMap;
    private GameObject[] blockArr;
    private GameObject[] roadArr;

    private GameObject startCube;
    private GameObject endCube;
    private GameObject block;
    private List<Node> openList;
    private List<Node> closeList;
    private List<Node> finalList;
    private List<GameObject> BlockList;
    private Node cursor;
    private Node finalNode;
    private bool Research;
    private int nodeCount;
    private Vector3 nextMovePos;
    private Vector3 AfterPos;

    [SerializeField]
    GameObject cube;
    List<GameObject> cubeList;


    private void Awake()
    {
        if (gameObject.name == "Start")
        { transform.position = SetPosition(transform.position); }

        startCube = GameObject.Find("Start");
        endCube = GameObject.Find("End");
        openList = new List<Node>();
        closeList = new List<Node>();
        finalList = new List<Node>();
        
        cubeList= new List<GameObject>();
        nextMovePos = transform.position;
        AfterPos=transform.position;    

    }
    void Start()
    {
        SearchRoad();
    }
   
    void Update()
    {
        if(finalList.Count == 0|| Research==true)
        {
            Research = false;
            ResearchRoad();
        }

        StartCoroutine(Dir());
        Move();

    }
    public void ResearchRoad()
    {
        Initialize();
        SearchRoad();
        Debug.Log("Research");
    }
    private void SearchRoad()
    {
        SetMap();
        SetBlockList();
        SetStartNode();
        SetFinalNode();
        SearchDir();
      //  CubeCreator();
    }
    private void Initialize()
    {
        openList.Clear();
        closeList.Clear();
        finalList.Clear();
        BlockList.Clear();
        nodeCount = 0;
    }
   
    public IEnumerator Dir()
    {
        while (true)
        {
            Vector3 currentPos = transform.position;
            Vector3 moveDirection = currentPos - AfterPos;
            if (moveDirection.magnitude > 0.01f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetAngle - 180, 0), Time.deltaTime * 10f);
            }
            AfterPos = currentPos;
            yield return null;
        }
    }
    private void Move()
    {

        if (finalList == null || finalList.Count == 0) return;

        if (transform.position.x==endCube.transform.position.x
            && transform.position.z == endCube.transform.position.z)
        {
            Destroy(gameObject);
        }

        if (Vector3.Distance(transform.position, nextMovePos) < 0.01f)
        {
            if (finalList.Count - 1 > nodeCount)
            {
                nodeCount++;
            }
        }
        if (gameObject.tag == "Enemy")
        {
            if (nodeCount < finalList.Count)
            {
                 nextMovePos = finalList[nodeCount].Pos;
                 transform.position = Vector3.MoveTowards(
                    transform.position,
                    nextMovePos,
                    Speed * Time.deltaTime
                );
            }
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
               // Debug.Log("finalList.Count == 0");
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
            if (finalList.Count - 2 < 0 || finalList.Count - 1 < 0)
            {
                finalList.Clear();
                finalList.Add(new Node(endCube.transform.position.x, endCube.transform.position.z, false));
                return;
            }

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

            finalList.Add(new Node(transform.position.x, transform.position.z, true));
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
            if (count > (searchMap.GetLength(0) * searchMap.GetLength(1)- blockArr.Length)) 
            {
                Debug.Log("SearchDir Count : "+count);
                GameManager.Instance.LastBlockDestroy();
                Research = true;
                break; 
            }

            foreach (Node node in closeList)
            {
                cursor = node;

                if (cursor.Pos == finalNode.Pos)
                {
                    //Debug.Log("cursor.Pos == finalNode.Pos");
                    FinalNodeCheck();
                    hasFinalNode = true;
                }
                else
                {
                    int x = (int)cursor.Pos.x;
                    int z = (int)cursor.Pos.z;

                    //©Л
                    if (x + 1 == searchMap[x + 1, z].Pos.x && z == searchMap[x + 1, z].Pos.z)
                    {
                        OpenNodeCheck(searchMap[x + 1, z]);
                    }
                    //аб
                    if (x - 1 == searchMap[x - 1, z].Pos.x && z == searchMap[x - 1, z].Pos.z)
                    {
                        OpenNodeCheck(searchMap[x - 1, z]);
                    }
                    //╩С
                    if (x == searchMap[x, z + 1].Pos.x && z + 1 == searchMap[x, z + 1].Pos.z)
                    {
                        OpenNodeCheck(searchMap[x, z + 1]);
                    }
                    //го
                    if (x == searchMap[x, z - 1].Pos.x && z - 1 == searchMap[x, z - 1].Pos.z)
                    {
                        OpenNodeCheck(searchMap[x, z - 1]);
                    }

                }
            }
            CloseNodeCheck();
            count++;
        }
     
    }
  
    private void SetBlockList()
    {
        BlockList = blockArr.ToList();
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
        Vector3 startPos = SetPosition(transform.position);
        Node startNode = new Node((int)startPos.x, (int)startPos.z, true);
       // Debug.Log("startNode : " + startNode.Pos.x + ", " + startNode.Pos.z);
        closeList.Add(startNode);
    }
    private void SetMap()
    {
        FindRoad();
        FindBlock();
        SetMapSize();

        searchMap = new Node[mapLength_X, mapLength_Z];

        foreach (GameObject block in blockArr)
        {
            int x = (int)block.transform.position.x;
            int z = (int)block.transform.position.z;
            searchMap[x, z] = new Node(x, z, true);
        }

        for (int x = 0; x < mapLength_X; x++)
        {
            for (int z = 0; z < mapLength_Z; z++)
            {
                if (searchMap[x, z] == null)
                {
                    searchMap[x, z] = new Node(x, z, false);
                }
            }
        }
    }
    private void SetMapSize()
    {
        foreach (GameObject block in blockArr)
        {
            if (block.transform.position.x > mapLength_X)
            { mapLength_X = (int)block.transform.position.x; }
            if (block.transform.position.z > mapLength_Z)
            { mapLength_Z = (int)block.transform.position.z; }
        }

        mapLength_X += 1;
        mapLength_Z += 1;
        // Debug.Log("MapX : " + mapLength_X + ", MapZ : " + mapLength_Z);
    }

    private void FindBlock()
    {
        if (blockArr != null) { blockArr = null; }

        blockArr = GameObject.FindGameObjectsWithTag("Block");

        foreach (GameObject block in blockArr)
        {
            block.transform.position = SetPosition(block.transform.position);
        }
    }

    private void FindRoad()
    {
        if (roadArr != null) { roadArr = null; }

        roadArr = GameObject.FindGameObjectsWithTag("Road");

        foreach (GameObject road in roadArr)
        {
            road.transform.position = SetPosition(road.transform.position);
        }
    }
   
    private Vector3 SetPosition(Vector3 position)
    {
        float posX = position.x;
        float posZ = position.z;
        int i_PosX = (int)position.x;
        int i_PosZ = (int)position.z;
        if (posX - i_PosX > 0.500f) { i_PosX += 1; }
        if (posZ - i_PosZ > 0.500f) { i_PosZ += 1; }

        return new Vector3(i_PosX, position.y, i_PosZ);
    }

}
