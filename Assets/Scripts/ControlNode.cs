using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNode
{
    public enum Team {Wolves, Bears, Chickens, NoTeam}
    public enum State {Captured, Capturing, Empty, Contested}

    private Team owner;
    private Team capturingTeam;
    private State status;
    private List<Team> challengers;


    public ControlNode()
    {
        owner = Team.NoTeam;
        capturingTeam = Team.NoTeam;
        status = State.Empty;
        challengers = new List<Team>();
    }

    // Setters and getters for team
    public Team getTeam()
    {
        return owner;
    }

    public void setTeam(Team team)
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
   /*private void setState(State state, Team team)
    {
        switch (state)
        {
            case State.Captured:
                owner = team;
                capturingTeam = Team.NoTeam;
                break;
            case State.Capturing:
                capturingTeam = team;
                break;
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
        status = State.Empty;
        capturingTeam = Team.NoTeam;
        challengers.Clear();
    }
    public void Enter(Team challenger)
    {
        // if node is uncontested, let the entering team take the node, regardless of
        if (getState() != State.Contested)
        {
            setState(State.Capturing, challenger);
        }

        // if a challenging team comes in, let the owner and the challenger contest
        else if (challenger != owner && !challengers.Contains(challenger))
        {
            setState(State.Contested, challenger);
        }
    }

    public void Capture(Team stayingTeam)
    {

    }

    public void Leave(Team leavingTeam)
    {
        //assumes if one of the challengers are gone
        if (challengers.Contains(leavingTeam))
        {
            challengers.Remove(leavingTeam);

            //if no challengers, owner keeps node
            if(challengers.Count == 0)
            {
                setState(State.Captured, owner);
            }
        }
        
        //if owner has left, let the challengers take over
        else if(leavingTeam == owner)
        {
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

        //if a team fails to capture with 100%, reset to empty
        else
        {
            setStateEmpty();
        }
    }
    */

}
