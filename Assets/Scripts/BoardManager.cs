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

    [Header("Circuit Prefabs")]
    [SerializeField] GameObject _emptyCirc;
    [SerializeField] GameObject _emptyVoltCirc;
    [SerializeField] GameObject _cornerCirc;
    [SerializeField] GameObject _straightCirc;
    [SerializeField] GameObject _posOneCirc;
    [SerializeField] GameObject _posTwoCirc;

    protected Dictionary<string, GameObject> _circuitMap;
    protected GameObject[,] _circuitBoard;
    private GameObject _selected;
    private int _currRow = 0;
    private int _currCol = 0;

    private InventoryManager _inventoryManager;
    private GameManager _gameManager;
    private WinCondition _winCondition;

    /* constants */
    private int _numRows = 5;
    private int _numCols = 5;
    private bool rotating = false;
    private string _straightCircKey = "straight";
    private string _cornerCircKey = "corner";
    private string _posOneCircKey = "posOne";
    private string _posTwoCircKey = "posTwo";

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

        // Initializing circuit dictionary
        _circuitMap = new Dictionary<string, GameObject>();
        _circuitMap.Add(_straightCircKey, _straightCirc);
        _circuitMap.Add(_cornerCircKey, _cornerCirc);
        _circuitMap.Add(_posOneCircKey, _posOneCirc);
        _circuitMap.Add(_posTwoCircKey, _posTwoCirc);

        _gameManager = FindObjectOfType<GameManager>();
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _winCondition = FindObjectOfType<WinCondition>();
    }

    private void Start()
    {
        // Setting first selection
        _selected = _circuitBoard[_currRow, _currCol];
        GameObject currGOBorder = _selected.transform.GetChild(1).gameObject;
        currGOBorder.GetComponent<Image>().sprite = _selectedSprite;
    }

    void Update()
    {
        if (_gameManager.GetCurrentGameState() == GameManager.GameState.CircuitBoard)
        {
            HandleUserInput();
        }
    }

    private void SelectPlace()
    {
        // Getting the border object
        GameObject currGOBorder = _selected.transform.GetChild(1).gameObject;
        // Replacing the border with the selection sprite
        currGOBorder.GetComponent<Image>().sprite = _selectedSprite;
    }

    private void ChangeSelection()
    {
        // Removing selection border from previous space
        GameObject prevGOBorder = _selected.transform.GetChild(1).gameObject;
        prevGOBorder.GetComponent<Image>().sprite = _borderSprite;
        // Selecting the new space
        _selected = _circuitBoard[_currRow, _currCol];
        SelectPlace();

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

    private void HandleUserInput()
    {
        // If place is filled, check for input to remove or rotate selected circuit
        BoardPlace place = _selected.transform.parent.gameObject.GetComponent<BoardPlace>();
        if (place.GetCurrentPlaceState() == BoardPlace.PlaceState.Filled)
        {
            // Removing circuit from board
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                RemoveCircuitFromBoard();
            }
            // Rotating counterclockwise
            else if (Input.GetKeyDown(KeyCode.Q) && !rotating)
            {
                GameObject circuit = _selected.transform.GetChild(0).gameObject;
                RotateCircuit(circuit, false);
            }
            // Rotating clockwise
            else if (Input.GetKeyDown(KeyCode.E) && !rotating)
            {
                GameObject circuit = _selected.transform.GetChild(0).gameObject;
                RotateCircuit(circuit, true);
            }
        }

        // Check if moving around board
        if (Input.GetKeyDown(KeyCode.RightArrow)) // Moving right
        {
            // Can only select a spot if it is within bounds of the board and is not locked
            if (_currCol + 1 < _numCols)
            {
                bool locked = _circuitBoard[_currRow, _currCol + 1].GetComponent<Circuit>().GetLocked();
                if (!locked)
                {
                    _currCol++;
                    ChangeSelection();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) // Moving left
        {
            // Can only select a spot if it is within bounds of the board and is not locked
            if (_currCol - 1 >= 0)
            {
                bool locked = _circuitBoard[_currRow, _currCol - 1].GetComponent<Circuit>().GetLocked();
                if (!locked)
                {
                    _currCol--;
                    ChangeSelection();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) // Moving down
        {
            // Can only select a spot if it is within bounds of the board and is not locked
            if (_currRow + 1 < _numRows)
            {
                bool locked = _circuitBoard[_currRow + 1, _currCol].GetComponent<Circuit>().GetLocked();
                if (!locked)
                {
                    _currRow++;
                    ChangeSelection();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) // Moving up
        {
            // Can only select a spot if it is within bounds of the board and is not locked
            if (_currRow - 1 >= 0)
            {
                bool locked = _circuitBoard[_currRow - 1, _currCol].GetComponent<Circuit>().GetLocked();
                if (!locked)
                {
                    _currRow--;
                    ChangeSelection();
                }
            }
        }
    }

    public void AddCircuitToBoard(GameObject circuit)
    {
        // Instantiating the circuit prefab
        GameObject clone;
        string key = circuit.GetComponent<Circuit>().GetCircuitDictionaryKey();
        if (key == _straightCircKey)
        {
            clone = Instantiate(_straightCirc);
        }
        else if (key == _cornerCircKey)
        {
            clone = Instantiate(_cornerCirc);
        }
        else if (key == _posOneCircKey)
        {
            clone = Instantiate(_posOneCirc);
        }
        else // posTwoCircKey
        {
            clone = Instantiate(_posTwoCirc);
        }
        // Replacing the circuit
        ReplaceSelectedWithGiven(clone);
        // Setting board place state to filled
        GameObject parent = _selected.transform.parent.gameObject;
        parent.GetComponent<BoardPlace>().UpdatePlaceState(BoardPlace.PlaceState.Filled);

        // Adding circuit to existing circuits list
        _winCondition.AddToTotalCircuitsOnBoard(_selected);
    }

    private void RemoveCircuitFromBoard()
    {
        // Removing circuit from existing circuits list
        _winCondition.RemoveFromTotalCircuitsOnBoard(_selected);

        // Instantiating empty circuit prefab
        GameObject clone;
        if (_selected.GetComponent<Circuit>().GetIfVoltageSpot())
        {
            clone = Instantiate(_emptyVoltCirc);
        }
        else
        {
            clone = Instantiate(_emptyCirc);
        }
        // Replacing the circuit
        ReplaceSelectedWithGiven(clone);
        // Setting board place state to empty
        GameObject parent = _selected.transform.parent.gameObject;
        parent.GetComponent<BoardPlace>().UpdatePlaceState(BoardPlace.PlaceState.Empty);
    }

    private void ReplaceSelectedWithGiven(GameObject prefabClone)
    {
        // Parenting the circuit
        GameObject parent = _selected.transform.parent.gameObject;
        prefabClone.transform.SetParent(parent.transform, false);
        // Resetting position
        prefabClone.transform.localPosition = new Vector3(0, 0, 0);
        // Destroying old circuit and adding new one to board
        Destroy(_selected);
        _circuitBoard[_currRow, _currCol] = prefabClone;
        _selected = prefabClone;
        // Activating selection border
        SelectPlace();
    }

    public GameObject GetCurrentSelection()
    {
        return _selected;
    }

    // Pass false to rotate counterclockwise
    public void RotateCircuit(GameObject circuit, bool clockwise)
    {
        float angle;
        if (clockwise)
        {
            angle = -90;
        }
        else
        {
            angle = 90;
        }

        StartCoroutine(Rotate(circuit, angle));
    }

    private IEnumerator Rotate(GameObject circuit, float angle)
    {
        rotating = true;

        float elapsedTime = 0;
        float duration = 1f;
        Quaternion targetRotation = circuit.transform.rotation * Quaternion.Euler(0, 0, angle);
        while (elapsedTime < duration)
        {
            circuit.transform.rotation = Quaternion.RotateTowards(
                circuit.transform.rotation,
                targetRotation,
                elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rotating = false;
    }

    public Dictionary<string, GameObject> GetCircuitDictionary()
    {
        return _circuitMap;
    }

    public GameObject[,] GetCircuitBoard()
    {
        return _circuitBoard;
    }
}
