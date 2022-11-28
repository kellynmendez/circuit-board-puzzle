using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlace : MonoBehaviour
{
    private PlaceState CurrentPlaceState;

    public enum PlaceState
    {
        Empty,
        Filled
    }

    private void Start()
    {
        // Setting starting game state
        CurrentPlaceState = PlaceState.Empty;
    }

    public void UpdatePlaceState(PlaceState placeState)
    {
        CurrentPlaceState = placeState;
    }

    public PlaceState GetCurrentPlaceState()
    {
        return CurrentPlaceState;
    }
}
