using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public GameObject Cusor;
    public bool DeployUnit;
    public GameObject UnitGround;
    public GameObject BlockGround;
    public GameObject LastDropBlock;
    public bool ResearchRoad;
    public GameObject OnDestoryBlock;
    public int Cost;


    private GameObject _canvas;
    private int mapLength_X, mapLength_Z;
    private Node[,] _searchMap;
    private GameObject[] _blockArr;
    private GameObject[] _roadArr;
    private Dictionary<string,GameObject> _unitDict;
    private GameObject _joyStick;
    private List<GameObject> _groundList;
    private List<GameObject> _destroyGroundList;

    private void Awake()
    {
        LoadUI();
        _unitDict = new Dictionary<string, GameObject>();
        _groundList = new List<GameObject>();
        _destroyGroundList = new List<GameObject>();
        SetEndCubePos();
        SetMap();
    }
    private void Start()
    {
        LoadSetUnitPrefab();
    }
    void Update()
    {
        if(ResearchRoad)
        {
            Debug.Log("OnDestoryBlock : " + OnDestoryBlock);

            if (OnDestoryBlock == null)
            {
                Debug.Log("OnDestoryBlock : " + OnDestoryBlock);
                AllUnitResearchRoad();

                
                ResearchRoad = false;
            }
        }
    }

 



    public void UnitGroundDeploy(GameObject gameObject)
    {
        if(DeployUnit)
        {
            if ((gameObject.tag=="Block"|| gameObject.tag == "Road")
                && gameObject.GetComponent<Ground>().StandingUnit!=null)
            {
                UnitGround = gameObject;
            }
            if(UnitGround!=null&&gameObject.tag=="Card")
            {
                UnitExitGround(UnitGround.GetComponent<Ground>());
            }
        }
        else
        {
            UnitGround = null;
        }
    }
    public void LastBlockDestroy()
    {
        if (LastDropBlock != null)
        {
           
            Ground ground = LastDropBlock.GetComponent<DropBlock>().ground.GetComponent<Ground>();
            ground.IsSortie = false;
            ground.HasUnit = false;
            ground.StandingUnit = null;
            Destroy(LastDropBlock);
            if (GameObject.Find("BlockDestroyMassage(Clone)") == null)
            {
                Instantiate(Resources.Load<GameObject>("Prefab/BlockDestroyMassage"), UIManager.Instance.canvas.transform);
            }
        }
    }
    public void AllUnitResearchRoad()
    {
       

        SetMap();
        NaviGator[] naviGatorArr = FindObjectsOfType<NaviGator>();

        Debug.Log("UnitReSeachRoad   " + naviGatorArr.Length);
        Debug.Log(naviGatorArr[0].gameObject.name);
        foreach (NaviGator arr in naviGatorArr)
        {
            arr.ResearchRoad();
        }
    }
    
    public void ResetGroundDir(UnitOnDuty unit)
    {
        foreach (GameObject list in _groundList)
        {
            list.GetComponent<Ground>().IsDirection = false;
        }
    }
    public void UnitAttackField(GameObject unit)
    {
        if(unit == null) 
        { Debug.Log("UnitAttackField, unit is null"); return; }

        if (unit.GetComponent<UnitOnDuty>().SelectedAttField == false)
        {
            unit.GetComponent<UnitOnDuty>().AttackFieldList.Clear();
        }
        Direction dir = unit.GetComponent<UnitOnDuty>().dir;
        bool[,] attackArea = unit.GetComponent<UnitOnDuty>().unitdata.Area;
        int x = (int)unit.transform.position.x;
        int z = (int)unit.transform.position.z;

        for (int i = 0; i < attackArea.GetLength(0); i++)
        {
            for (int j = 0; j < attackArea.GetLength(1); j++)
            {
                switch (dir)
                {
                    case Direction.Defaut:
                        break;
                    case Direction.Top:
                        for (int k = 0; k < _groundList.Count; k++)
                        {
                            if (_groundList[k] == null) { continue; }
                            if (x - attackArea.GetLength(0) / 2 + i == _groundList[k].transform.position.x
                                && z + j == _groundList[k].transform.position.z)
                            {
                                if (attackArea[i, j] == true)
                                {
                                  _groundList[k].GetComponent<Ground>().IsDirection = true;
                                    if (unit.GetComponent<UnitOnDuty>().SelectedAttField == false)
                                    {
                                        unit.GetComponent<UnitOnDuty>().AttackFieldList.Add(_groundList[k]);
                                    }
                                }
                               
                            }
                        }
                        break;
                    case Direction.Bottom:
                        for (int k = 0; k < _groundList.Count; k++)
                        {
                            if (_groundList[k] == null) { continue; }
                            if (x - attackArea.GetLength(0) / 2 + i == _groundList[k].transform.position.x
                                && z - j == _groundList[k].transform.position.z)
                            {
                                if (attackArea[i, j] == true)
                                {
                                    _groundList[k].GetComponent<Ground>().IsDirection = true;
                                    if (unit.GetComponent<UnitOnDuty>().SelectedAttField == false)
                                    {
                                        unit.GetComponent<UnitOnDuty>().AttackFieldList.Add(_groundList[k]);
                                    }
                                }

                            }
                        }
                        break;
                    case Direction.Right:
                        for (int k = 0; k < _groundList.Count; k++)
                        {
                            if (_groundList[k] == null) { continue; }
                            if (z - attackArea.GetLength(0) / 2 + i == _groundList[k].transform.position.z
                                && x + j == _groundList[k].transform.position.x)
                            {
                                if (attackArea[i, j] == true)
                                {
                                    _groundList[k].GetComponent<Ground>().IsDirection = true;
                                    if (unit.GetComponent<UnitOnDuty>().SelectedAttField == false)
                                    {
                                        unit.GetComponent<UnitOnDuty>().AttackFieldList.Add(_groundList[k]);
                                    }
                                }

                            }
                        }
                        break;
                    case Direction.Left:
                        for (int k = 0; k < _groundList.Count; k++)
                        {
                            if (_groundList[k] == null) { continue; }
                            if (z - attackArea.GetLength(0) / 2 + i == _groundList[k].transform.position.z
                                && x - j == _groundList[k].transform.position.x)
                            {
                                if (attackArea[i, j] == true)
                                {
                                    _groundList[k].GetComponent<Ground>().IsDirection = true;
                                    if (unit.GetComponent<UnitOnDuty>().SelectedAttField == false)
                                    {
                                        unit.GetComponent<UnitOnDuty>().AttackFieldList.Add(_groundList[k]);
                                    }
                                }

                            }
                        }
                        break;
                }
            }
        }
    }
    public GameObject InsJoyStick(Ground ground)
    {
        GameObject JoyStick = Instantiate(_joyStick, _canvas.transform);
        JoyStick.transform.position
            = Camera.main.WorldToScreenPoint(ground.StandingUnit.transform.position);
        JoyStick.GetComponent<JoyStick>().Ground = ground;
        JoyStick.GetComponent<JoyStick>().Card = ground.Card;

        return JoyStick;
    }
    private void LoadUI()
    {
        _joyStick = Resources.Load<GameObject>("Prefab/JoyStick");
        _canvas = GameObject.Find("Canvas");

        if(_joyStick==null|| _canvas==null)
        {
            Debug.Log("UI Object is null");
        }
    }
    public void UnitOnGround(UnitCard card, Ground ground, Vector3 vec3)
    {
        ground.StandingUnit = Instantiate(_unitDict[card.unitData.EnName]);
        ground.StandingUnit.transform.position = vec3;
        ground.StandingUnit.GetComponent<UnitControl>().ground= ground;
        ground.StandingUnit.GetComponent<UnitControl>().Card= card.gameObject;
        ground.StandingUnit.GetComponent<UnitControl>().data = card.unitData;
    }
    public void BlockOnGround(BlockCard card, Ground ground, Vector3 vec3)
    {
        ground.StandingUnit = Instantiate(Resources.Load<GameObject>("Prefab/DropBlock"));
        ground.StandingUnit.GetComponent<DropBlock>().ground= ground;
        ground.StandingUnit.transform.position = vec3;
    }
    public void UnitExitGround(UnitCard card, Ground ground)
    {
           if(ground.StandingUnit != null)
        {
            ground.IsSortie = false;
            ground.HasUnit = false;
            ground.Card = null;

            foreach (GameObject list in ground.StandingUnit.GetComponent<UnitOnDuty>().AttackFieldList)
            {
                list.GetComponent<Ground>().IsDirection = false;
                list.GetComponent<Ground>().AttFieldUnitList.Remove(ground.StandingUnit);
            }

            Destroy(ground.StandingUnit);
            ground.StandingUnit = null;
        }
    }
    public void BlockExitGround(BlockCard card, Ground ground)
    {
        if (ground.StandingUnit != null)
        {
            ground.IsSortie = false;
            ground.HasUnit = false;

            Destroy(ground.StandingUnit);
            ground.StandingUnit = null;
        }
    }
    public void UnitExitGround(Ground ground)
    {
        if (ground.StandingUnit != null)
        {
            Destroy(GameObject.Find("JoyStick(Clone)"));
            ground.IsSortie = false;
            ground.HasUnit = false;
            
            foreach (GameObject list in ground.StandingUnit.GetComponent<UnitOnDuty>().AttackFieldList)
            {
                list.GetComponent<Ground>().IsDirection = false;
                list.GetComponent<Ground>().AttFieldUnitList.Remove(ground.StandingUnit);
            }

            Destroy(ground.StandingUnit);
            ground.StandingUnit = null;
        }
    }

    public void UnSelectFieldUnit(Ground ground)
    {
        foreach (GameObject list in ground.StandingUnit.GetComponent<UnitOnDuty>().AttackFieldList)
        {
            list.GetComponent<Ground>().IsDirection = false;
        }
    }
    public void LoadSetUnitPrefab()
    {
        List<UnitData> unitdata = JsonManager.Instance.LoadUnitData();

        foreach (UnitData data in unitdata)
        {
            GameObject loadUnit = Resources.Load<GameObject>("Prefab/Unit/"+data.EnName);
            _unitDict.Add(data.EnName,loadUnit);
        }
    }
    private void SetEndCubePos()
    {
        GameObject endCube = GameObject.Find("End");
        endCube.transform.position = SetPosition(endCube.transform.position);
    }
    
    public Node[,] GetMap()
    {
        return _searchMap;
    }
    private void SetGroundList()
    {
        OverlapGroundDelete();

        if (_blockArr == null || _roadArr == null)
        { Debug.Log("SetGroundList arr is null"); return; }

        foreach (GameObject arr in _blockArr)
        {
            if (arr.GetComponent<Ground>()){ _groundList.Add(arr); }
        }

        foreach (GameObject arr in _roadArr)
        {
            if (arr.GetComponent<Ground>())
            {
                { _groundList.Add(arr); }
            }

            foreach (GameObject list in _destroyGroundList)
            {
                if(arr== list)
                {
                    _groundList.Remove(arr);
                }
            }
        }

    }
    private void OverlapGroundDelete()
    {
        foreach (GameObject block in _blockArr)
        {
            foreach (GameObject road in _roadArr)
            {
                if ((int)block.transform.position.x == (int)road.transform.position.x
                    && (int) block.transform.position.z == (int)road.transform.position.z ) 
                {
                    if (block.GetComponent<Ground>() == true)
                    {
                        _destroyGroundList.Add(road);
                        Destroy(road);
                    }
                    else continue;
                }
            }
        }
    }
    private void SetMap()
    {
        FindRoad();
        FindBlock();
        SetGroundList();
        SetMapSize();




        _searchMap = new Node[mapLength_X, mapLength_Z];

        foreach (GameObject block in _blockArr)
        {
            int x = (int)block.transform.position.x;
            int z = (int)block.transform.position.z;
            _searchMap[x, z] = new Node(x, z, true);
        }

        for (int x = 0; x < mapLength_X; x++)
        {
            for (int z = 0; z < mapLength_Z; z++)
            {
                if (_searchMap[x, z] == null)
                {
                    _searchMap[x, z] = new Node(x, z, false);
                }
            }
        }
    }
    private void SetMapSize()
    {
        foreach (GameObject block in _blockArr)
        {
            if (block.transform.position.x > mapLength_X)
            { mapLength_X = (int)block.transform.position.x; }
            if(block.transform.position.z > mapLength_Z)
            { mapLength_Z = (int)block.transform.position.z; }
        }

        mapLength_X += 1;
        mapLength_Z += 1;
        Debug.Log("MapX : " + mapLength_X + ", MapZ : " + mapLength_Z);
    }
   
    private void FindBlock() 
    {
        if (_blockArr != null) { _blockArr = null; }

        _blockArr = GameObject.FindGameObjectsWithTag("Block");
        
        foreach (GameObject block in _blockArr)
        {
            block.transform.position = SetPosition(block.transform.position);
        }
    }
    private void FindRoad()
    {
        if (_roadArr != null) { _roadArr = null; }

        _roadArr = GameObject.FindGameObjectsWithTag("Road");

        foreach (GameObject road in _roadArr)
        {
            road.transform.position = SetPosition(road.transform.position);
        }
    }

    public GameObject[] GetArrBlocks()
    {
        FindBlock();
        return _blockArr;
    }
    public Vector3 SetPosition(Vector3 position)
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
