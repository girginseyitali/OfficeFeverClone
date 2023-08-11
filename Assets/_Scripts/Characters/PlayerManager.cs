using System;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
    #region Private Variables
    
    [SerializeField] private float playerSpeed;
    
    private Vector3 direction;
    private Camera cam;
    private PlayerAnimationController playerAnimationController;
    [SerializeField] private List<Transform> papers = new List<Transform>();
    [SerializeField] private Transform paperPlace;
    private float yAxis, delay;
    private static readonly int IsRun = Animator.StringToHash("isRun");

    #endregion

    #region Public Variables

    public event EventHandler OnMoneyCollected;

    #endregion
    
    
    void Start()
    {
        cam = Camera.main;
        playerAnimationController = GetComponent<PlayerAnimationController>();
        papers.Add(paperPlace);
        
    }

    private void UIManager_OnMoneyCollected(object sender, EventArgs e)
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                direction = ray.GetPoint(distance);
            }

            var position = transform.position;
            position = Vector3.MoveTowards(position, new Vector3(direction.x, 0f, direction.z), playerSpeed * Time.deltaTime);
            transform.position = position;

            var offset = direction - position;
            if(offset.magnitude > 1f)
                transform.LookAt(direction);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerAnimationController.animator.SetBool("isRun", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerAnimationController.animator.SetBool("isRun", false);
        }


        if(papers.Count > 1)
        {
            for(int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers.ElementAt(i-1);
                var secondPaper = papers.ElementAt(i);

                var secondPaperPosition = secondPaper.position;
                secondPaperPosition = new Vector3(Mathf.Lerp(secondPaperPosition.x, firstPaper.position.x, Time.deltaTime * 10f),
                    Mathf.Lerp(secondPaperPosition.y, firstPaper.position.y + 0.09f, Time.deltaTime * 10f), firstPaper.position.z);
                secondPaper.position = secondPaperPosition;
            }
        }


        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {
            if (hit.collider.CompareTag("Table") && papers.Count < 21)
            {
                if(hit.collider.transform.childCount > 2)
                {
                    var paper = hit.collider.transform.GetChild(1);
                    paper.rotation = Quaternion.Euler(paper.rotation.x, Random.Range(0,180f), paper.rotation.z);
                    papers.Add(paper);
                    paper.parent = null;

                    if (hit.collider.transform.parent.GetComponent<Printer>().countPapers > 1)
                        hit.collider.transform.parent.GetComponent<Printer>().countPapers--;


                    if (hit.collider.transform.parent.GetComponent<Printer>().yAxis > 0f)
                        hit.collider.transform.parent.GetComponent<Printer>().yAxis -= 0.17f;

                    playerAnimationController.animator.SetBool("isCarry", true);
                    
                }
            }
            if (hit.collider.CompareTag("PaperPlace") && papers.Count > 1)
            {
                var workDesk = hit.collider.transform;

                yAxis = workDesk.childCount > 0 ? workDesk.GetChild(workDesk.childCount - 1).position.y : workDesk.position.y;

                for(int i = papers.Count - 1; i >= 1; i--)
                {
                    var position = workDesk.position;
                    papers[i].DOJump(new Vector3(position.x, yAxis, position.z), 2f, 1, 0.02f).SetDelay(delay).
                        SetEase(Ease.Flash);

                    papers.ElementAt(i).parent = workDesk;
                    papers.RemoveAt(i);

                    yAxis += 0.05f;
                    delay += 0.1f;
                }

                if(papers.Count <= 1)
                {
                    playerAnimationController.animator.SetBool("isCarry", false);
                    delay = 0f;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dollar"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.Dollar += 5;
            PlayerPrefs.SetInt("Dollar", GameManager.Instance.Dollar);
            OnMoneyCollected?.Invoke(this, EventArgs.Empty);
        }
    }
}
