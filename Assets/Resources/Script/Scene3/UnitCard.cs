
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitCard : MonoBehaviour
    ,IPointerUpHandler, IPointerDownHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public UnitData unitData;
    public RectTransform Rect;
    public float Width;
    public int CardNum;
    public bool IsHide;
    public bool IsDrag;
    public string Position;

    private Sprite strikerSymbol;
    private Sprite guardSymbol;
    
    private Image portrait;
    private Image PosColor;
    private Image PosSymbol;
    private Text CostText;
    private Image redCover;
    private Image blackCover;
    private GameObject retireCoolTime;
    private RetireCoolTime InsRetireCoolTime;

    private float scaleValue;
    private float cardUpValue;
    private bool InsCard;
    private bool cardUp;
    private void Awake()
    {
        strikerSymbol = Resources.Load<Sprite>("Sprite/StrikerSymbol");
        guardSymbol = Resources.Load<Sprite>("Sprite/GuardSymbol");
        portrait = GetComponent<Image>();
        Rect = GetComponent<RectTransform>();
        Width = Rect.rect.width;
        PosSymbol = transform.GetChild(2).GetComponent<Image>();
        CostText = transform.GetChild(4).GetComponent<Text>();
        PosColor = transform.GetChild(5).GetComponent<Image>();
        blackCover = transform.GetChild(6).GetComponent<Image>();
        redCover = transform.GetChild(7).GetComponent<Image>(); 
        retireCoolTime = Resources.Load<GameObject>("Prefab/RebornCoolTime");
        CoverHide(redCover, true);
        CoverHide(blackCover, true);
    }
    void Start()
    {
    }
    void Update()
    {
        ManageCover();
        SetRectTransform();
        if (GameManager.Instance.Cusor != gameObject)
        {
            cardUp = false;
            IsDrag = false;
        }
    }
    private void OnEnable()
    {
        if (InsCard)
        {
            UIManager.Instance.CardEnumerate();
            InsRetireCoolTime = Instantiate(retireCoolTime, redCover.transform).GetComponent<RetireCoolTime>();
            InsRetireCoolTime.SetCount(unitData.Cost * 2);
        }
        InsCard = true;
    }

    private void OnDisable()
    {
       
    }
    private void ManageCover()
    {
        CoverHide(blackCover, true);

        if (InsRetireCoolTime!=null)
        {
            CoverHide(redCover, false);
        }
        else if(InsRetireCoolTime == null)
        {
            CoverHide(redCover, true);

            if (GameManager.Instance.Cost>= unitData.Cost)
            {
                CoverHide(blackCover, true);
            }
            else if(GameManager.Instance.Cost < unitData.Cost)
            {
                CoverHide(blackCover, false);
            }
        }
    }
    private void CoverHide(Image Cover, bool isHide)
    {
        Color color = Cover.color;

        if (isHide)
        {
            color.a = 0;
        }
        else
        {
            color.a = 200/225f;

        }
        Cover.color = color;

    }
   
    public void SetCard(UnitData data)
    {
        unitData = data;
        Position = data.Position;

        portrait.sprite = Resources.Load<Sprite>("Sprite/Portrait/Student_Portrait_" + data.EnName + "_Collection");
        CostText.text =""+ data.Cost;
        switch (data.Position)
        {
            case "Striker":
                PosSymbol.sprite = strikerSymbol;
                PosColor.color = Color.red;
                break;
            case "Guard":
                PosSymbol.sprite = guardSymbol;
                PosColor.color = Color.blue;
                break;
            default:
                PosColor.color = Color.gray;
                break;
        }
    }
    private void SetRectTransform()
    {
        scaleValue = 1920 / (11f * Width);
        Rect.localScale = new Vector3(scaleValue, scaleValue, 1);
        if(cardUp)
        {
            cardUpValue = 1080 / 20f;
        }
        else { cardUpValue = 0; }
            Rect.anchoredPosition = new Vector2((1920 / 11f) * CardNum * -1f, cardUpValue);
    }

  

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.Cost < unitData.Cost || InsRetireCoolTime != null) return;

        CardDrag();
    }
    private void CardDrag()
    {
        if (GameManager.Instance.Cost < unitData.Cost || InsRetireCoolTime != null) return;
       
        GameManager.Instance.Cusor = gameObject;
        GameManager.Instance.UnitGroundDeploy(gameObject);
        cardUp = true;
        IsDrag = true;
    }
    public void OnDrag(PointerEventData eventData)
    {

    }
    public void OnEndDrag(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.Instance.Cost < unitData.Cost || InsRetireCoolTime != null) return;

        if (!cardUp)
        {
                GameManager.Instance.Cusor = gameObject;
                cardUp = true;
        }
        else
        {
           UnitOnDuty unit;

            if (GameManager.Instance.Cusor == gameObject)
            {
                if (GameObject.Find(unitData.EnName+"(Clone)") != null)
                {
                    unit = GameObject.Find(unitData.EnName + "(Clone)").GetComponent<UnitOnDuty>();
                    unit.Card = gameObject;
                    unit.Ground.GetComponent<Ground>().HasUnit = true;
                    unit.HasJoystick = true;
                }
                GameManager.Instance.Cusor = null;
            }

          
            cardUp = false;
            IsDrag = false;
        }

    }
    private void OnCardEnumerate()
    {
        gameObject.SetActive(false);
    }
   

}
