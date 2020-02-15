using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeObject : MonoBehaviour
{

    Collider2D region;
    private double capturePercentage;
    private int numTeamsOnNode;

    private ControlNode node;
    private const double CAPTURE_RATE = 1/4f;
    private ControlNode occupyingTeam;

    private void Awake()
    {
        region = GetComponentInParent<Collider2D>();
        capturePercentage = 0f;
        node = new ControlNode();
        numTeamsOnNode = 0;
        occupyingTeam = occupyingTeam.NoTeam;
        StartCoroutine("capture");
    }

    // For trigger implementation, the incoming gameobject must have a 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        numTeamsOnNode++;
        if(numTeamsOnNode > 1)
        {
            node.setState(ControlNode.State.Contested);
        }
        else
        {
            occupyingTeam = collider.gameObject.tag;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (numTeamsOnNode == 1 && node.getState() != ControlNode.State.Contested)
        {
            node.setState(ControlNode.State.Capturing);
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        numTeamsOnNode--;
        if(numTeamsOnNode == 0)
        {
            occupyingTeam = "NoTeam";
            node.setState(ControlNode.State.Empty);
        }
        else if(numTeamsOnNode == 1 && node.getState() != ControlNode.State.Contested)
        {
            node.setState(ControlNode.State.Capturing);
        }
    }

    public ControlNode getControlNode()
    {
        return node;
    }
    public void Update()
    {
        Debug.Log(occupyingTeam);
    }
    IEnumerator capture()
    { 
        while (true)
        {
            ControlNode.State nodeStatus = node.getState();
            if(capturePercentage >= 1 && nodeStatus != ControlNode.State.Contested)
            {
                Debug.Log("Captured");
                node.setState(ControlNode.State.Captured);
            }
            else if (nodeStatus == ControlNode.State.Capturing && capturePercentage < 1)
            {
                Debug.Log("Capturing State");
                capturePercentage += CAPTURE_RATE * Time.deltaTime;
                Debug.Log(capturePercentage);
            }
            else if(nodeStatus == ControlNode.State.Contested)
            {
                Debug.Log("Contested!");
            }
            else if(nodeStatus == ControlNode.State.Empty)
            {
                Debug.Log("Lost State");
                capturePercentage = 0;
            }

            yield return null;

        }
        
    }
}
