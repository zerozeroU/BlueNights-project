using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    public GameObject canvas { get; set; }
    List<UnitData> unitDataList;
    public GameObject card;
    public bool IsSetActive;
    List<GameObject> cardList;

    private void Awake()
    {
        Awake_Initialize();
    }
    private void Awake_Initialize()
    {
        canvas = GameObject.Find("Canvas");
        unitDataList = JsonManager.Instance.LoadUnitData();
        card = Resources.Load<GameObject>("Prefab/Card");
        cardList = new List<GameObject>();
    }
    void Start()
    {
        Start_Initialize();
    }
    private void Start_Initialize()
    {
        SetInsCard();
        ListCard();
    }

    public void CardEnumerate()
    {
        int CardNumCount = 1;
        foreach (GameObject card in cardList)
        {
            
            if(card.GetComponent<UnitCard>()==true)
            {
                if(card.activeSelf==true)
                {
                    card.GetComponent<UnitCard>().CardNum = CardNumCount;
                    CardNumCount++;
                }
            }
        }
    }

    private void ListCard()
    {
        foreach (GameObject list in cardList)
        {
            if (list.GetComponent<UnitCard>() == true)
            {
                UnitCard slideCard = list.GetComponent<UnitCard>();
            }
        }
    }
    private void SetInsCard()
    {
        unitDataList.Reverse();
        int count = 1;
        foreach (UnitData unitData in unitDataList)
        {
            GameObject cardTemp = Instantiate(card, canvas.transform);
            UnitCard slideCard = cardTemp.GetComponent<UnitCard>();
            slideCard.CardNum = count;
            slideCard.SetCard(unitData);
            cardList.Add(cardTemp);
            count++;
        }
        GameObject BlockCard = Instantiate(Resources.Load<GameObject>("Prefab/BlockCard"), canvas.transform);
        cardList.Add(BlockCard);
    }

  
    void Update()
    {
        if(IsSetActive)
        {
            CardEnumerate();
            IsSetActive = false;
        }
    }

}
