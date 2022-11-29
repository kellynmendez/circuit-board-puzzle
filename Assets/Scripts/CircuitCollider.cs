using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitCollider : MonoBehaviour
{
    private bool connected = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        connected = true;
        //Debug.Log("connected with another circuit");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        connected = false;
        //Debug.Log("circuit is no longer connected");
    }

    public bool GetConnected()
    {
        return connected;
    }
}
