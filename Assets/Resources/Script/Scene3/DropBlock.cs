using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBlock : MonoBehaviour
{
    public Ground ground;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnDestoryBlock = gameObject;
    }
}
