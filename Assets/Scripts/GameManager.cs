using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    CapturePoint[] capturePoints;
    public float winningPercent = 0.5f;
    public GameObject packsContainer;
    internal Pack[] packs;
    public Tilemap ground;

    private void Start()
    {
        // GetComponents should always be tried to be called in Start rather than Update
        GameObject[] capturePointObjs = GameObject.FindGameObjectsWithTag("CapturePoint");
        capturePoints = new CapturePoint[capturePointObjs.Length];
        for(int i = 0; i < capturePoints.Length; i++)
        {
            capturePoints[i] = capturePointObjs[i].GetComponent<CapturePoint>();
        }

        // Find packs
        packs = new Pack[packsContainer.transform.childCount];
        for (int i = 0; i < packsContainer.transform.childCount; i++)
        {
            packs[i] = packsContainer.transform.GetChild(i).GetComponent<Pack>();
        }

    }

    private void Update()
    {
        int controlled = 0;
        for (int i = 0; i < capturePoints.Length; i++)
        {
            if (capturePoints[i].owner.Equals("Player"))
                controlled++;
        }
        if(((float)controlled)/capturePoints.Length >= winningPercent)
        {
            Debug.Log("Player Wins!");
        }
    }

    
}
