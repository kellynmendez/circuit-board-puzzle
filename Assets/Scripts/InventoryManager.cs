using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private InventoryState CurrentInventoryState;

    [SerializeField] GameObject _normalInventoryView;
    [SerializeField] GameObject _voltageInventoryView;
    [SerializeField] GameObject[] _normalInventory;
    [SerializeField] GameObject[] _voltInventory;

    [Header("Sprites")]
    [SerializeField] Sprite _selectedSprite;
    [SerializeField] Sprite _noMoreItemSelSprite;

    [Header("Constants")]
    [SerializeField] float _scaleDuration = 0.25f;
    [SerializeField] float _timeIntervalBwScale = 0.015f;

    [Header("Feedback")]
    [SerializeField] AudioClip _addOrRemSFX = null;
    [SerializeField] AudioClip _selectFX = null;

    AudioSource _audioSource = null;

    private Dictionary<string, InventoryItem> _inventoryMap;

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
        _audioSource = GetComponent<AudioSource>();
        _inventoryMap = new Dictionary<string, InventoryItem>();
        _gameManager = FindObjectOfType<GameManager>();
        _boardManager = FindObjectOfType<BoardManager>();
    }

    private void Start()
    {
        // Populating circuit dictionary
        foreach (GameObject circ in _normalInventory)
        {
            _inventoryMap.Add(circ.GetComponent<Circuit>().GetCircuitDictionaryKey(), 
                circ.GetComponent<InventoryItem>());
        }
        foreach (GameObject circ in _voltInventory)
        {
            _inventoryMap.Add(circ.GetComponent<Circuit>().GetCircuitDictionaryKey(),
                circ.GetComponent<InventoryItem>());
        }

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
        StartCoroutine(MaximizeInventoryScreen());
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
        // Setting border according to items left and activating
        SelectAndActivateBorder();
    }

    public void SwitchFromInventoryToBoard()
    {
        StartCoroutine(MinimizeInventoryScreen());
        // Unselect the current selection
        _selected.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void HandleUserInput()
    {
        // Add circuit to board
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InventoryItem circuit = _selected.GetComponent<InventoryItem>();
            if (!circuit.GetItemEmpty())
            {
                // Adding circuit to board
                AddCircuitToBoard();
                // Removing from inventory
                circuit.RemoveItem();
            }
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

    private void AddCircuitToBoard()
    {
        // Adding the circuit
        _boardManager.AddCircuitToBoard(_selected);
        // Play audio
        PlayAddOrRemoveFX();
        // Going back to the board
        _gameManager.UpdateGameState(GameManager.GameState.CircuitBoard);
    }

    public void RemoveCircuitFromBoard(string circuitKey)
    {
        // Play audio
        PlayAddOrRemoveFX();
        // Adding item back to inventory
        InventoryItem item = _inventoryMap[circuitKey];
        item.AddItemBack();
    }

    private void SelectSpot()
    {
        PlaySelectFX();
        // Removing selection border from previous space
        _selected.transform.GetChild(1).gameObject.SetActive(false);
        // Selecting the new space
        if (CurrentInventoryState == InventoryState.NormalInventory)
        {
            _selected = _normalInventory[_currRow];
            // Setting border according to items left and activating
            SelectAndActivateBorder();
        }
        else if (CurrentInventoryState == InventoryState.VoltInventory)
        {
            _selected = _voltInventory[_currRow];
            // Setting border according to items left and activating
            SelectAndActivateBorder();
        }
    }

    private void SelectAndActivateBorder()
    {
        int num = _selected.GetComponent<InventoryItem>().GetNumberItemsLeft();
        if (num == 0)
        {
            _selected.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = _noMoreItemSelSprite;
        }
        else
        {
            _selected.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = _selectedSprite;
        }
        _selected.transform.GetChild(1).gameObject.SetActive(true);
    }

    public InventoryState GetCurrentInventoryState()
    {
        return CurrentInventoryState;
    }

    private void PlayAddOrRemoveFX()
    {
        if (_audioSource != null && _addOrRemSFX != null)
        {
            _audioSource.PlayOneShot(_addOrRemSFX, _audioSource.volume);
        }
    }

    private void PlaySelectFX()
    {
        if (_audioSource != null && _selectFX != null)
        {
            _audioSource.PlayOneShot(_selectFX, _audioSource.volume);
        }
    }

    private IEnumerator MinimizeInventoryScreen()
    {
        float start = Time.time;
        while (start + _scaleDuration > Time.time)
        {
            // Slowly shrink screen
            gameObject.transform.localScale = new Vector3(
                gameObject.transform.localScale.x  * 0.99f,
                gameObject.transform.localScale.y * 0.99f,
                gameObject.transform.localScale.z);
            yield return new WaitForSeconds(_timeIntervalBwScale);
        }
        yield break;
    }

    private IEnumerator MaximizeInventoryScreen()
    {
        float start = Time.time;
        while (start + _scaleDuration > Time.time)
        {
            // Slowly shrink screen
            gameObject.transform.localScale = new Vector3(
                gameObject.transform.localScale.x * 1.01f,
                gameObject.transform.localScale.y * 1.01f,
                gameObject.transform.localScale.z);
            yield return new WaitForSeconds(_timeIntervalBwScale);
        }
        yield break;
    }
}
