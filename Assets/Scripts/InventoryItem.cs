using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] int _numOfItemLeft = 0;
    [SerializeField] Text _numOfItemLeftText;

    private void Awake()
    {
        SetTextToNumberLeft();
    }

    public bool GetItemEmpty()
    {
        if (_numOfItemLeft > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void RemoveItem()
    {
        _numOfItemLeft--;
        SetTextToNumberLeft();
    }

    public void AddItemBack()
    {
        _numOfItemLeft++;
        SetTextToNumberLeft();
    }

    public int GetNumberItemsLeft()
    {
        return _numOfItemLeft;
    }

    private void SetTextToNumberLeft()
    {
        _numOfItemLeftText.text = _numOfItemLeft.ToString();
    }
}
