using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuyArea : MonoBehaviour
{
    public bool isUnlocked = false;
    
    [SerializeField] private Image buyAreaImage;
    [SerializeField] private TextMeshProUGUI buyAreaText;
    [SerializeField] private int moneyToUnlock, remainMoney;
    [SerializeField] private GameObject unlockedObject;
    

    private void Awake()
    {
        buyAreaText.text = moneyToUnlock.ToString();
        remainMoney = moneyToUnlock;
    }

    public void UpdateMoneyToUnlockText(int moneyAmount)
    {
        remainMoney -= moneyAmount;
        if (remainMoney <= 0)
        {
            remainMoney = 0;
            isUnlocked = true;
            InstantiateObject();
        }
        
        buyAreaText.text = remainMoney.ToString();
    }
    
    private void InstantiateObject()
    {
        GameObject newObject = Instantiate(unlockedObject, transform.position, Quaternion.identity);
        buyAreaImage.transform.parent.gameObject.SetActive(false);
    }
}
