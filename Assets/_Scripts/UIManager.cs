using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject moneyPanel;
    private TextMeshProUGUI moneyText;
    [SerializeField] private PlayerManager playerManager;
    
    private void Start()
    {
        moneyText = moneyPanel.transform.GetComponentInChildren<TextMeshProUGUI>();
        moneyText.text = PlayerPrefs.GetInt("Dollar").ToString();
        playerManager.OnMoneyCollected += PlayerManager_OnMoneyCollected;
    }

    private void PlayerManager_OnMoneyCollected(object sender, EventArgs e)
    {
        moneyText.text = GameManager.Instance.Dollar.ToString();
    }
}
