using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNode
{
    private const double PERCENT_OF_CAPTURED = 1f;
    public enum Team {Wolves, Bears, Chickens, NoTeam}
    public enum State {Captured, Capturing, Empty, Contested}

    private double percentageOfCapture;
    private Team owner;
    private Team capturingTeam;
    private State status;
    private List<Team> challengers;


    public ControlNode()
    {
        percentageOfCapture = 0;
        owner = Team.NoTeam;
        capturingTeam = Team.NoTeam;
        status = State.Empty;
        challengers = new List<Team>();
    }

    // Setters and getters for team
    public Team getOwner()
    {
        return owner;
    }

    public void setOwner(Team team)
    {
        owner = team;
    }

    // Setters and getters for state
    public State getState()
    {
        return status;
    }

    public void setState(State state)
    {
        status = state;
    }

   private void setState(State state, Team team)
    {
        switch (state)
        {
            //sets status to captured with no one else trying to capture
            case State.Captured:
                owner = team;
                capturingTeam = Team.NoTeam ;
                break;
            //sets status to capturing with capturing team being the passed in team
            case State.Capturing:
                capturingTeam = team;
                percentageOfCapture = 0;
                break;
            //adds team to challengers
            case State.Contested:
                capturingTeam = Team.NoTeam;
                challengers.Add(team);
                break;
            default:
                break;
        }
        status = state;
    }
    public void setStateEmpty()
    {
        //special case to empty a control node when no one captures it
        percentageOfCapture = 0;
        owner = Team.NoTeam;
        status = State.Empty;
        capturingTeam = Team.NoTeam;
        challengers.Clear();
    }

    public bool isEmptyState()
    {
        return (percentageOfCapture == 0 && owner == Team.NoTeam && status == State.Empty && capturingTeam == Team.NoTeam && challengers.Count == 0);
    }

    public bool isOwnerOccupying()
    {
        return owner == capturingTeam;
    }
    public bool isUncontested()
    {
        return !isOwnerOccupying() && isEmptyState();
    }
    public void Enter(Team challenger)
    {
        if(challenger != owner)
        {
            //#1. if node is uncontested, let the entering team take the node, regardless of the owner
            if (status != State.Contested)
            {
                setState(State.Capturing, challenger);
            }
            //#2. if a challenging team comes in, let the owner and the challenger contest
            else if (!challengers.Contains(challenger))
            {
                setState(State.Contested, challenger);
            }
        }
        
    }

    public void Stay(Team stayingTeam)
    {
        if(getState() != State.Contested)
        {
            setState(State.Capturing, stayingTeam);
        }


    }

    public void Leave(Team leavingTeam)
    {
        //#1. assumes if one of the challengers are gone
        if (challengers.Contains(leavingTeam))
        {
            challengers.Remove(leavingTeam);

            //if no challengers, owner keeps node
            if(challengers.Count == 0)
            {
                setState(State.Captured, owner);
            }
        }
        
        //#2. if owner has left, let the challengers take over
        else if(leavingTeam == owner)
        {
            //empty the node

            //if single challenger, let challenger capture
            if (challengers.Count == 1)
            {
                setState(State.Capturing, challengers.ToArray()[0]);
            }
            //if multiple challengers, set state to contested
            else
            {
                foreach (Team team in challengers)
                {
                    setState(State.Contested, team);
                }
            }
        }
        //#3. if a team fails to capture with 100%, reset to empty
        else
        {
            setStateEmpty();
        }
    }

    public void Capture(double increment)
    {

        percentageOfCapture += increment;
        if (percentageOfCapture >= PERCENT_OF_CAPTURED)
        {
            setState(State.Captured, capturingTeam);
        }
    }
    public void debugDisplay()
    {
        Debug.Log("Current owner: " + owner);
        Debug.Log("Current capturing team: " + capturingTeam);
        Debug.Log("Current status: " + status);
        Debug.Log("Percentage of capture: " + percentageOfCapture);
    }

}
