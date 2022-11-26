using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState CurrentGameState;
    private BoardManager _boardManager;
    private InventoryManager _inventoryManager;

    public enum GameState
    {
        CircuitBoard,
        Inventory
    }

    private void Awake()
    {
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
            if (CurrentGameState == GameState.CircuitBoard)
            {
                UpdateGameState(GameState.Inventory);
            }
            else
            {
                UpdateGameState(GameState.CircuitBoard);
            }
        }
    }

    public void UpdateGameState(GameState gameState)
    {
        CurrentGameState = gameState;

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
}
