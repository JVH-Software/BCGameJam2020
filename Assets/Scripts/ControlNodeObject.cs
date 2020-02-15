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

    // Collision detection when entering region
    private void OnTriggerEnter2D(Collider2D collider)
    {
        numTeamsOnNode++;
        if(numTeamsOnNode > 1)
        {
            node.setState(ControlNode.State.Contested);
        }
    }

    // Collision detection when staying/capturing region
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (numTeamsOnNode == 1)
        {
            node.setState(ControlNode.State.Capturing);
        }
    }

    // Collision detection when leaving region
    private void OnTriggerExit2D(Collider2D collider)
    {
        numTeamsOnNode--;
        if(numTeamsOnNode == 0)
        {
            node.setState(ControlNode.State.Empty);
        }
        else if(numTeamsOnNode == 1){
            node.setState(ControlNode.State.Capturing);
        }
    }

    // Run every frame
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
