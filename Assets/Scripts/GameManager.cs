using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState CurrentGameState;
    private BoardManager _boardManager;
    private InventoryManager _inventoryManager;

    [Header("Feedback")]
    [SerializeField] AudioClip _switchFX = null;

    AudioSource _audioSource = null;

    public enum GameState
    {
        CircuitBoard,
        Inventory
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _boardManager = FindObjectOfType<BoardManager>();
        _inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void Start()
    {
        // Setting starting game state
        CurrentGameState = GameState.CircuitBoard;
    }

    private void Update()
    {
        // Switching between circuit board and inventory
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject selected = _boardManager.GetCurrentSelection();
            BoardPlace place = selected.transform.parent.gameObject.GetComponent<BoardPlace>();
            // Can only go into inventory if current spot is empty
            if (CurrentGameState == GameState.CircuitBoard && 
                place.GetCurrentPlaceState() != BoardPlace.PlaceState.Filled)
            {
                UpdateGameState(GameState.Inventory);
            }
            else if (CurrentGameState == GameState.Inventory)
            {
                UpdateGameState(GameState.CircuitBoard);
            }
        }
    }

    public void UpdateGameState(GameState gameState)
    {
        CurrentGameState = gameState;

        if (gameState == GameState.Inventory)
        {
            PlaySwitchFX();
        }

        switch (CurrentGameState)
        {
            case GameState.CircuitBoard:
                SwitchToCircuitBoard();
                break;
            case GameState.Inventory:
                SwitchToInventory();
                break;
            default:
                Debug.Log("Error: State doesn't exist");
                break;
        }
    }

    private void SwitchToCircuitBoard()
    {
        _inventoryManager.SwitchFromInventoryToBoard();
    }

    private void SwitchToInventory()
    {
        _inventoryManager.SwitchFromBoardToInventory();
    }

    public GameState GetCurrentGameState()
    {
        return CurrentGameState;
    }

    private void PlaySwitchFX()
    {
        if (_audioSource != null && _switchFX != null)
        {
            _audioSource.PlayOneShot(_switchFX, _audioSource.volume);
        }
    }
}
