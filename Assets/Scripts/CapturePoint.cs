using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{

    public string owner = "";
    public float ownership = 1;
    public float captureRate = 0.01f;
    public Territory territory;
    public Upgrades upgrade;

    private List<Collider2D> packs = new List<Collider2D>();
    
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
            if(attacker != owner && owner != "")
            {
                ownership -= captureRate;
                if(ownership < 0)
                {
                    // Ownership lost
                    GameObject.FindGameObjectWithTag(owner).GetComponent<PackMember>().pack.upgrades.Remove(upgrade);
                    territory.SetTerritoryColor(Color.white);
                    owner = "";
                    ownership *= -1;
                }
            }
            else
            {
                ownership += captureRate;
                if(ownership >= 1)
                {
                    if(owner == "")
                    {
                        // Ownership won
                        owner = attacker;
                        territory.SetTerritoryColor(Teams.teams[attacker]);
                        GameObject.FindGameObjectWithTag(owner).GetComponent<PackMember>().pack.upgrades.Add(upgrade);
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
