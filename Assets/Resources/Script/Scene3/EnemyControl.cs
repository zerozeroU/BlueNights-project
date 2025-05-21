using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyData
{
    public EnemyData(int _Hp, int _Atk, int _Def, int _Dex)
    {
        Hp= _Hp;
        Atk= _Atk;
        Def= _Def;
        Dex= _Dex;
    }
    public int Hp;
    public int Atk;
    public int Def;
    public int Dex;
}
public class EnemyControl : MonoBehaviour
{
    public EnemyData Status;
    private GameObject detectedUnit;
    private NaviGator navi;
    private Animator animator;
    private GameObject hpBar;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        navi= gameObject.GetComponent<NaviGator>();
        SetStatus();
    }
    void Start()
    {
        hpBar = Instantiate(Resources.Load<GameObject>("Prefab/HpBar"),GameManager.Instance.canvas.transform);
        hpBar.GetComponent<HpBar>().unit = gameObject;
    }

    void Update()
    {
        AniMotion();
    }

    void OnTriggerEnter(Collider other)
    {
        DetectUnit(other);
    }

    void OnTriggerStay(Collider other)
    {
        DetectUnit(other);
    }
    private void DetectUnit(Collider other)
    {
        if (detectedUnit == null && other.gameObject.GetComponent<Ground>() == true)
        {
            Ground ground = other.gameObject.GetComponent<Ground>();
            if (ground.IsSortie == true && ground.StandingUnit.tag == "Unit")
            {
                detectedUnit = other.gameObject;
            }
        }
    }
    private void AniMotion()
    {
        if(detectedUnit==null)
        {
            navi.Speed = Status.Dex;
            animator.Play("Move");
        }
        else if (detectedUnit != null)
        {
            navi.Speed = 0;
            animator.Play("Attack");
        }
    }
 
    private void SetStatus()
    {
        int hp = Random.Range(500, 2000);
        int atk = Random.Range(10, 100);
        int def = Random.Range(1, 10);
        int dex = Random.Range(1, 3);
        Status = new EnemyData(hp, atk, def, dex);
    }
   
}
