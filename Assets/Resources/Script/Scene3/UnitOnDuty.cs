using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UnitOnDuty : UnitControl
{
    public Ground Ground;
    public GameObject Card;
    public bool HasJoystick;
    public bool SelectedAttField;
    public bool IsEnemy;
    public bool DidDrop;
    public bool DidSelected;
    public Direction dir;
    public UnitData unitdata;
    public List<GameObject> AttackFieldList;
    public GameObject DetectedEnemy;
    public GameObject bullet;
    public Transform point;

    private string unitName;

    private void Awake()    
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(false);
        foreach (var arr in allChildren)
        {
            if (arr.name == "Point")
                point = arr;
        }
        transform.rotation= Quaternion.Euler(0f, 180f, 0f);
        AttackFieldList = new List<GameObject>();
        
        foreach (char c in gameObject.name)
        {
            if (c != '(')
            {
                unitName += c;
            }
            else if(c== '(')
            {
                break;
            }
        }
        bullet = Resources.Load<GameObject>("Prefab/Bullet/" + unitName + "Bullet");

    }
    void Start()
    {
    }

    void Update()
    {
        DetectEnemy();
        OnJoyPanel();
        AniMotion();
        if(DetectedEnemy!=null)
        {
            WatchEnenmy();
            IsEnemy = true;
        }
        else if(DetectedEnemy == null)
        {
            IsEnemy = false;
        }
    }
    public void Shot()
    {
        if (DetectedEnemy != null)
        {
            BulletMove insBullet = Instantiate(bullet).GetComponent<BulletMove>();
            insBullet.transform.position = point.position;
            insBullet.TargetTr = DetectedEnemy.transform;

         }
    }
    private void WatchEnenmy()
    {
        transform.LookAt(DetectedEnemy.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    }
    private void DetectEnemy()
    {
        bool overlapEnemy = false;

        foreach (GameObject list in AttackFieldList)
        {
            foreach (GameObject enemy in list.GetComponent<Ground>().DetectedEnenmyList)
            {
                if (DetectedEnemy==null)
                {
                    DetectedEnemy = enemy;
                }
                if (DetectedEnemy == enemy)
                {
                    overlapEnemy = true;
                }
            }
            if(!overlapEnemy)
            {
                DetectedEnemy = null;
            }
        }
    }
    private void OnJoyPanel()
    {
        if (HasJoystick)
        {
            GameManager.Instance.InsJoyStick(Ground);   
            HasJoystick = false;
        }
    }
    public override void AniMotion()
    {
        if (animator == null) { base.AniMotion(); }

        if (DidDrop)
        {
            if(IsEnemy)
            {
                animator.Play("Battle_Attack");
            }
            else 
            {
                animator.Play("Battle_Idle");
            }
        }
        else
        {
            animator.Play("Formation_Idle");
        }
    }

    private void OnDestroy()
    {
        if (Ground.IsSortie)
        {
           
        }
    }
}
