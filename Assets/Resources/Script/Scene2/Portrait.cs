using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
    ,IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    private ScrollRect parentScrollRect;
    public UnitData PortraitData { get; set; }
    public Slot Slot { get; set; }
    private Image Cover;

    private bool isDragging;

    private void Awake()
    {
        parentScrollRect = GetComponentInParent<ScrollRect>();
        Cover=transform.GetChild(2).GetComponent<Image>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
   
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Slot == null) { CoverAlpha(true); }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Slot == null) { CoverAlpha(false); }
        Select();    }
    private void Select()
    {
        if (isDragging || SlotManager.Instance.Cusor == null ) return;
        
        if (SlotManager.Instance.Cusor.tag == "Slot")
        {
              if (PortraitData == null) { Debug.Log("PortraitData is Null"); }
           
              if (SlotManager.Instance.Cusor.GetComponent<Slot>())
              {
                Slot slot = SlotManager.Instance.Cusor.GetComponent<Slot>();

                if (slot.SlotUnitData ==null && slot != Slot && Slot == null ) { SlotIn(slot); }
                else if (slot == Slot) { SlotOut(slot); }
                else if (slot.SlotUnitData == null && slot != Slot && Slot != null) { SlotSwap(slot); }
                else if (slot.SlotUnitData != null && slot.SlotUnitData != PortraitData&& Slot != null) { PortraitSwap(slot); }
                else if (slot.SlotUnitData != null && slot.SlotUnitData != PortraitData && Slot == null) { PortraitChange(); }

                SelectEnd();
            }
            else { Debug.Log("Fail to GetComponent<Slot>()"); }
        }
        else if (SlotManager.Instance.Cusor.tag != "Slot")
        {
            Debug.Log("Tag is Not Slot");
        }
    }
    private void SelectEnd()
    {
        SlotManager.Instance.Cusor = null;
        SlotManager.Instance.ScrollActiveOn(false);
    }
    private void SlotIn(Slot slot)
    {
        Debug.Log("Slot In");

        slot.GetUnit(gameObject);
        Slot = slot;
    }
    private void SlotOut(Slot slot)
    {
        Debug.Log("SlotOut");

        slot.SlotReset();
        Slot = null;
    }
    private void SlotSwap(Slot slot)
    {
        Debug.Log("SlotSwap");

        Slot.SlotReset();
        slot.GetUnit(gameObject);
        Slot = slot;
    }
  
    private void PortraitSwap(Slot slot)
    {
        Debug.Log("PortraitSwap");

        GameObject portrait = slot.SlotPortrait;
        Slot tempSlot = portrait.GetComponent<Portrait>().Slot;

        slot.SlotReset();
        Slot.GetComponent<Slot>().SlotReset();
        slot.GetUnit(gameObject); // slot == game -> my
        Slot.GetComponent<Slot>().GetUnit(portrait); // my == port -> slot
        portrait.GetComponent<Portrait>().Slot = Slot;
        Slot = tempSlot;

    }
    private void PortraitChange()
    {
        Debug.Log("PortraitChange");

        SlotManager.Instance.Cusor.GetComponent<Slot>().SlotReset();
        SlotManager.Instance.Cusor.GetComponent<Slot>().GetUnit(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
            parentScrollRect.OnBeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
            parentScrollRect.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
            parentScrollRect.OnEndDrag(eventData);
    }

    public void CoverAlpha(bool DownUp)
    {
            Color color = Cover.color;
            if (DownUp)
            {
                color.a = 0.6f;
            }
            else
            {
                color.a = 0f;
            }
            Cover.color = color;
    }
}
