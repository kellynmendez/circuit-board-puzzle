using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    // Array of colliders
    [SerializeField] CircuitCollider[] _colliders;
    // Key to get circuit from circuit dictionary
    [SerializeField] string _circuitDictKey;
    // If locked, cannot select on board
    [SerializeField] bool _locked;
    // If spot is a voltage spot
    [SerializeField] bool _voltageSpot;

    private bool connected = false;

    private void Update()
    {
        if (_colliders.Length > 0)
        {
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
        connected = checkConnected;

        if (checkConnected)
        {
            Debug.Log($"{gameObject.name} is connected");
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
        return connected;
    }
}
