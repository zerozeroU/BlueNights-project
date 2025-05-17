using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
    ,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler,IDragHandler,IBeginDragHandler,IEndDragHandler, IDropHandler
{
    public GameObject DragUnit;
    public GameObject SlotPortrait;
    public UnitData SlotUnitData;
    public bool StartDrag;

    public GameObject _slotInUnit;
    public GameObject _addSymbol;
    public GameObject _positionSymbol;
    public GameObject _unitName;

    private Sprite _strikerSymbol;
    private Sprite _guardSymbol;

    private bool _pointerInSlot;
    private void Awake()
    {
        LoadSprite();
        GetChildObject();
    }
    void Start()
    {

    }
    void Update()
    {
        NameUpdate();
        SymbolUpdate();
        if (_slotInUnit != null && _slotInUnit.GetComponent<SelectedUnit>()) 
        { _slotInUnit.GetComponent<SelectedUnit>().AniMotion(); }
    }


    private void NameUpdate()
    {
        if(SlotUnitData!=null)
        {
            _unitName.GetComponent<Text>().text = SlotUnitData.Name;
        }
        else if(SlotUnitData== null)
        {
            _unitName.GetComponent<Text>().text = "";

        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        StartDrag = true;
        if (_slotInUnit != null) _slotInUnit.GetComponent<SelectedUnit>().Dragging = true;
        DragUnit = _slotInUnit;
    }
    private void UnitMotionChange()
    {
        if(DragUnit!=null)
        if (StartDrag)
        {
            DragUnit.GetComponent<SelectedUnit>().Dragging = true;
        }
        else
        {
            DragUnit.GetComponent<SelectedUnit>().Dragging = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        
            UnitPosition(Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0, 0, 100));
        
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        StartDrag = false;
        if (_slotInUnit != null) _slotInUnit.GetComponent<SelectedUnit>().Dragging = false;
        DragUnit = null;
        UnitPosition(transform.position);
    }
    public void OnDrop(PointerEventData eventData)
    {
        DragUnit = null;
        UnitSwap();
        UnitPosition(transform.position);
        if (_slotInUnit != null) _slotInUnit.GetComponent<SelectedUnit>().Dragging = false;

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerInSlot = true;
        AddSymbolScaleChange(_pointerInSlot);
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerInSlot = false;
        AddSymbolScaleChange(_pointerInSlot);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        SlotManager.Instance.Cusor = gameObject;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!StartDrag)
        {
            SlotManager.Instance.ScrollActiveOn(true);
        }
    }

    private void UnitPosition(Vector3 position)
    {
        if (DragUnit != null || _slotInUnit != null)
        {
            if (StartDrag)
            {
                DragUnit.transform.position = position - new Vector3(0, 0.560963f, 0.8f);
            }
            else
            {
                if (_slotInUnit != null)
                { _slotInUnit.transform.position = position - new Vector3(0, 0.560963f, 0.8f); }
            }
        }
    }
    

    public void GetUnit(GameObject portrait)
    {
        if (portrait.tag == "Portrait")
        {
            SlotPortrait = portrait;
            SlotUnitData = portrait.GetComponent<Portrait>().PortraitData;
            portrait.GetComponent<Portrait>().CoverAlpha(true);
            GameObject unit = Resources.Load<GameObject>("Prefab/Unit/" + SlotUnitData.EnName);
            _slotInUnit = Instantiate(unit);
            _slotInUnit.transform.position = transform.position;
        }
        else if (portrait.tag != "Portrait") { Debug.Log("It's not a Portrait"); }

    }

    private void UnitSwap()
    {
        if (SlotManager.Instance.Cusor==null
            ||SlotManager.Instance.Cusor.tag != "Slot") return;

        Slot slot = SlotManager.Instance.Cusor.GetComponent<Slot>();

        if (slot.StartDrag) { SlotTemp(slot); }
       
    }
    private void SlotTemp(Slot slot)
    {
        UnitData dataTemp = slot.SlotUnitData;
        GameObject unitTemp = slot._slotInUnit;
        GameObject portraitTemp = slot.SlotPortrait;


        slot.SlotUnitData = SlotUnitData;
        slot._slotInUnit = _slotInUnit;
        slot.SlotPortrait = SlotPortrait;

        SlotUnitData = dataTemp;
        _slotInUnit = unitTemp;
        SlotPortrait = portraitTemp;

        if (SlotPortrait != null)
        {
            SlotPortrait.GetComponent<Portrait>().Slot = gameObject.GetComponent<Slot>();
        }
        if (slot.SlotPortrait!=null)
        {
            slot.SlotPortrait.GetComponent<Portrait>().Slot = slot;
        }
    }
    public void SlotReset()
    {
        Destroy(_slotInUnit);
        SlotUnitData = null;
        _slotInUnit = null;
        SlotPortrait.GetComponent<Portrait>().CoverAlpha(false);
        SlotPortrait = null;
    }
    void SymbolUpdate()
    {
        if (_slotInUnit == null)
        {
            _positionSymbol.SetActive(false);
            _addSymbol.SetActive(true);
        }
        else if (_slotInUnit != null)
        {
            _positionSymbol.SetActive(true);
            _addSymbol.SetActive(false);

            if (SlotUnitData == null) return;
            switch (SlotUnitData.Position)
            {
                case "Striker":
                    _positionSymbol.GetComponent<Image>().sprite = _strikerSymbol;
                    break;
                case "Guard":
                    _positionSymbol.GetComponent<Image>().sprite = _guardSymbol;
                    break;
                default:
                    Debug.Log("Not Position");
                    break;
            }
        }
    }
    private void AddSymbolScaleChange(bool InOut)
    {
        if (InOut)
        {
            _addSymbol.transform.localScale = new Vector2(1.15f, 1.15f);
        }
        else
        {
            _addSymbol.transform.localScale = new Vector2(1, 1);
        }
    }
   
    private void LoadSprite()
    {
        _strikerSymbol = Resources.Load<Sprite>("Sprite/StrikerSymbol");
        _guardSymbol = Resources.Load<Sprite>("Sprite/GuardSymbol");

        if (_strikerSymbol == null || _guardSymbol == null)
        {
            Debug.Log("Fail to Load Symbol Sprite");
        }
    }
    private void GetChildObject()
    {
        _addSymbol = transform.GetChild(0).gameObject;
        _positionSymbol = transform.GetChild(2).gameObject;
        _unitName = transform.GetChild(3).gameObject;

        if (_addSymbol == null || _positionSymbol == null || _unitName == null)
        {
            Debug.Log("Fail to Get Child Object ");
        }
    }




}
