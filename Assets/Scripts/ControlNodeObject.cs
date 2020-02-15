using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeObject : MonoBehaviour
{

    Collider2D region;
    private float capturePercentage;

    private void Awake()
    {
        region = GetComponentInParent<Collider2D>();
        capturePercentage = 0f;
    }


    

}
