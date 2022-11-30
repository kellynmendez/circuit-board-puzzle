using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] GameObject _startCircuit;
    [SerializeField] GameObject _endCircuit;
    [SerializeField] GameObject _negativeCircuit;

    [Header("Feedback")]
    [SerializeField] AudioClip _winFX = null;

    AudioSource _audioSource = null;

    private List<Circuit> _circuits;
    private ActivateConnectionLine _connectionLine;
    private UIManager _uiManager;
    bool _win = false;

    private void Awake()
    {
        _win = false;
        _audioSource = GetComponent<AudioSource>();
        _circuits = new List<Circuit>();
        _connectionLine = FindObjectOfType<ActivateConnectionLine>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        _circuits.Add(_endCircuit.GetComponent<Circuit>());
        _circuits.Add(_negativeCircuit.GetComponent<Circuit>());
        _circuits.Add(_startCircuit.GetComponent<Circuit>());
    }
    
    public void CheckForWin()
    {
        bool connected = true;
        for (int i = 0; i < _circuits.Count && connected; i++)
        {
            connected = _circuits[i].GetIfConnected();
        }
        _win = connected;

        // win if circuits connected start to end and voltage is correct
        bool voltageMet = _connectionLine.CheckForVoltage();
        if (voltageMet && _win)
        {
            Win();
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

    private void Win()
    {
        PlayWinFX();
        StartCoroutine(WaitBeforeShowingWinScren());
    }

    private IEnumerator WaitBeforeShowingWinScren()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1;
        _uiManager.ShowWinScreen();
    }

    private void PlayWinFX()
    {
        if (_audioSource != null && _winFX != null)
        {
            _audioSource.PlayOneShot(_winFX, _audioSource.volume);
        }
    }
}
