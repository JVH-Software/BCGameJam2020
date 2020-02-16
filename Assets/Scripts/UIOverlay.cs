using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlay : MonoBehaviour
{
    private SimpleHealthBar healthBar;

    public void UpdateHealthBar(float health, float maxHealth) {
        healthBar?.UpdateBar(health, maxHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<SimpleHealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
