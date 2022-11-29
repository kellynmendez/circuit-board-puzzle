using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitCollider : MonoBehaviour
{
    private Circuit _parentCircuit;
    [SerializeField] bool connected = false;

    private void Awake()
    {
        _parentCircuit = gameObject.transform.parent.transform.parent.GetComponent<Circuit>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        connected = true;
        _parentCircuit.AddColliderToConnectedList(collision.gameObject.GetComponent<CircuitCollider>());
        Debug.Log("connected with another circuit");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        connected = false;
        _parentCircuit.RemoveColliderFromConnectedList(collision.gameObject.GetComponent<CircuitCollider>());
        Debug.Log("circuit is no longer connected");
    }

    public bool GetConnected()
    {
        return connected;
    }

    public void SetConnected(bool c)
    {
        connected = c;
    }
}
