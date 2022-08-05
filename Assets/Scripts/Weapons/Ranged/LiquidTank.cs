using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidTank : MonoBehaviour
{
    [SerializeField]
    private int _maxLiquidAmount = 10;
    [SerializeField]
    private int _currentLiquidAmount;
    [SerializeField]
    private GameObject _projectilePrefab;

    private void Start()
    {
        _currentLiquidAmount = _maxLiquidAmount;
    }

    public int CheckCurrentLiquid()
    {
        return _currentLiquidAmount;
    }
    public GameObject GetProjectilePrefab()
    {
        return _projectilePrefab;
    }

    public void SubstractLiquid(int liquidAmount) 
    {
        _currentLiquidAmount -= liquidAmount;
    }

}
