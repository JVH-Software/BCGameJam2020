using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
  
    public const int NUM_NODES = 5;
    private ControlNode[] nodeList;

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public ControlNode getNode(int index)
    {
        return nodeList[index];
    }


}
