using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text _actualVoltageTxt;
    [SerializeField] Text _actualVoltageNumTxt;
    [SerializeField] Text _targetVoltageTxt;
    [SerializeField] Text _targetVoltageNumTxt;
    [SerializeField] Image _voltOne;
    [SerializeField] Image _voltTwo;
    [SerializeField] Image _voltThree;
    [SerializeField] Image _voltFour;
    [SerializeField] GameObject _winScreen;

    [Header("Feedback")]
    [SerializeField] AudioClip _voltageChangeFX = null;

    AudioSource _audioSource = null;

    private int _targetVoltage;
    private WinCondition _winCondition;
    Color _orange = new Color(0.87f, 0.6f, 0f);
    Color _transparent = new Color(1.0f, 1.0f, 1.0f, 0.33f);
    private Color _currentColor;
    private bool _win = false;

    private void Awake()
    {
        _win = false;
        _winScreen.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        _targetVoltage = int.Parse(_targetVoltageNumTxt.text);
        _winCondition = FindObjectOfType<WinCondition>();
        SetTextColor(_orange);
        SetVoltImagesToNumber(0);
    }

    private void Update()
    {
        if (_win)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                ReloadLevel();
            }
        }
    }

    public void UpdateCurrentVoltage(int newVoltage)
    {
        //Playing audio
        PlayVoltageChangeFX();

        _actualVoltageNumTxt.text = newVoltage.ToString();
        if (newVoltage > _targetVoltage)
        {
            SetTextColor(Color.red);
        }
        else if (newVoltage == _targetVoltage)
        {
            SetTextColor(Color.green);
        }
        else if (newVoltage < _targetVoltage)
        {
            SetTextColor(_orange);
        }

        SetVoltImagesToNumber(newVoltage);
    }

    public int GetTargetVoltage()
    {
        return _targetVoltage;
    }

    private void SetTextColor(Color col)
    {
        _currentColor = col;
        _actualVoltageTxt.color = col;
        _actualVoltageNumTxt.color = col;
        _targetVoltageTxt.color = col;
        _targetVoltageNumTxt.color = col;
    }

    private void SetVoltImagesToNumber(int num)
    {
        if (num <= 0)
        {
            _voltOne.color = _transparent;
            _voltTwo.color = _transparent;
            _voltThree.color = _transparent;
            _voltFour.color = _transparent;
        }
        else if (num == 1)
        {
            _voltOne.color = _currentColor;
            _voltTwo.color = _transparent;
            _voltThree.color = _transparent;
            _voltFour.color = _transparent;
        }
        else if (num == 2)
        {
            _voltOne.color = _currentColor;
            _voltTwo.color = _currentColor;
            _voltThree.color = _transparent;
            _voltFour.color = _transparent;
        }
        else if (num == 3)
        {
            _voltOne.color = _currentColor;
            _voltTwo.color = _currentColor;
            _voltThree.color = _currentColor;
            _voltFour.color = _transparent;
        }
        else if (num >= 4)
        {
            _voltOne.color = _currentColor;
            _voltTwo.color = _currentColor;
            _voltThree.color = _currentColor;
            _voltFour.color = _currentColor;
        }
    }

    public void ShowWinScreen()
    {
        _win = true;
        _winScreen.SetActive(true);
    }

    private void PlayVoltageChangeFX()
    {
        if (_audioSource != null && _voltageChangeFX != null)
        {
            _audioSource.PlayOneShot(_voltageChangeFX, _audioSource.volume);
        }
    }

    private void ReloadLevel()
    {
        // Reloading the level
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }
}
