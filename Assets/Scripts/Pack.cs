using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{

    public float speedMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public Transform respawnPoint;
    public List<PackMember> packMembers;
    public PackMember packLeader;

    protected UpgradeList upgrades;

    public void Start()
    {
        upgrades = new UpgradeList(this);
    }
     
    public void Shoot(Vector3 target)
    {

        foreach (PackMember packMember in packMembers) {
            packMember.Shoot(target, this);
        }

    }


    public void Respawn()
    {
        foreach (PackMember packMember in packMembers)
        {
            packMember.health = packMember.maxHealth;
            packMember.transform.position = respawnPoint.position;
            packMember.dead = false;
        }
    }
}
