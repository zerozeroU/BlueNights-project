using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostProducer : MonoBehaviour
{

    private Image fill;
    private Text costText;
    private float fillMaxValue;
    private float fillValue;

    private void Awake()
    {
        fill = transform.GetChild(2).GetComponent<Image>();
        costText = transform.GetChild(3).GetComponent<Text>();
        fillMaxValue = 1.5f;
        GameManager.Instance.Cost = 0;
    }

    void Update()
    {
        fillValue += Time.deltaTime;
        fill.fillAmount = fillValue / fillMaxValue;
        if(fillValue >= fillMaxValue)
        {
            fill.fillAmount = 0;
            fillValue = 0;
            GameManager.Instance.Cost++;
            if(GameManager.Instance.Cost>99)
            {
                GameManager.Instance.Cost--;
            }
        }

        costText.text = "" + GameManager.Instance.Cost;
    }
}
