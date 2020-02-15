using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTerritoryColor(Color color)
    {
        for (int i = 1; i < transform.childCount; i++) {
            SpriteRenderer renderer = transform.GetChild(i).GetComponent<SpriteRenderer>();
            renderer.color = color;
        }
    }

}
