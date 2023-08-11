using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Dollar { get; set; }

    private void Start()
    {
        Dollar = !PlayerPrefs.HasKey("Dollar") ? 0 : PlayerPrefs.GetInt("Dollar");
    }
}
