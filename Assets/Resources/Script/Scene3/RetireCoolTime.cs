using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetireCoolTime : MonoBehaviour
{
    private Image cycleFill;
    private Text countText;
    private float CountValue;
    private float CountMaxValue;
    private bool CountStart;

    private void Awake()
    {
        cycleFill = transform.GetChild(0).GetComponent<Image>();
        countText = transform.GetChild(1).GetComponent<Text>();
    }
  
    void Update()
    {
        if(CountStart == true)
        {
            CountValue += Time.deltaTime;
            float f_point = ((CountMaxValue - CountValue) - (int)(CountMaxValue - CountValue))*10;
            int i_point = (int)f_point;
            countText.text = "" + (int)(CountMaxValue - CountValue) + "."+ i_point;
            cycleFill.fillAmount = CountValue / CountMaxValue;

            if(CountValue >= CountMaxValue)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetCount(float value)
    {
        CountMaxValue=value;
        if(CountMaxValue!=0)
        {
            CountStart=true;
        }
    }
}
