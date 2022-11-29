using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    // Array of this circuit's colliders
    [SerializeField] CircuitCollider[] _colliders;
    // Key to get circuit from circuit dictionary
    [SerializeField] string _circuitDictKey;
    // If locked, cannot select on board
    [SerializeField] bool _locked;
    // If spot is a voltage spot
    [SerializeField] bool _voltageSpot;

    // List of colliders on other circuits that are connected to this one
    private List<CircuitCollider> _collidersConnected;

    private bool connected = false;

    private void Awake()
    {
        _collidersConnected = new List<CircuitCollider>();
    }

    private void Update()
    {
        if (_colliders.Length > 0)
        {
            CheckIfConnected();
        }

        /*string str = "";
        for (int i = 0; i < _collidersConnected.Count; i++)
        {
            str += _collidersConnected[i].gameObject.name + ", ";
        }Debug.Log(str);*/
    }

    private void CheckIfConnected()
    {
        bool checkConnected = true;
        for (int i = 0; i < _colliders.Length && checkConnected; i++)
        {
            CircuitCollider collider = _colliders[i];
            checkConnected = collider.GetConnected();
            Debug.Log($"{checkConnected}");
        }
        connected = checkConnected;
    }

    public string GetCircuitDictionaryKey()
    {
        return _circuitDictKey;
    }

    public bool GetLocked()
    {
        return _locked;
    }

    public bool GetIfVoltageSpot()
    {
        return _voltageSpot;
    }

    public bool GetIfConnected()
    {
        return connected;
    }

    public List<CircuitCollider> GetConnectedList()
    {
        return _collidersConnected;
    }

    public void AddColliderToConnectedList(CircuitCollider circuit)
    {
        _collidersConnected.Add(circuit);
    }

    public void RemoveColliderFromConnectedList(CircuitCollider circuit)
    {
        _collidersConnected.Remove(circuit);
    }
}
