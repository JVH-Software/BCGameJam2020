using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public enum States {intro, main, paused};
    private const string CONTROL_POINT_TAG = "control point";
    public GameObject[] controlPoints;
    public List<ControlNodeObject> ControlNodeObjects = new List<ControlNodeObject>();
  
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private void Start()
    {
        controlPoints = GameObject.FindGameObjectsWithTag(CONTROL_POINT_TAG);

        foreach (GameObject point in controlPoints)
        {
            ControlNodeObject node = point.GetComponent<ControlNodeObject>();
            ControlNodeObjects.Add(node);
        }
    }


    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (this == instance)
        {
            instance = null;
        }
    }

    
    public ControlNodeObject getNode(int index)
    {
        //return nodeList[index];
        return null;
    }






}
