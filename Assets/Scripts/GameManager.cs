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
    internal Pack[] packs = new Pack[0];
    public Tilemap ground;
    public UIOverlay healthbar;

    public GameObject playerPackPrefab;
    public GameObject packPrefab;

    private GlobalData gd;
    private int numEnemyTeams;
    private int numStartingPackMembers;
    private int numPacksPerTeam;

    private void Start()
    {

        gd = GameObject.Find("GlobalData").GetComponent<GlobalData>();

        if (gd.seed == 0)
        {
            gd.seed = UnityEngine.Random.Range(0, 10000);
        }
        UnityEngine.Random.InitState(gd.seed);

        // GetComponents should always be tried to be called in Start rather than Update
        GameObject[] capturePointObjs = GameObject.FindGameObjectsWithTag("CapturePoint");
        capturePoints = new CapturePoint[capturePointObjs.Length];
        for(int i = 0; i < capturePoints.Length; i++)
        {
            capturePoints[i] = capturePointObjs[i].GetComponent<CapturePoint>();
        }

        GenerateLevel(gd.level);

    }

    private void Update()
    {
        int controlled = 0;
        for (int i = 0; i < capturePoints.Length; i++)
        {
            if (capturePoints[i].owner != null && capturePoints[i].owner.Equals("Player"))
                controlled++;
        }
        if(((float)controlled)/capturePoints.Length >= winningPercent)
        {
            Debug.Log("Player Wins!");
        }
    }

    void GenerateLevel(int level)
    {
        numEnemyTeams = allLevelParameters[level].numEnemyTeams;
        numStartingPackMembers = allLevelParameters[level].numStartingPackMembers;
        numPacksPerTeam = allLevelParameters[level].numPacksPerTeam;
        numStartingPackMembers = 4;

        if(numEnemyTeams + 1 > Teams.teams.Count)
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

        // Generate Control Points & Territories
        List<CapturePoint> capturePointsRandomized = new List<CapturePoint>(capturePoints);
        Utility.Shuffle(capturePointsRandomized);
        int capPointsPerTeam = capturePoints.Length / (numEnemyTeams+1);
        for(int i = 0; i < capturePoints.Length; i++)
        {
            capturePoints[i].RemoveOwnership();
            capturePoints[i].upgrade = availableUpgrades[i];
        }
        int count = 0;
        for (int i = 0; i < numEnemyTeams + 1; i++)
        {
            for (int j = 0; j < capPointsPerTeam; j++)
            {
                capturePointsRandomized[count].gameManager = this;
                if (i == 0)
                {
                    capturePointsRandomized[count].SetOwnership("Player");
                }
                else
                {
                    capturePointsRandomized[count].SetOwnership(enemyTeamTags[i - 1]);
                }
                count++;

            }
        }

        // Generate Packs
        packs = new Pack[(numEnemyTeams + 1)*numPacksPerTeam];
        count = 0;
        for(int i = 0; i < numEnemyTeams+1; i++)
        {
            for(int j = 0; j < numPacksPerTeam; j++)
            {
                if(i == 0)
                {
                    if (j == 0)
                    {
                        packs[count] = GeneratePack("Player", FindRandomOwnedPoint("Player").transform.position, true);
                    }
                    else
                    {
                        packs[count] = GeneratePack("Player", FindRandomOwnedPoint("Player").transform.position, false);
                    }

                }
                else
                {
                    packs[count] = GeneratePack(enemyTeamTags[i - 1], FindRandomOwnedPoint(enemyTeamTags[i - 1]).transform.position, false);

                }
                count++;
            }
        }

        //Quick easy way to apply upgrades to packs (repeat of earlier loop, but now we have packs to apply upgrades to)
        count = 0;
        for (int i = 0; i < numEnemyTeams + 1; i++)
        {
            for (int j = 0; j < capPointsPerTeam; j++)
            {
                if (i == 0)
                {
                    capturePointsRandomized[count].SetOwnership("Player");
                }
                else
                {
                    capturePointsRandomized[count].SetOwnership(enemyTeamTags[i - 1]);
                }
                count++;

            }
        }

    }

    public CapturePoint FindRandomOwnedPoint(string tag)
    {
        List<CapturePoint> foundPoints = new List<CapturePoint>();
        foreach(CapturePoint cp in capturePoints)
        {
            if (tag.Equals(cp.owner))
                foundPoints.Add(cp);
        }
        Utility.Shuffle(foundPoints);
        if(foundPoints.Count == 0)
        {
            return null;
        }
        return foundPoints[0];
    }

    private Pack GeneratePack(string tag, Vector3 pos, bool player)
    {
        GameObject pack;
        if (player)
        {
            pack = Instantiate(playerPackPrefab, packsContainer.transform);
            pack.transform.position = pos;
            pack.GetComponent<PlayerPack>().overlay = healthbar;
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

    public struct LevelParameters
    {
        public int numEnemyTeams;
        public int numStartingPackMembers;
        public int numPacksPerTeam;

        public LevelParameters(int numEnemyTeams, int numStartingPackMembers, int numPacksPerTeam)
        {
            this.numEnemyTeams = numEnemyTeams;
            this.numStartingPackMembers = numStartingPackMembers;
            this.numPacksPerTeam = numPacksPerTeam;
        }
    }

    private LevelParameters[] allLevelParameters = {
        new LevelParameters(1,1,1), // Level 1
        new LevelParameters(1,2,1), // Level 2
        new LevelParameters(2,2,1), // Level 3
        new LevelParameters(2,2,1), // Level 4
        new LevelParameters(2,3,1), // Level 5
        new LevelParameters(2,3,1), // Level 6
        new LevelParameters(2,2,2), // Level 7
        new LevelParameters(3,2,1), // Level 8
        new LevelParameters(3,3,2), // Level 9
        new LevelParameters(3,3,3)  // Level 10
    };

}
