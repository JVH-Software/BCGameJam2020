using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeObject : MonoBehaviour
{
    private const string WOLF_TAG = "Wolf";
    private const string BEAR_TAG = "Bear";

    private double capturePercentage;

    private ControlNode node;
    private const double CAPTURE_RATE = 1/3f;
    private string occupyingTeam;
    private void Awake()
    {
        capturePercentage = 0f;
        node = new ControlNode();
        numTeamsOnNode = 0;
        occupyingTeam = "NoTeam";
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
        string tag = collider.gameObject.tag;
        if (isEntity(tag))
        {
            ControlNode.Team enteringTeam = convertTagToTeam(tag);
            node.Enter(enteringTeam);
        }
    }

    
    private void OnTriggerStay2D(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        if (isEntity(tag))
        {
            ControlNode.Team stayingTeam = convertTagToTeam(tag);
            node.Stay(stayingTeam);
        }
    } 

    //handles when a previous team leaves and when there is no one on tag
    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (isEntity(tag))
        {
            ControlNode.Team leavingTeam = convertTagToTeam(tag);
            node.Leave(leavingTeam);
        }

    }
    private void Update()
    {
        node.debugDisplay();
    }

    IEnumerator capture()
    { 
        while (true)
        {
            
            ControlNode.State nodeStatus = node.getState();
            switch (nodeStatus)
            {
                case ControlNode.State.Captured:
                    if(node.getOwner() == ControlNode.Team.Bears)
                    {
                        renderer.color = Color.black;
                    }
                    else if(node.getOwner() == ControlNode.Team.Wolves)
                    {
                        renderer.color = Color.blue;
                    }
                    break;
                case ControlNode.State.Capturing:
                    double increment = CAPTURE_RATE * Time.deltaTime;
                    node.Capture(increment);
                    renderer.color = Color.green;
                    break;
                case ControlNode.State.Contested:
                    renderer.color = Color.red;
                    break;
                case ControlNode.State.Empty:
                    renderer.color = Color.gray;
                    break;
                default:
                    break;
            }

            
            yield return null;
        }
        
    }
}
