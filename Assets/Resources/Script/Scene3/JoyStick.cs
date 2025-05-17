using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Direction
{
    Defaut, Top, Bottom, Right, Left
}
public class JoyStick : MonoBehaviour
    ,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Ground Ground;
    public GameObject Card;

    private RectTransform rect;
    private Vector2 touchPos;
    private Image panel;
    private Image stick;
    private Image dirUp;
    private Image dirDown;
    private Image dirRight;
    private Image dirLeft;
    private Button Exit;
    private Direction dir;
    private bool isDragging;
    private bool isStickInvi;
    private float scaleValue;
    private float Width;
    void Awake()
    {
        SetImage();
        rect = GetComponent<RectTransform>();
        Width = rect.rect.width;
        scaleValue = 1920 / (5f* Width);
        rect.localScale = new Vector3(scaleValue, scaleValue, 1);

    }
    void Start()
    {

    }
    void Update()
    {
        if (Ground.StandingUnit != null)
        {
            if (Ground.StandingUnit.GetComponent<UnitOnDuty>().DidDrop == true)
            {
                GameManager.Instance.UnitAttackField(Ground.StandingUnit);
                SeletedPos();
            }
            else if (Ground.StandingUnit.GetComponent<UnitOnDuty>().DidDrop == false)
            {
                GameManager.Instance.DeployUnit = true;
                if (Ground != null)
                {
                    GameManager.Instance.UnitGroundDeploy(Ground.gameObject);
                }
            }
        }
    }
    private void SetImage()
    {
        panel = GetComponent<Image>();
        Exit = transform.GetChild(0).gameObject.AddComponent<Button>();
        dirUp = transform.GetChild(1).GetComponent<Image>();
        dirDown = transform.GetChild(2).GetComponent<Image>();
        dirRight = transform.GetChild(3).GetComponent<Image>();
        dirLeft = transform.GetChild(4).GetComponent<Image>();
        stick = transform.GetChild(5).GetComponent<Image>();
        for (int i = 1; i <= 4; i++)
        {
            SetAlpha(transform.GetChild(i).GetComponent<Image>(), true);
        }

        Exit.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        if (Ground.IsSortie == true)
        {
            GameManager.Instance.DeployUnit = false;
            GameManager.Instance.UnitGroundDeploy(gameObject);

            CardSetActive(true);
        }

        if (dir==Direction.Defaut) 
        {
            Ground.ResetGround(Ground.StandingUnit.GetComponent<UnitOnDuty>().Card.GetComponent<UnitCard>());
            Destroy(gameObject);
        }

      
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        DragPosValue(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Ground.IsSortie && dir != Direction.Defaut) 
        {
            Ground.StandingUnit.GetComponent<UnitOnDuty>().SelectedAttField = true;
            Destroy(gameObject); 
        }

        stick.rectTransform.anchoredPosition = Vector2.zero;
        touchPos = Vector2.zero;
        isDragging = false;
    }
    
    private void SeletedPos()
    {
        if (Ground == null) { Debug.Log("Ground is null"); return; }
        if (Ground.IsSortie && !isStickInvi) 
        { 
            SetAlpha(stick, true);
            OnTargetRagerPadding(false);
            isStickInvi = true; 
        }
    }
    private void OnTargetRagerPadding(bool isPadding)
    {
        gameObject.GetComponent<Image>().raycastTarget = isPadding;
        for (int i = 1; i <= 4; i++)
        {
            transform.GetChild(i).GetComponent<Image>().raycastTarget = isPadding;
        }
    }
    private void DragPosValue(PointerEventData eventData)
    {
        if (!Ground.IsSortie)
        {
            touchPos = Vector2.zero;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                panel.rectTransform, eventData.position, eventData.pressEventCamera, out touchPos))
            {
                touchPos.x = (touchPos.x / panel.rectTransform.sizeDelta.x);
                touchPos.y = (touchPos.y / panel.rectTransform.sizeDelta.y);

                touchPos = new Vector2(touchPos.x * 2.2f, touchPos.y * 2.2f);

                touchPos = (touchPos.magnitude > 0.5f) ? touchPos.normalized * 0.5f : touchPos;

                stick.rectTransform.anchoredPosition
                    = new Vector2(
                        touchPos.x * panel.rectTransform.sizeDelta.x / 2,
                        touchPos.y * panel.rectTransform.sizeDelta.y / 2);
            }
            SetUnitDir();
        }
    }
    private void SetUnitDir()
    {
        Vector2 stickPos = stick.rectTransform.anchoredPosition;

        if (stickPos.y > 45f)
        {
            Ground.StandingUnit.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            dir = Direction.Top;
            OnDir(dir);
        }
        else if (stickPos.y < -45f)
        {
            Ground.StandingUnit.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            dir = Direction.Bottom;
            OnDir(dir);
        }
        else if (stickPos.x > 45f)
        {
            Ground.StandingUnit.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            dir = Direction.Right;
            OnDir(dir);
        }
        else if (stickPos.x < -45f)
        {
            Ground.StandingUnit.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            dir = Direction.Left;
            OnDir(dir);
        }
        else if(stickPos.y<=45f && stickPos.y >= -45f
            && stickPos.x <= 45f && stickPos.x >= -45f)
        {
            Ground.StandingUnit.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            dir = Direction.Defaut;
            OnDir(dir);
        }
    }
    private void SetAlpha(Image image, bool isInvisible)
    {
        Color color = image.color;
        if (isInvisible) { color.a = 0; }
        else { color.a = 1; }
        image.color = color;

       
    }
    private void OnDir(Direction dir)
    {
       
            GameManager.Instance.ResetGroundDir(Ground.StandingUnit.GetComponent<UnitOnDuty>());
            switch (dir)
            {
                case Direction.Defaut:
                    SetAlpha(transform.GetChild((int)Direction.Defaut)
                        .GetComponent<Image>(), false);
                    break;
                case Direction.Top:
                    SetAlpha(transform.GetChild((int)Direction.Top)
                        .GetComponent<Image>(), false);
                    break;
                case Direction.Bottom:
                    SetAlpha(transform.GetChild((int)Direction.Bottom)
                        .GetComponent<Image>(), false);
                    break;

                case Direction.Right:
                    SetAlpha(transform.GetChild((int)Direction.Right)
                        .GetComponent<Image>(), false);
                    break;
                case Direction.Left:
                    SetAlpha(transform.GetChild((int)Direction.Left)
                        .GetComponent<Image>(), false);
                    break;
            }
            Ground.StandingUnit.GetComponent<UnitOnDuty>().dir = dir;
        GameManager.Instance.UnitAttackField(Ground.StandingUnit);


        for (int i = 0; i <= 4; i++)
        {
            if(i==(int)dir){continue;}

            SetAlpha(transform.GetChild(i).GetComponent<Image>(), true);
        }

    }
    private void CardSetActive(bool isActive)
    {
        Card.SetActive(isActive);
        UIManager.Instance.IsSetActive = true;
    }
    private void OnDestroy()
    {
      
        if (Ground.StandingUnit.GetComponent<UnitOnDuty>().SelectedAttField == true
            && Ground.StandingUnit.GetComponent<UnitOnDuty>().DidDrop == false)
        {
            GameManager.Instance.DeployUnit = false;
            GameManager.Instance.UnitGroundDeploy(gameObject);
            Ground.IsSortie = true;
            Ground.StandingUnit.GetComponent<UnitOnDuty>().DidDrop = true;
            GameManager.Instance.ResetGroundDir(Ground.StandingUnit.GetComponent<UnitOnDuty>());
            foreach (GameObject list in Ground.StandingUnit.GetComponent<UnitOnDuty>().AttackFieldList)
            {
                list.GetComponent<Ground>().AttFieldUnitList.Add(Ground.StandingUnit);
            }
            GameManager.Instance.Cost -= Card.GetComponent<UnitCard>().unitData.Cost;
            CardSetActive(false);
        }
    }
}
