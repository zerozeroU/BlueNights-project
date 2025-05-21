using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public GameObject unit;
    private bool selectFillColor;
    private Image Fill;
    void Awake()
    {
        Fill=transform.GetChild(0).GetComponent<Image>();
    }
    private void ChangeColor()
    {
        Color color = Fill.color;
        switch (unit.tag)
        {
            case "Unit":
                color = Color.green;
                break;
            case "Enemy":
                color = Color.red;
                break;
        }
        Fill.color=color;
    }
    void Update()
    {
       if (selectFillColor)
        {
            if (unit == null)
            {
                Destroy(gameObject);
            }
        }

        if (unit != null)
        {
            if (!selectFillColor)
            {
                ChangeColor();
                selectFillColor = true;
            }

            float posX = unit.transform.position.x;
            float posZ = unit.transform.position.z;

            Vector3 pos = unit.transform.position + new Vector3( (posX - 5) * 0.055f, 0, (posZ - 6) /12f);
         
            transform.position = Camera.main.WorldToScreenPoint(pos);
        }
    }
}
