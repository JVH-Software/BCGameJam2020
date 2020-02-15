using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeObject : MonoBehaviour
{

    Collider2D region;
    private float capturePercentage;
    private int numTeamsOnNode;

    private ControlNode node;
    private void Awake()
    {
        region = GetComponentInParent<Collider2D>();
        capturePercentage = 0f;
        node = new ControlNode();
        numTeamsOnNode = 0;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        numTeamsOnNode++;
        if(numTeamsOnNode > 1)
        {
            node.setState(ControlNode.State.Contested);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (numTeamsOnNode == 1)
        {
            node.setState(ControlNode.State.Capturing);
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        numTeamsOnNode--;
        if(numTeamsOnNode == 0)
        {
            node.setState(ControlNode.State.Empty);
        }
    }

    private void Update()
    {
        //printing the node state
        Debug.Log(node.getState());
        Debug.Log(numTeamsOnNode);
    }

    public ControlNode getControlNode()
    {
        return node;
    }
}
