using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private InventoryState CurrentInventoryState;

    [SerializeField] GameObject _normalInventoryView;
    [SerializeField] GameObject _voltageInventoryView;
    [SerializeField] GameObject[] _normalInventory;
    [SerializeField] GameObject[] _voltInventory;

    private GameManager _gameManager;
    private BoardManager _boardManager;
    private GameObject _selected;
    private int _currRow;

    /* constants */
    private int _numNormalRows = 2;
    private int _numVoltRows = 2;

    public enum InventoryState
    {
        NormalInventory,
        VoltInventory
    }

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _boardManager = FindObjectOfType<BoardManager>();
    }

    private void Start()
    {
        // Setting starting inventory state
        UpdateGameState(InventoryState.NormalInventory);
        _currRow = 0;
        _selected = _normalInventory[_currRow];
    }

    private void Update()
    {
        if (_gameManager.GetCurrentGameState() == GameManager.GameState.Inventory)
        {
            HandleUserInput();
        }
    }

    public void UpdateGameState(InventoryState inventoryState)
    {
        CurrentInventoryState = inventoryState;

        switch (CurrentInventoryState)
        {
            case InventoryState.NormalInventory:
                SwitchToNormalInventory();
                break;
            case InventoryState.VoltInventory:
                SwitchToVoltInventory();
                break;
            default:
                Debug.Log("Error: State doesn't exist");
                break;
        }
    }

    private void SwitchToNormalInventory()
    {
        _voltageInventoryView.SetActive(false);
        _normalInventoryView.SetActive(true);
    }

    private void SwitchToVoltInventory()
    {
        _normalInventoryView.SetActive(false);
        _voltageInventoryView.SetActive(true);
    }

    public void SwitchFromBoardToInventory()
    {
        // Set row to the first object in inventory
        _currRow = 0;
        // Update current selected object to be the first object in inventory
        if (CurrentInventoryState == InventoryState.NormalInventory)
        {
            _selected = _normalInventory[0];
        }
        else
        {
            _selected = _voltInventory[0];
        }
        // Activate selection
        _selected.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void SwitchFromInventoryToBoard()
    {
        // Unselect the current selection
        _selected.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void HandleUserInput()
    {
        // Add circuit to board
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddCircuit();
        }
        else
        {
            // Selecting from normal inventory circuits available
            if (CurrentInventoryState == InventoryState.NormalInventory)
            {
                // Can only select a spot if it is within bounds of inventory
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (_currRow + 1 < _numNormalRows)
                    {
                        _currRow++;
                        SelectSpot();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (_currRow - 1 >= 0)
                    {
                        _currRow--;
                        SelectSpot();
                    }
                }
            }
            // Selecting from voltage inventory circuits available
            else if (CurrentInventoryState == InventoryState.VoltInventory)
            {
                // Can only select a spot if it is within bounds of inventory
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (_currRow + 1 < _numVoltRows)
                    {
                        _currRow++;
                        SelectSpot();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (_currRow - 1 >= 0)
                    {
                        _currRow--;
                        SelectSpot();
                    }
                }
            }
        }
    }

    private void AddCircuit()
    {
        // Adding the circuit
        _boardManager.AddCircuitToBoard(_selected);
        // Going back to the board
        _gameManager.UpdateGameState(GameManager.GameState.CircuitBoard);
    }

    private void SelectSpot()
    {
        // Removing selection border from previous space
        _selected.transform.GetChild(1).gameObject.SetActive(false);
        // Selecting the new space
        if (CurrentInventoryState == InventoryState.NormalInventory)
        {
            _selected = _normalInventory[_currRow];
            // Activating selection border
            _selected.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (CurrentInventoryState == InventoryState.VoltInventory)
        {
            _selected = _voltInventory[_currRow];
            // Activating selection border
            _selected.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public InventoryState GetCurrentInventoryState()
    {
        return CurrentInventoryState;
    }
}
