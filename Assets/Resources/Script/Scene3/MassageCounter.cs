using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassageCounter : MonoBehaviour
{
    private float count;

    void Update()
    {
        count += Time.deltaTime;
        if (count >= 1) Destroy(gameObject);
    }
}
