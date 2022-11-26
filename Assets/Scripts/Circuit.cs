using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    // If locked, cannot select on board
    [SerializeField] bool _locked;
    // If spot is a voltage spot
    [SerializeField] bool _voltageSpot;

    public bool GetLocked()
    {
        return _locked;
    }

    public bool GetIfVoltageSpot()
    {
        return _voltageSpot;
    }
}
