using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNode
{
    public enum Team {Wolves, Bears, Chickens, NoTeam}
    public enum State {Captured, Capturing, Empty, Contested}

    private Team owner;
    private State status;


    public ControlNode()
    {
        owner = Team.NoTeam;
        status = State.Empty;
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
}
