using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] GameObject _startCircuit;
    [SerializeField] GameObject _endCircuit;
    [SerializeField] GameObject _negativeCircuit;

    private List<GameObject> _circuits;
    private BoardManager _boardManager;
    bool win = false;

    private void Awake()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        _circuits = new List<GameObject>();
    }

    private void Start()
    {
        _circuits.Add(_endCircuit);
        _circuits.Add(_negativeCircuit);
        _circuits.Add(_startCircuit);
    }

    void Update()
    {
        CheckForWin();

        /*string str = "";
        for (int i = 0; i < _circuits.Count; i++)
        {
            str += _circuits[i].gameObject.name + ", ";
        }Debug.Log(str);*/
    }
    
    private void CheckForWin()
    {
        bool connected = true;
        for (int i = 0; i < _circuits.Count && connected; i++)
        {
            connected = _circuits[i].GetComponent<Circuit>().GetIfConnected();
            //Debug.Log($"{_circuits[i].gameObject.name} {connected}");
        }
        win = connected;

        if (win)
        {
            Debug.Log("Game won!");
        }
    }

    public void AddToTotalCircuitsOnBoard(GameObject circuit)
    {
        _circuits.Add(circuit);
    }

    public void RemoveFromTotalCircuitsOnBoard(GameObject circuit)
    {
        _circuits.Remove(circuit);
    }


}
