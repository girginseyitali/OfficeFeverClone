using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    #region Private Variables
   
    [SerializeField] private Transform[] papersPlace = new Transform[10];
    [SerializeField] private Transform paperSpawnPoint;
    [SerializeField] private GameObject paper;

    #endregion

    #region Public Variables

    public float paperDeliveryTime, yAxis;
    public int countPapers;

    #endregion

    void Start()
    {
        for(int i = 0; i < papersPlace.Length; i++)
        {
            papersPlace[i] = transform.GetChild(0).GetChild(i);
        }

        PrintPaper(paperDeliveryTime);
    }

    public async void PrintPaper(float time)
    {

        var pp_Index = 0;

        while(countPapers < 100)
        {
            GameObject newPaper = Instantiate(paper, paperSpawnPoint.position, Quaternion.identity, transform.GetChild(3));

            newPaper.transform.DOJump(new Vector3(papersPlace[pp_Index].position.x, papersPlace[pp_Index].position.y + yAxis, 
                papersPlace[pp_Index].position.z), 2, 1, 0.5f).SetEase(Ease.OutQuad);

            if(pp_Index < 9)
            {
                pp_Index++;
            }
            else
            {
                pp_Index = 0;
                yAxis += 0.05f;
            }
            countPapers++;
            await UniTask.Delay(TimeSpan.FromSeconds(time));
        }
        
    }

    
   
}
