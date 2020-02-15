using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{

    private float health = 10f;
    private float maxHealth = 10f;
    public float speedMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public SimpleHealthBar healthBar;
    public Transform respawnPoint;
    public int respawnTime = 500;
    public List<PackMember> packMembers;
    public PackMember packLeader;

    private int respawnDelay = 0;
    private bool dead = false;

    protected UpgradeList upgrades;

    public void Start()
    {
        upgrades = new UpgradeList(this);
    }

    public void FixedUpdate()
    {
        PerformMovement();
    }

    public void Update()
    {
        respawnDelay--;
        Debug.Log(respawnDelay);
        if (respawnDelay < 0)
        {
            respawnDelay = 0;
            if(dead)
            {
                dead = false;
                foreach (PackMember packMember in packMembers)
                {
                    packMember.health = packMember.maxHealth;
                }
                UpdateHealth();
                transform.position = respawnPoint.position;
            }
        }
    }

    private void PerformMovement()
    {

        if (dead)
            return;
    }
     
    public void Shoot(Vector3 target)
    {
        if (dead)
            return;

        foreach (PackMember packMember in packMembers) {
            packMember.Shoot(target, this);
        }

    }

    internal void UpdateHealth()
    {
        float totalHealth = 0f;
        float totalMaxHealth = 0f;
        foreach (PackMember packMember in packMembers)
        {
            totalHealth = packMember.health;
            totalMaxHealth = packMember.maxHealth;
        }
        health = totalHealth;
        maxHealth = totalMaxHealth;

        if (healthBar != null)
        {
            healthBar.UpdateBar(health, maxHealth);
        }
    }

    private void Death()
    {
        dead = true;
        respawnDelay = respawnTime;
    }
}
