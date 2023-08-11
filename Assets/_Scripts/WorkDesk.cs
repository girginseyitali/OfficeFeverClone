using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class WorkDesk : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private Transform dollarPlace;
    [SerializeField] private GameObject dollarPrefab;
    private float yAxis = 0f;
    private IEnumerator makeMoneyIE;

    #endregion

    private void Awake()
    {
        makeMoneyIE = MakeMoney();
    }

    private void Start()
    {
        Work();
    }

    private IEnumerator MakeMoney()
    {
        var counter = 0;
        var dollarPlaceIndex = 0;

        yield return new WaitForSecondsRealtime(2f);

        while (counter < transform.childCount)
        {
            GameObject newDollar = Instantiate(dollarPrefab, new Vector3(dollarPlace.GetChild(dollarPlaceIndex).position.x,yAxis,dollarPlace.GetChild(dollarPlaceIndex).position.z),
                dollarPlace.GetChild(dollarPlaceIndex).rotation);

            //newDollar.transform.DOScale(new Vector3(0.9f, 0.9f, .9f), .5f).SetEase(Ease.OutElastic);

            if (dollarPlaceIndex < dollarPlace.childCount - 1)
            {
                dollarPlaceIndex++;
            }
            else
            {
                dollarPlaceIndex = 0;
                yAxis += 0.1f;
            }

            yield return new WaitForSecondsRealtime(3f);
            
        }
    }

    public void Work()
    {
        InvokeRepeating(nameof(DoSubmitPapers), 2f, 1f);
        
    }
    
    
    private async void DoSubmitPapers()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            StartCoroutine(makeMoneyIE);
            
        }
        else
        {
            StopCoroutine(makeMoneyIE);
            yAxis = 0f;
        }
    }
}
