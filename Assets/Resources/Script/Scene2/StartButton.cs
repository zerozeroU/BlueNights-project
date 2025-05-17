using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
    ,IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    private bool pointerIn;
    private void Awake()
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = new Vector2(1.1f, 1.1f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = new Vector2(1, 1);
        if (pointerIn) 
        { 
            JsonManager.Instance.ExportSlotUnitData();
            SceneManager.LoadScene("BattleScene");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIn = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        pointerIn = false;
    }
}
