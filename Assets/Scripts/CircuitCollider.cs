using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitCollider : MonoBehaviour
{
    private Circuit _parentCircuit;
    [SerializeField] bool _connected = false;

    private void Awake()
    {
        _parentCircuit = gameObject.transform.parent.transform.parent.GetComponent<Circuit>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _connected = true;
        CircuitCollider other = collision.gameObject.GetComponent<CircuitCollider>();
        _parentCircuit.AddColliderToConnectedList(other);

        if (other.GetCircuit().GetConnectedToStart())
        {
            _parentCircuit.SetConnectedToStart(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _connected = false;
        CircuitCollider other = collision.gameObject.GetComponent<CircuitCollider>();
        _parentCircuit.RemoveColliderFromConnectedList(other);

        if (other.GetCircuit().GetConnectedToStart())
        {
            _parentCircuit.SetConnectedToStart(false);
        }
    }

    public bool GetConnected()
    {
        return _connected;
    }

    public void SetConnected(bool c)
    {
        _connected = c;
    }

    public Circuit GetCircuit()
    {
        return _parentCircuit;
    }
}
