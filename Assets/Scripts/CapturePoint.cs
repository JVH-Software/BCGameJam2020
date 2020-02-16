using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{

    public string owner = null;
    public float ownership = 1;
    public float captureRate = 0.01f;

    private List<Collider> packs = new List<Collider>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<string> tags = new List<string>();
        foreach (Collider pack in packs)
        {
            if(!tags.Contains(pack.tag))
            {
                tags.Add(pack.tag);
            }
        }
        if(tags.Count == 1)
        {
            string attacker = tags[0];
            if(attacker != owner)
            {
                ownership -= captureRate;
                if(captureRate < 0)
                {
                    owner = attacker;
                    ownership *= -1;
                }
            }
            else
            {
                ownership += captureRate;
                if(ownership > 1)
                {
                    ownership = 1;
                }
            }
        }
    }

    private void OnTrigger2DEnter(Collider other)
    {
        if (!packs.Contains(other) && (other.tag.Equals("Player") || other.tag.Equals("Wolf"))) {
            packs.Add(other);
        }
    }

    private void OnTrigger2DExit(Collider other)
    {
        if (packs.Contains(other))
        {
            packs.Remove(other);
        }
    }

}
