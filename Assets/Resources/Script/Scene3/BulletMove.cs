using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public Transform TargetTr;
    public float Damage;

    private Transform tr;
    void Start()
    {
        tr = transform.GetChild(0).GetComponent<Transform>();
    }

    void Update()
    {
        tr.position= transform.position;
        if (TargetTr != null)
        {
            transform.position = Vector3.MoveTowards
                 (transform.position,
                 TargetTr.position + new Vector3(0, 0.5f, 0),
                 20.0f * Time.deltaTime);
        }
    }
    private void OnDestroy()
    {
        
    }
}
