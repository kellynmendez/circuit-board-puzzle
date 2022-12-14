using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivateConnectionLine : MonoBehaviour
{
    private List<Circuit> _circuitPath;
    [SerializeField] Circuit _startCircuit;

    private WinCondition _winCondition;
    private UIManager _uiManager;
    private int _targetVoltage;
    private int _currentVoltage = 0;
    private bool _targetVoltageMet = false;

    private void Awake()
    {
        _circuitPath = new List<Circuit>();
        _winCondition = FindObjectOfType<WinCondition>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        _targetVoltage = _uiManager.GetTargetVoltage();
    }

    private void Update()
    {
        CheckForVoltage();
    }

    public void UpdateCircuitPath()
    {
        RecreatePath();
        HandleAllConnectionLines();
    }

    private void RecreatePath()
    {
        _circuitPath.Clear();
        _circuitPath.Add(_startCircuit);

        bool addedAfterStart = false;
        Circuit startCirc = _circuitPath[0];
        List<CircuitCollider> startConList = startCirc.GetConnectedList();
        Circuit startPrev = startCirc.GetPreviousCircuit();
        for (int i = 0; i < startConList.Count; i++)
        {
            Circuit check = startConList[i].GetCircuit();
            // If the circuit connected to this one is not its previous, then it is its next
            if (check != startPrev)
            {
                addedAfterStart = true;
                // Set this circuit to its next circuit
                startCirc.SetNextCircuit(check);
                // Set the next circuit's previous to this circuit
                check.SetPreviousCircuit(startCirc);
                // Add this circuit to the end of the list
                _circuitPath.Add(check);
            }
        }

        int k = 1;
        bool added = true;
        if (addedAfterStart)
        {
            while (added)
            {
                added = false;
                // Getting the circuit and its connected list
                Circuit circ = _circuitPath[k];
                List<CircuitCollider> connectedList = circ.GetConnectedList();
                // Getting the circuit before this one
                Circuit prev = circ.GetPreviousCircuit();
                // Check each circuit that is connected to this one; if the circuit
                //      connected to this one is NOT the one before it, then that
                //      circuit is the one after this circuit, and should be added
                //      to the path
                for (int i = 0; i < connectedList.Count && !added; i++)
                {

                    Circuit check = connectedList[i].GetCircuit();
                    // If the circuit connected to this one is not its previous,
                    //      then it is its next
                    if (check && check != prev)
                    {
                        added = true;
                        // Set this circuit to its next circuit
                        circ.SetNextCircuit(check);
                        // Set the next circuit's previous to this circuit
                        check.SetPreviousCircuit(circ);
                        // Add this circuit to the end of the list
                        _circuitPath.Add(check);
                    }
                }
                k++;
            }
        }

        _winCondition.CheckForWin();
    }

    private void HandleAllConnectionLines()
    {
        // Activating circuits in circuit path
        for (int i = 0; i < _circuitPath.Count; i++)
        {
            _circuitPath[i].ActivateConnectedLine();
        }
        // Deactivating circuits not in circuit path
        List<Circuit> allCircs = _winCondition.GetAllCircuitsOnBoard();
        for (int i = 0; i < allCircs.Count; i++)
        {
            if (!_circuitPath.Contains(allCircs[i]))
            {
                allCircs[i].DeactivateConnectedLine();
            }
        }
    }

    public void RemoveCircuitFromPath(Circuit c)
    {
        // When you remove a circuit from the path, all others after it are disconnected
        if (_circuitPath.Contains(c))
        {
            int idx = _circuitPath.IndexOf(c);
            // Removing this circuit and deactivating all after it
            for (int i = idx; i < _circuitPath.Count; i++)
            {
                Circuit rem = _circuitPath[i];
                ResetConnections(rem);
                rem.DeactivateConnectedLine();  
            }
        }
    }

    private void ResetConnections(Circuit c)
    {
        // Resetting pointers
        if (c.GetNextCircuit())
        {
            c.GetNextCircuit().SetPreviousCircuit(null);
        }
        if (c.GetPreviousCircuit())
        {
            c.GetPreviousCircuit().SetNextCircuit(null);
        }
    }

    public bool CheckForVoltage()
    {
        int voltage = 0;
        for (int i = 0; i < _circuitPath.Count; i++)
        {
            voltage += _circuitPath[i].GetVoltage();
        }

        if (_currentVoltage != voltage)
        {
            // Updating voltage
            _currentVoltage = voltage;
            if (_currentVoltage != _targetVoltage)
            {
                _targetVoltageMet = false;
            }
            else
            {
                _targetVoltageMet = true;
            }
            _uiManager.UpdateCurrentVoltage(voltage);
        }

        return _targetVoltageMet;
    }

    public bool GetIfTargetVoltageMet()
    {
        return _targetVoltageMet;
    }
}
