using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUnit : UnitControl
{
    public bool Dragging { get; set; }
    public GameObject Slot { get; set; }
    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.position -= new Vector3(0, 0.560963f, 0.8f);
    }
    void Start()
    {
        
    }
    public override void AniMotion()
    {
        if (animator == null) { base.AniMotion(); }

        if (Dragging)
        {
            animator.Play("Formation_Pickup");
        }
        else { animator.Play("Formation_Idle"); }
    }
    
    void Update()
    {
    }
}
