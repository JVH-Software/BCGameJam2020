using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{

    public Pack owner = null;
    public float ownership = 1;
    public float captureRate = 0.01f;
    public Territory territory;
    public Upgrades upgrade;

    private List<Collider2D> packs = new List<Collider2D>();

    private void Start()
    {
        if (owner == null)
        {
            territory.SetTerritoryColor(Color.white);
        }
        else
        {
            territory.SetTerritoryColor(Teams.teams[owner.tag]);
            owner.upgrades.Add(upgrade);
        }
    }

    void Update()
    {
        List<string> tags = new List<string>();
        foreach (Collider2D pack in packs)
        {
            if(!tags.Contains(pack.tag))
            {
                tags.Add(pack.tag);
            }
        }
        if(tags.Count == 1)
        {
            string attacker = tags[0];
            if(owner != null && attacker != owner.tag)
            {
                ownership -= captureRate;
                if(ownership < 0)
                {
                    // Ownership lost
                    Debug.Log(owner.tag + " just lost a control point!");
                    owner.upgrades.Remove(upgrade);
                    territory.SetTerritoryColor(Color.white);
                    owner = null;
                    ownership *= -1;
                }
            }
            else
            {
                ownership += captureRate;
                if(ownership >= 1)
                {
                    if(owner == null)
                    {
                        // Ownership won
                        Debug.Log(packs[0].tag + " just captured a control point!");
                        owner = packs[0].gameObject.GetComponent<PackMember>().pack;
                        territory.SetTerritoryColor(Teams.teams[attacker]);
                        packs[0].gameObject.GetComponent<PackMember>().pack.upgrades.Add(upgrade);
                    }
                    ownership = 1;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!packs.Contains(other) && Teams.teams.ContainsKey(other.tag)) {
            packs.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (packs.Contains(other))
        {
            packs.Remove(other);
        }
    }

}
