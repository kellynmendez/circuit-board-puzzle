using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitCollider : MonoBehaviour
{
    private Circuit _parentCircuit;
    private bool connected = false;

    private void Awake()
    {
        _parentCircuit = gameObject.transform.parent.transform.parent.GetComponent<Circuit>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        connected = true;
        _parentCircuit.AddColliderToConnectedList(this);
        //Debug.Log("connected with another circuit");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        connected = false;
        _parentCircuit.RemoveColliderFromConnectedList(this);
        //Debug.Log("circuit is no longer connected");
    }

    public bool GetConnected()
    {
        return connected;
    }
}
