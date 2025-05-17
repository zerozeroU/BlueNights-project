using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitControl : MonoBehaviour
{
    protected Animator animator;
    public Ground ground;
    public GameObject Card;
    public UnitData data;

    private void Awake()
    {

    }
    void Start()
    {
        SceneCheck();
    }
    void Update()
    {
        SendValue();
    }
    private void SendValue()
    {
        if(ground!=null&& data!=null)
        {
            GetComponent<UnitOnDuty>().Ground = ground;
            GetComponent<UnitOnDuty>().unitdata = data;
            GetComponent<UnitOnDuty>().Card = Card;

            ground = null;
            data = null;
        }
    }
    private void SceneCheck()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        switch (currentScene.name)
        {
            case "SelectScene":
                {
                    SelectedUnit myUnit = gameObject.AddComponent<SelectedUnit>();
                    Debug.Log("Select SelectScene");
                    break;
                }
            case "BattleScene":
                {
                    UnitOnDuty myUnit = gameObject.AddComponent<UnitOnDuty>();
                    Debug.Log("Select BattleScene");
                    break;
                }
            default:
                Debug.Log("Not Applicable");
                break;
        }
    }
    public virtual void AniMotion()
    {
        animator = GetComponent<Animator>();
    }
   
}
