using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    private static SlotManager _instance;
    public static SlotManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SlotManager>();
            }
            return _instance;
        }
    }
    public GameObject Cusor { get; set; }

    private GameObject ScrollView;
    private GameObject[] Units;


    public void ScrollActiveOn(bool OnOff)
    {
        ScrollView.SetActive(OnOff);
    }
    public void HideUnit(bool OnOff)
    {
        if (!OnOff)
        {
            Units = GameObject.FindGameObjectsWithTag("Unit");
        }
        if (Units != null)
        {
            foreach (GameObject unit in Units)
            {
                unit.SetActive(OnOff);
            }
        }
        if (OnOff) { Units = null; }
    }
   
    private void Awake()
    {
        ScrollView = GameObject.Find("SelectWindow");
    }

    void Update()
    {
     
    }
}
