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
    public List<ControlNode> controlNodes = new List<ControlNode>();
  
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
            ControlNode node = point.GetComponent<ControlNode>();
            controlNodes.Add(node);
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

    public ControlNode getNode(int index)
    {
        //return nodeList[index];
        return null; 
    }


}
