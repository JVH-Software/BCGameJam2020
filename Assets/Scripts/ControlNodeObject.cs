using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeObject : MonoBehaviour
{
    private const string WOLF_TAG = "Wolf";
    private const string BEAR_TAG = "Bear";

    private double capturePercentage;
    private int numTeamsOnNode;

    private ControlNode node;
    private const double CAPTURE_RATE = 1/3f;
    private ControlNode.Team occupyingTeam;

    SpriteRenderer renderer;
    private void Awake()
    {
        capturePercentage = 0f;
        node = new ControlNode();
        numTeamsOnNode = 0;
        occupyingTeam = node.getTeam();
        renderer = GetComponent<SpriteRenderer>();
        StartCoroutine("capture");
        
    }
    bool isEntity(string tag)
    {
        return (tag == WOLF_TAG|| tag == BEAR_TAG);
    }

    ControlNode.Team convertTagToTeam(string tag)
    {
        switch (tag)
        {
            case WOLF_TAG:
                return ControlNode.Team.Wolves;
            case BEAR_TAG:
                return ControlNode.Team.Bears;
            default:
                return ControlNode.Team.NoTeam;
        }
    }

    // For trigger implementation, the incoming gameobject must have a 

    //Enter sets contesting state
    private void OnTriggerEnter2D(Collider2D collider)
    {

        //Takes in an object, calls controlNode enter
        string tag = collider.gameObject.tag;
        if (isEntity(tag))
        {
            numTeamsOnNode++;
            ControlNode.Team incomingTeam = convertTagToTeam(tag);
            if(occupyingTeam != incomingTeam && numTeamsOnNode > 1)
            {
                node.setState(ControlNode.State.Contested);
                occupyingTeam = ControlNode.Team.NoTeam;
            }
            else if(occupyingTeam != incomingTeam)
            {
                occupyingTeam = incomingTeam;
            }

        }
    }

    //Stay sets capturing state, allows the occupying team(whatever is staying on the collider to capture)
    private void OnTriggerStay2D(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        if (isEntity(tag))
        {
            if(numTeamsOnNode == 1)
            {
                occupyingTeam = convertTagToTeam(tag);
                node.setState(ControlNode.State.Capturing);
            }
        }
    }

    //handles when a previous team leaves and when there is no one on tag
    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (isEntity(tag))
        {
            
            ControlNode.Team leavingTeam = convertTagToTeam(tag);
            Debug.Log("Object that has left: " + leavingTeam);
            numTeamsOnNode--;
            if (occupyingTeam == leavingTeam)
            {
                occupyingTeam = ControlNode.Team.NoTeam;
                capturePercentage = 0;
            }
            
            if(numTeamsOnNode == 0 && node.getState() != ControlNode.State.Captured)
            {
                node.setState(ControlNode.State.Empty);
            }
        }
    }
    /*private void Update()
    {
        /*Debug.Log("Control Point State: " + node.getState());
        Debug.Log("The Occupying Team: " + occupyingTeam);
        Debug.Log("Node Captured by: " + node.getTeam());
        Debug.Log("Number of factions at Node: " + numTeamsOnNode);
    }*/

    IEnumerator capture()
    { 
        while (true)
        {
            
            ControlNode.State nodeStatus = node.getState();

            switch (nodeStatus)
            {
                case ControlNode.State.Empty:
                    occupyingTeam = ControlNode.Team.NoTeam;
                    capturePercentage = 0;
                    renderer.color = Color.gray;
                    break;

                case ControlNode.State.Capturing:
                    if(capturePercentage < 1)
                    {
                        capturePercentage += CAPTURE_RATE * Time.deltaTime;
                        renderer.color = Color.green;
                    }
                    else
                    {
                        capturePercentage = 1;
                        node.setState(ControlNode.State.Captured);                        
                    }
                    break;
                case ControlNode.State.Contested:
                    renderer.color = Color.red;
                    break;
                case ControlNode.State.Captured:
                    node.setTeam(occupyingTeam);
                    if (node.getTeam() == ControlNode.Team.Bears)
                    {
                        renderer.color = Color.black;
                    }
                    else if (node.getTeam() == ControlNode.Team.Wolves)
                    {
                        renderer.color = Color.blue;
                    }
                    break;
                default:
                    break;
            }
            yield return null;
        }
        
    }
}
