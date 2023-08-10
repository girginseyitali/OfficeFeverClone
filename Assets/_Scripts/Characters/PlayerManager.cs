using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Private Variables
    
    [SerializeField] private float playerSpeed;
    
    private Vector3 direction;
    private Camera cam;
    private Animator playerAnimator;
    [SerializeField] private List<Transform> papers = new List<Transform>();
    [SerializeField] private Transform paperPlace;
    

    #endregion
    void Start()
    {
        cam = Camera.main;
        playerAnimator = GetComponent<Animator>();
        papers.Add(paperPlace);
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

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(direction.x, 0f, direction.z), playerSpeed * Time.deltaTime);

            var offset = direction - transform.position;
            if(offset.magnitude > 1f)
                transform.LookAt(direction);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetBool("isRun", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("isRun", false);
        }


        if(papers.Count > 1)
        {
            for(int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers.ElementAt(i-1);
                var secondPaper = papers.ElementAt(i);

                secondPaper.position = new Vector3(Mathf.Lerp(secondPaper.position.x, firstPaper.position.x, Time.deltaTime * 10f),
                    Mathf.Lerp(secondPaper.position.y, firstPaper.position.y + 0.09f, Time.deltaTime * 10f), firstPaper.position.z);
            }
        }


        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {
            if (hit.collider.CompareTag("Table") && papers.Count < 21)
            {
                if(hit.collider.transform.childCount > 2)
                {
                    var paper = hit.collider.transform.GetChild(3);
                    paper.rotation = Quaternion.Euler(paper.rotation.x, Random.Range(0,180f), paper.rotation.z);
                    papers.Add(paper);
                    paper.parent = null;

                    if (hit.collider.transform.parent.GetComponent<Printer>().countPapers > 1)
                        hit.collider.transform.parent.GetComponent<Printer>().countPapers--;


                    if (hit.collider.transform.parent.GetComponent<Printer>().yAxis > 0f)
                        hit.collider.transform.parent.GetComponent<Printer>().yAxis -= 0.17f;

                    playerAnimator.SetBool("isCarry", true);
                    
                }
            }
        }
    }
}
