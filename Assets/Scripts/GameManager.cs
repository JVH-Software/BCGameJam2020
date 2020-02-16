using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    CapturePoint[] capturePoints;
    public float winningPercent = 0.5f;
    public GameObject packsContainer;
    internal Pack[] packs;
    public Tilemap ground;

    // Level Generation
    public int seed = 0;
    public int numEnemyPacks = 1;
    public int numStartingPackMembers = 1;

    public GameObject playerPackPrefab;
    public GameObject packPrefab;

    private void Start()
    {
        if (seed != 0)
        {
            UnityEngine.Random.InitState(seed);
        }

        // GetComponents should always be tried to be called in Start rather than Update
        GameObject[] capturePointObjs = GameObject.FindGameObjectsWithTag("CapturePoint");
        capturePoints = new CapturePoint[capturePointObjs.Length];
        for(int i = 0; i < capturePoints.Length; i++)
        {
            capturePoints[i] = capturePointObjs[i].GetComponent<CapturePoint>();
        }

        GenerateLevel();

        /*// Find packs
        packs = new Pack[packsContainer.transform.childCount];
        for (int i = 0; i < packsContainer.transform.childCount; i++)
        {
            packs[i] = packsContainer.transform.GetChild(i).GetComponent<Pack>();
        }*/

    }

    private void Update()
    {
        int controlled = 0;
        for (int i = 0; i < capturePoints.Length; i++)
        {
            if (capturePoints[i].owner != null && capturePoints[i].owner.tag.Equals("Player"))
                controlled++;
        }
        if(((float)controlled)/capturePoints.Length >= winningPercent)
        {
            Debug.Log("Player Wins!");
        }
    }

    void GenerateLevel()
    {
        if(numEnemyPacks+1 > Teams.teams.Count)
        {
            throw new UnityException();
        }


        // Generate Teams
        Dictionary<string, Color>.KeyCollection.Enumerator enumerator = Teams.teams.Keys.GetEnumerator();
        var enemyTeamTags = new List<string>();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current != "Player")
            {
                enemyTeamTags.Add(enumerator.Current);
            }
        }
        Utility.Shuffle(enemyTeamTags);

        // Generate Upgrades
        List<Upgrades> availableUpgrades = Enum.GetValues(typeof(Upgrades)).Cast<Upgrades>().ToList<Upgrades>();
        Utility.Shuffle(availableUpgrades);

        // Generate Packs, Control Points & Territories
        packs = new Pack[numEnemyPacks + 1];
        List<CapturePoint> capturePointsRandomized = new List<CapturePoint>(capturePoints);
        Utility.Shuffle(capturePointsRandomized);
        int capPointsPerTeam = capturePoints.Length / packs.Length;
        for(int i = 0; i < capturePoints.Length; i++)
        {
            capturePoints[i].RemoveOwnership();
            capturePoints[i].upgrade = availableUpgrades[i];
        }
        for (int i = 0; i < numEnemyPacks+1; i++)
        {
            for (int j = 0; j < capPointsPerTeam; j++)
            {
                if(j==0)
                {
                    Pack pack;
                    if (i == 0)
                    {
                        pack = GeneratePack("Player", capturePointsRandomized[i+j].transform.position);
                    }
                    else
                    {
                        pack = GeneratePack(enemyTeamTags[i], capturePointsRandomized[i + j].transform.position);
                    }
                    packs[i] = pack;
                }
                capturePointsRandomized[i + j].SetOwnership(packs[i]);
            }
        }
    }

    private Pack GeneratePack(string tag, Vector3 pos)
    {
        GameObject pack;
        if (tag == "Player")
        {
            pack = Instantiate(playerPackPrefab, packsContainer.transform);
            pack.transform.position = pos;
        }
        else
        {
            pack = Instantiate(packPrefab, packsContainer.transform);
            pack.transform.position = pos;
        }
        pack.tag = tag;
        //pack.transform.GetChild(0).transform.position = pos;
        //pack.transform.GetChild(0).tag = tag;
        for (int i = 0; i < numStartingPackMembers; i++)
        {
            pack.GetComponent<Pack>().AddMember();
        }
        return pack.GetComponent<Pack>();
    }



}
