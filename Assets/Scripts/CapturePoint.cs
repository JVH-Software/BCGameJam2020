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
    public GameManager gameManager;
    public string capturerName;

    private List<Collider2D> packs = new List<Collider2D>();

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void SetOwnership(string newOwner)
    {

        territory.SetTerritoryColor(Teams.teams[newOwner]);
        foreach (Pack p in gameManager.packs)
        {
            if (p.tag.Equals(newOwner))
                p.upgrades.Add(upgrade);
        }
        owner = newOwner;
        ownership = 1;
    }

    public void RemoveOwnership()
    {
        territory.SetTerritoryColor(Color.white);

        if (owner == "")
        {
            ownership = 0;
        }
        else
        {
            foreach(Pack p in gameManager.packs)
            {
                if(p.tag.Equals(owner))
                    p.upgrades.Remove(upgrade);
            }
            owner = "";
            ownership *= -1;
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
        if(tags.Count == 0)
        {
            capturerName = "";
        }
        if(tags.Count == 1)
        {
            string attacker = tags[0];
            capturerName = attacker;

            if (owner != "" && attacker != owner)

            {
                ownership -= captureRate;
                if(ownership < 0)
                {
                    // Ownership lost
                    RemoveOwnership();
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
                        RemoveOwnership();
                        SetOwnership(attacker);
                    }
                    ownership = 1;
                }
            }
        }
    }

    public bool isContested() { return packs.Count >= 2;  }
    public bool IsBeingCaptured() { return ownership < 1 && capturerName != "";  }
    public bool IsOwned() { return owner != "";  }

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
