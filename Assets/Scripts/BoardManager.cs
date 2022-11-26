using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [Header("Circuit Board")]
    [SerializeField] GameObject[] _row1;
    [SerializeField] GameObject[] _row2;
    [SerializeField] GameObject[] _row3;
    [SerializeField] GameObject[] _row4;
    [SerializeField] GameObject[] _row5;

    [Header("Sprites")]
    [SerializeField] Sprite _borderSprite;
    [SerializeField] Sprite _selectedSprite;

    private GameObject[,] _circuitBoard;
    private GameObject _selected;
    private int _currRow = 0;
    private int _currCol = 0;

    private InventoryManager _inventoryManager;
    private GameManager _gameManager;

    private int _numRows = 5;
    private int _numCols = 5;

    private void Awake()
    {
        // Initializing game object 2d array
        _circuitBoard = new GameObject[_numRows, _numCols];

        for (int i = 0; i < _numRows; i++)
        {
            for (int j = 0; j < _numCols; j++)
            {
                if (i == 0)
                    _circuitBoard[i, j] = _row1[j];
                else if (i == 1)
                    _circuitBoard[i, j] = _row2[j];
                else if (i == 2)
                    _circuitBoard[i, j] = _row3[j];
                else if (i == 3)
                    _circuitBoard[i, j] = _row4[j];
                else
                    _circuitBoard[i, j] = _row5[j];
            }
        }

        _gameManager = FindObjectOfType<GameManager>();
        _inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void Start()
    {
        // Setting first selection
        _selected = _circuitBoard[_currRow, _currCol];
        GameObject currGOBorder = _selected.transform.GetChild(0).gameObject;
        currGOBorder.GetComponent<Image>().sprite = _selectedSprite;
    }

    void Update()
    {
        if (_gameManager.GetCurrentGameState() == GameManager.GameState.CircuitBoard)
        {
            HandleBoardMovement();
        }
    }

    private void SelectSpot()
    {
        // Removing selection border from previous space
        GameObject prevGOBorder = _selected.transform.GetChild(0).gameObject;
        prevGOBorder.GetComponent<Image>().sprite = _borderSprite;
        // Selecting the new space
        _selected = _circuitBoard[_currRow, _currCol];
        GameObject currGOBorder = _selected.transform.GetChild(0).gameObject;
        currGOBorder.GetComponent<Image>().sprite = _selectedSprite;

        // If spot has changed from voltage place and normal or vice versa, update inventory state/view
        bool isVoltSpot = _selected.GetComponent<Circuit>().GetIfVoltageSpot();

        if (isVoltSpot && _inventoryManager.GetCurrentInventoryState() 
            == InventoryManager.InventoryState.NormalInventory)
        {
            _inventoryManager.UpdateGameState(InventoryManager.InventoryState.VoltInventory);
        }
        else if (!isVoltSpot && _inventoryManager.GetCurrentInventoryState()
            == InventoryManager.InventoryState.VoltInventory)
        {
            _inventoryManager.UpdateGameState(InventoryManager.InventoryState.NormalInventory);
        }
    }

    private void HandleBoardMovement()
    {
        // Can only select a spot if it is within bounds of the board and is not locked
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currCol + 1 < _numCols)
            {
                bool locked = _circuitBoard[_currRow, _currCol + 1].GetComponent<Circuit>().GetLocked();
                if (!locked)
                {
                    _currCol++;
                    SelectSpot();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currCol - 1 >= 0)
            {
                bool locked = _circuitBoard[_currRow, _currCol - 1].GetComponent<Circuit>().GetLocked();
                if (!locked)
                {
                    _currCol--;
                    SelectSpot();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_currRow + 1 < _numRows)
            {
                bool locked = _circuitBoard[_currRow + 1, _currCol].GetComponent<Circuit>().GetLocked();
                if (!locked)
                {
                    _currRow++;
                    SelectSpot();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_currRow - 1 >= 0)
            {
                bool locked = _circuitBoard[_currRow - 1, _currCol].GetComponent<Circuit>().GetLocked();
                if (!locked)
                {
                    _currRow--;
                    SelectSpot();
                }
            }
        }
    }

    public GameObject GetCurrentSelection()
    {
        return _selected;
    }
}
