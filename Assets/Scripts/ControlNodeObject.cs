using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeObject : MonoBehaviour
{
    // - Constant Tags for testing
    // - SpriteRender for debugging purposes
    private const string WOLF_TAG = "Wolf";
    private const string BEAR_TAG = "Bear";
    private const double CAPTURE_RATE = 1 / 3f;
    private ControlNode node;
    private SpriteRenderer renderer;

    [SerializeField]
    int regionRadius = 10;

    // *** ControlNode Class Definition ***
    public class ControlNode
    {
        private const double PERCENT_OF_CAPTURED = 1f;

        // Team enum can be modified for flexibility (excluding NoTeam)
        public enum Team { Wolves, Bears, Chickens, NoTeam }
        public enum State { Captured, Capturing, Empty, Contested }

        private double percentageOfCapture;
        private Team owner;
        private Team capturingTeam;
        private State status;
        private List<Team> occupants;

        public ControlNode()
        {
            percentageOfCapture = 0;
            owner = Team.NoTeam;
            capturingTeam = Team.NoTeam;
            status = State.Empty;
            occupants = new List<Team>();
        }

        // Setters and getters
        public Team getOwner()
        {
            return owner;
        }
        public State getState()
        {
            return status;
        }
        // - Set state method:
        //
        // Captured state:
        //      - owner of the team is determined
        //      - no new capturingTeam
        // Capturing state:
        //      - a capturing team is defined
        //      - percent to capture has been reset to 0 for capture
        // Contested state:
        //      - no team can capture the region
        private void setState(State state, Team team)
        {
            switch (state)
            {
                case State.Captured:
                    owner = team;
                    capturingTeam = Team.NoTeam;
                    percentageOfCapture = 0;
                    break;

                case State.Capturing:
                    capturingTeam = team;
                    break;

                case State.Contested:
                    capturingTeam = Team.NoTeam;
                    break;
                default:
                    break;
            }
            status = state;
        }
        public void setStateEmpty()
        {
            percentageOfCapture = 0;
            owner = Team.NoTeam;
            status = State.Empty;
            capturingTeam = Team.NoTeam;
            occupants.Clear();
        }
        //-----------------Team detection methods-----------------
        //
        // - Corresponds to enter, stay, and exit watcher methods of trigger2d.
        // - Numbers correspond to if statements
        //
        // onEnter
        // #1. Checks if the oncoming team is in occupants list, adds to it if the team is not present
        // #2. Once occupants reaches past 1, a contest is detected
        public void onEnter(Team team)
        {
            if (!occupants.Contains(team))
            {
                occupants.Add(team);
            }
            if (occupants.Count > 1)
            {
                setState(State.Contested, team);
            }
        }

        // onStay
        // #1. Checks if the collider only has 1 team present, sets the state to capturing once detected
        public void onStay(Team team)
        {
            if (occupants.Count == 1 && hasNotCapturedNode(team))
            {
                setState(State.Capturing, team);
            }
        }

        // onLeave
        // - Removes the leaving team off the list
        // #1. If there is one team left and the original owner has left the region, the remaining team can capture the region
        // #2. If the team leaves before capturing the point, and there is no original owner, set the control point state empty
        // #3. Otherwise, set the region to being captured by previous owner
        public void onLeave(Team team)
        {
            occupants.Remove(team);
            if (occupants.Count == 1 && owner == team)
            {
                setState(State.Capturing, occupants.ToArray()[0]);
            }
            else if (occupants.Count == 0 && getOwner() == Team.NoTeam)
            {
                setStateEmpty();
            }
            else
            {
                setState(State.Captured, owner);
            }

        }
        // - Additional helper methods

        // - For use in IEnumerator coroutine, a capture method that can be interrupted 
        public void Capture(double increment)
        {
            percentageOfCapture += increment;
            if (percentageOfCapture >= PERCENT_OF_CAPTURED)
            {
                setState(State.Captured, capturingTeam);
            }
        }

        // - To return whether the passed in node has captured the region yet
        public bool hasNotCapturedNode(Team team)
        {
            return (getState() != State.Capturing && getOwner() != team);
        }

        // - Debugging purposes
        public void debugDisplay()
        {
            Debug.Log("Current owner: " + owner);
            Debug.Log("Current capturing team: " + capturingTeam);
            Debug.Log("Current status: " + status);
            Debug.Log("Percentage of capture: " + percentageOfCapture);
        }

    }

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        node = new ControlNode();
        StartCoroutine("statusWatcher");
        
    }
    bool isTeam(string tag)
    {
        return (tag == WOLF_TAG|| tag == BEAR_TAG);
    }

    // *** CURRENT SYSTEM RELIES ON GAMEOBJECT TAGS TO FIND DIFFERENT TEAMS ***
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
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (isTeam(tag))
        {
            ControlNode.Team enteringTeam = convertTagToTeam(tag);
            node.onEnter(enteringTeam);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        if (isTeam(tag))
        {
            ControlNode.Team stayingTeam = convertTagToTeam(tag);
            node.onStay(stayingTeam);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (isTeam(tag))
        {
            ControlNode.Team exitingTeam = convertTagToTeam(tag);
            node.onLeave(exitingTeam);
        }
    }

    private void Update()
    {
        node.debugDisplay();
    }


    // A forever while loop coroutine that uses switch statements to check status of the ControlNode
    IEnumerator statusWatcher()
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
