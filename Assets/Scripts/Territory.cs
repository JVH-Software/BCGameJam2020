using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{

    public void SetTerritoryColor(Color color)
    {
        for (int i = 0; i < transform.childCount; i++) {
            SpriteRenderer renderer = transform.GetChild(i).GetComponent<SpriteRenderer>();
            color.a = 0.2f;
            renderer.color = color;
        }
    }

}
