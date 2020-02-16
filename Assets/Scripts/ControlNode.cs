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
    private List<Team> occupants;


    public ControlNode()
    {
        percentageOfCapture = 0;
        owner = Team.NoTeam;
        capturingTeam = Team.NoTeam;
        status = State.Empty;
        occupants = new List<Team>();
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
            //adds team to occupants
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
        //special case to empty a control node when no one captures it
        percentageOfCapture = 0;
        owner = Team.NoTeam;
        status = State.Empty;
        capturingTeam = Team.NoTeam;
        occupants.Clear();
    }

    public bool isEmptyState()
    {
        return (percentageOfCapture == 0 && owner == Team.NoTeam && status == State.Empty && capturingTeam == Team.NoTeam && occupants.Count == 0);
    }

    public bool hasNotCapturedNode(Team team)
    {
        return (getState() != State.Capturing && getOwner() != team);
    }
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
    public void onStay(Team team)
    {
        if (occupants.Count > 1)
        {
            setState(State.Contested, team);
        }
        else if (occupants.Count == 1 && hasNotCapturedNode(team))
        {
            setState(State.Capturing, occupants.ToArray()[0]);
        }
        
    }

    public void onLeave(Team team)
    {
        occupants.Remove(team);
        if (occupants.Count == 1)
        {
            setState(State.Capturing, occupants.ToArray()[0]);
        }
        else if (occupants.Count == 0 && getState() != State.Captured)
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
