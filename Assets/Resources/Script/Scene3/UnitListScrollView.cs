using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListScrollView : MonoBehaviour
{
    private GameObject Portrait_Prefab;
    private List<UnitData> PortraitDataList;
    private ScrollRect scrollRect;
    private RectTransform content;
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
        scrollRect.scrollSensitivity = 10f;
        scrollRect.decelerationRate = 0.01f;
        scrollRect.horizontal = false;
        Prefeb_Load();

    }
    void Start()
    {
        PortraitDataList = JsonManager.Instance.LoadUnitData();
        Arrange_Portrait();
        SlotManager.Instance.ScrollActiveOn(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        SlotManager.Instance.HideUnit(false);
    }
    void OnDisable()
    {
        SlotManager.Instance.HideUnit(true);
    }

    private void Prefeb_Load()
    {
        Portrait_Prefab = Resources.Load<GameObject>("Prefab/Portrait");
        if(Portrait_Prefab==null)
        {
            Debug.Log("Fail to Prefab Load");
        }
    }
   
    private void Arrange_Portrait()
    {
        float posX=160, PosY=-220;
        int column = 0, row = 0;
        foreach (UnitData item in PortraitDataList)
        {
            GameObject portrait_clone = Instantiate(Portrait_Prefab, content.transform);
            portrait_clone.transform.GetChild(0).GetComponent<Image>().sprite
                = Resources.Load<Sprite>("Sprite/Portrait/Student_Portrait_" + item.EnName + "_Collection");
            portrait_clone.transform.GetChild(1).GetComponent<Text>().text = item.Name;
            portrait_clone.GetComponent<Portrait>().PortraitData = item;
            portrait_clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX+310* column, PosY- 430*row);

            column++;

            if(column == 6)
            {
                column = 0;
                row++;
                if (row > 1)
                {
                    content.sizeDelta 
                        = new Vector2(content.sizeDelta.x, content.sizeDelta.y + 400f);
                }
            }
           
        }
    }


}
