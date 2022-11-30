using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text _actualVoltageTxt;
    [SerializeField] Text _actualVoltageNumTxt;
    [SerializeField] Text _targetVoltageTxt;
    [SerializeField] Text _targetVoltageNumTxt;
    private int _targetVoltage;
    private WinCondition _winCondition;
    Color _orange = new Color(0.87f, 0.6f, 0f);

    private void Awake()
    {
        _targetVoltage = int.Parse(_targetVoltageNumTxt.text);
        _winCondition = FindObjectOfType<WinCondition>();
        SetTextColor(_orange);
    }

    public void UpdateCurrentVoltage(int newVoltage)
    {
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
    }

    public int GetTargetVoltage()
    {
        return _targetVoltage;
    }

    private void SetTextColor(Color col)
    {
        _actualVoltageTxt.color = col;
        _actualVoltageNumTxt.color = col;
        _targetVoltageTxt.color = col;
        _targetVoltageNumTxt.color = col;
    }
}
