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
    // If circuit is a voltage circuit
    [SerializeField] bool _voltageSpot;
    // Voltage
    [SerializeField] int _voltage;
    // If circuit is empty
    [SerializeField] bool _empty;
    // If circuit is start circuit
    [SerializeField] bool _start;
    // Line game object
    [SerializeField] GameObject _connectionLine;
    // If the circuit is connected to start (this is when connection line should be activated)
    //      Note that the start circuit will always have this set to true
    [SerializeField] bool _connectedToStart = false;

    private Circuit _prevCircuit;
    private Circuit _nextCircuit;
    // List of colliders on other circuits that are connected to this one
    private List<CircuitCollider> _collidersConnected;
    // Whether circuit is fully connected (all ends connected to another)
    private bool _connected = false;

    private void Awake()
    {
        _collidersConnected = new List<CircuitCollider>();
    }

    private void Start()
    {
        if (_start)
        {
            ActivateConnectedLine();
            _prevCircuit = null;
        }
    }

    private void Update()
    {
        if (_colliders.Length > 0)
        {
            // Checking if circuit is connected for win condition
            //    A circuit is connected if both ends of the circuit are connected to other circuit ends
            CheckIfConnected();
        }

    }

    private void CheckIfConnected()
    {
        bool checkConnected = true;
        for (int i = 0; i < _colliders.Length && checkConnected; i++)
        {
            CircuitCollider collider = _colliders[i];
            checkConnected = collider.GetConnected();
        }
        _connected = checkConnected;
    }

    public void ActivateConnectedLine()
    { 
        if (!_empty && !_connectionLine.activeSelf)
        {
            // activate line
            _connectionLine.SetActive(true);
        }
    }

    public void DeactivateConnectedLine()
    {
        if (!_empty && _connectionLine.activeSelf)
        {
            // deactivate line
            _connectionLine.SetActive(false);
        }
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
        return _connected;
    }

    public bool GetConnectedToStart()
    {
        return _connectedToStart;
    }

    public void SetConnectedToStart(bool c)
    {
        _connectedToStart = c;
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

    public void ClearConnectedList()
    {
        _collidersConnected.Clear();
    }

    public Circuit GetNextCircuit()
    {
        return _nextCircuit;
    }

    public void SetNextCircuit(Circuit c)
    {
        _nextCircuit = c;
    }

    public Circuit GetPreviousCircuit()
    {
        return _prevCircuit;
    }

    public void SetPreviousCircuit(Circuit c)
    {
        _prevCircuit = c;
    }

    public int GetVoltage()
    {
        return _voltage;
    }
}
