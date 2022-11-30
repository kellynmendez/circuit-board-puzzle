using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] GameObject _startCircuit;
    [SerializeField] GameObject _endCircuit;
    [SerializeField] GameObject _negativeCircuit;

    private List<Circuit> _circuits;
    private BoardManager _boardManager;
    bool win = false;

    private void Awake()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        _circuits = new List<Circuit>();
    }

    private void Start()
    {
        _circuits.Add(_endCircuit.GetComponent<Circuit>());
        _circuits.Add(_negativeCircuit.GetComponent<Circuit>());
        _circuits.Add(_startCircuit.GetComponent<Circuit>());
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
            connected = _circuits[i].GetIfConnected();
            //Debug.Log($"{_circuits[i].gameObject.name} {connected}");
        }
        win = connected;

        if (win)
        {
            Debug.Log("Game won!");
        }
    }

    public void AddToTotalCircuitsOnBoard(Circuit circuit)
    {
        _circuits.Add(circuit);
    }

    public void RemoveFromTotalCircuitsOnBoard(Circuit circuit)
    {
        _circuits.Remove(circuit);
    }

    public List<Circuit> GetAllCircuitsOnBoard()
    {
        return _circuits;
    }

}
