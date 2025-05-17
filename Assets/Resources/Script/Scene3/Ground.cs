using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ground : MonoBehaviour
    , IPointerUpHandler, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    public bool IsOperation;
    public bool OnAttField;
    public bool HasUnit;
    public bool IsSortie;
    public bool IsDirection;
    public GameObject StandingUnit;
    public GameObject InsjoyPad;
    public GameObject Card;
    public List<GameObject> AttFieldUnitList;
    public List<GameObject> DetectedEnenmyList;

    private Renderer renderer;
    private Material defaultMaterial;
    private Material searchingMaterial;
    private Material attackFieldMaterial;
    private Vector3 unitPos;
    private string defaultPath;
    private string searchingPath;
    private string attackFieldPath;
    private void SetMaterialPath()
    {
        defaultPath = "Material/"+tag+ "DefaultMaterial";
        searchingPath = "Material/"+tag+"SearchingMaterial";
        attackFieldPath = "Material/AttackFieldMaterial";
    }
    void Awake()
    {
        AttFieldUnitList  = new List<GameObject>();
        DetectedEnenmyList =new List<GameObject>();
        LoadMatrerial();
        SetPos();
    }
  
    void Start()
    {
        
    }

    void Update()
    {
        SearchArea();
        if (StandingUnit != null && GameManager.Instance.Cusor == gameObject 
            && InsjoyPad==null && StandingUnit.GetComponent<UnitOnDuty>()==true)
        {
            InsjoyPad = GameManager.Instance.InsJoyStick(gameObject.GetComponent<Ground>());
        }
        else if (GameManager.Instance.Cusor != gameObject && InsjoyPad != null)
        {
            GameManager.Instance.UnSelectFieldUnit(gameObject.GetComponent<Ground>());
            Destroy(InsjoyPad);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        DetectEnemy(other);
    }
    void OnTriggerStay(Collider other)
    {
        DetectEnemy(other);
    }
    void OnTriggerExit(Collider other)
    {
        if (AttFieldUnitList.Count > 0
            && other.gameObject.tag == "Enemy")
        {
            bool overlapEnemy = false;
            foreach (GameObject list in DetectedEnenmyList)
            {
                if (list.gameObject == other.gameObject)
                {
                    overlapEnemy = true;
                }
            }
            if (overlapEnemy)
            {
                DetectedEnenmyList.Remove(other.gameObject);
            }
        }

    }
    private void DetectEnemy(Collider other)
    {
        if(AttFieldUnitList.Count>0
            &&other.gameObject.tag=="Enemy")
        {
            bool overlapEnemy = false;
           
            foreach (GameObject list in DetectedEnenmyList)
            {
                if(list.gameObject== other.gameObject)
                {
                    overlapEnemy = true;
                }
            }
            if(!overlapEnemy)
            {
                DetectedEnenmyList.Add(other.gameObject);
            }

            DetectedEnenmyList.RemoveAll(list => list == null);
        }
    }

    private void SetPos()
    {
        if (tag == "Block")
        {
            unitPos = transform.position + new Vector3(0, 0.5f, 0);
        }
        else { unitPos = transform.position + new Vector3(0, 0, 0); }
    }
    private void SearchArea()
    {
        if (IsDirection)
        {
            renderer.material = attackFieldMaterial;
        }
        else 
        {
            if (IsOperation)
            {
                if (GameManager.Instance.Cusor != null
                   && GameManager.Instance.Cusor.GetComponent<UnitCard>())
                {
                    UnitCard card = GameManager.Instance.Cusor.GetComponent<UnitCard>();
                    if (gameObject.tag == "Block" && card.Position == "Striker" && !OnAttField && !HasUnit)
                    {
                        renderer.material = searchingMaterial;
                    }
                    else if (gameObject.tag == "Road" && card.Position == "Guard" && !OnAttField && !HasUnit)
                    {
                        renderer.material = searchingMaterial;
                    }
                    else
                    {
                        ChangeToAttackField();
                    }
                }
                else if (GameManager.Instance.Cusor != null
                  && GameManager.Instance.Cusor.GetComponent<BlockCard>())
                {
                    if (gameObject.tag == "Road" && !OnAttField && !HasUnit)
                    {
                        renderer.material = searchingMaterial;
                    }
                    else
                    {
                        ChangeToAttackField();
                    }
                }
                else
                {
                    ChangeToAttackField();
                }
            }
            else
            {
                ChangeToAttackField();
            }
        }
    }
    private void ChangeToAttackField()
    {
        if (!OnAttField)
        { renderer.material = defaultMaterial; }
        else
        { renderer.material = attackFieldMaterial; }
    }
    public void LoadMatrerial()
    {
        SetMaterialPath();

        renderer = GetComponent<Renderer>();

        defaultMaterial = Resources.Load<Material>(defaultPath); 
        searchingMaterial = Resources.Load<Material>(searchingPath); 
        attackFieldMaterial = Resources.Load<Material>(attackFieldPath);
       
        
        if(defaultMaterial==null|| searchingMaterial==null|| attackFieldMaterial==null)
        { Debug.Log("Fail to Load Material"); }

        renderer.material = defaultMaterial;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
      
    }

    public void OnPointerUp(PointerEventData eventData)
    {

            GameManager.Instance.Cusor = gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HasUnit && IsOperation )
        {
            if (GameManager.Instance.Cusor != null
                   && GameManager.Instance.Cusor.GetComponent<UnitCard>())
            {
                UnitCard card = GameManager.Instance.Cusor.GetComponent<UnitCard>();
                Card = card.gameObject;
                if (card.IsDrag)
                {
                    if (gameObject.tag == "Block" && card.Position == "Striker")
                    {

                        GameManager.Instance.UnitOnGround(card, GetComponent<Ground>(), unitPos);
                    }
                    else if (gameObject.tag == "Road" && card.Position == "Guard")
                    {
                        GameManager.Instance.UnitOnGround(card, GetComponent<Ground>(), unitPos);
                    }
                }
            }
            else if(GameManager.Instance.Cusor != null
                  && GameManager.Instance.Cusor.GetComponent<BlockCard>())
            {
                BlockCard card = GameManager.Instance.Cusor.GetComponent<BlockCard>();

                if (card.IsDrag)
                {
                    if (gameObject.tag == "Road")
                    {
                        GameManager.Instance.BlockGround = gameObject;
                        GameManager.Instance.BlockOnGround(card, GetComponent<Ground>(), unitPos);
                    }
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!HasUnit && IsOperation)
        {
            ExitGround();
        }
    }
    public void ExitGround()
    {
        if (GameManager.Instance.Cusor != null
             && GameManager.Instance.Cusor.GetComponent<UnitCard>())
        {
            UnitCard card = GameManager.Instance.Cusor.GetComponent<UnitCard>();
            GameManager.Instance.UnitExitGround(card, GetComponent<Ground>());
        }
        else if (GameManager.Instance.Cusor != null
                  && GameManager.Instance.Cusor.GetComponent<BlockCard>())
        {
            BlockCard card = GameManager.Instance.Cusor.GetComponent<BlockCard>();
            GameManager.Instance.BlockExitGround(card, GetComponent<Ground>());
            GameManager.Instance.BlockGround = null;
        }
    }

    public void ResetGround(UnitCard card)
    {
            GameManager.Instance.UnitExitGround(card, GetComponent<Ground>());
    }
}
