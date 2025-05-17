
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockCard : MonoBehaviour
        , IPointerUpHandler, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform Rect;
    public float Width;
    public bool IsHide;
    public bool IsDrag;
    private bool cardUp;

    private float scaleValue;
    private float cardUpValue;
    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        Width = Rect.rect.width;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetRectTransform();
        if (GameManager.Instance.Cusor != gameObject)
        {
            cardUp = false;
            IsDrag = false;
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.ResearchRoad = true;
        GameManager.Instance.OnDestoryBlock = gameObject;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        CardDrag();
    }
    private void CardDrag()
    {
        GameManager.Instance.Cusor = gameObject;
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
        if (!cardUp)
        {
            GameManager.Instance.Cusor = gameObject;
            cardUp = true;
        }
        else
        {

            if (GameManager.Instance.Cusor == gameObject)
            {
                if(GameManager.Instance.BlockGround!=null)
                {
                    Ground ground = GameManager.Instance.BlockGround.GetComponent<Ground>();
                    if (ground.StandingUnit.tag=="Block")
                    {
                       GameManager.Instance.LastDropBlock = ground.StandingUnit;
                    }
                    ground.HasUnit = true;
                    GameManager.Instance.ResearchRoad = true;
                    GameManager.Instance.BlockGround = null;
                }
                GameManager.Instance.Cusor = null;
            }


            cardUp = false;
            IsDrag = false;
        }

    }
    private void SetRectTransform()
    {
        scaleValue = 1920 / (11f * Width);
        Rect.localScale = new Vector3(scaleValue, scaleValue, 1);
        if (cardUp)
        {
            cardUpValue = 1080 / 20f;
        }
        else { cardUpValue = 0; }
        Rect.anchoredPosition = new Vector2((1920 / 11f) * 0 * -1f, cardUpValue);
    }
}
