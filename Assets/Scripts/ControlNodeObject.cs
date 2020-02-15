using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeObject : MonoBehaviour
{

    private float capturePercentage;
    private ControlNode node;

    private void Awake()
    {
        node = new ControlNode();
        capturePercentage = 0f;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wolf")
        {
            Debug.Log("Wolf has been detected");
        }
    }

    void capture()
    {

        node.setTeam(ControlNode.Team.Wolves);
    }



}
