using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnap : MonoBehaviour
{
    public BoxManager boxManager;

    public float speed;
    private float startTime;
    private float distance;

    public bool pulled;
    public bool stopped;
    public bool moving;
    public bool move;
    public bool aligned;

    public string moveDir;

    public Transform box;

    private Vector3 newPos;

    public RaycastHit hit;

    public AudioSource audio;
    
    private void Start()
    {
        if(speed<1)
            speed = 1;
        box = this.transform;
    }

    void Update()
    {
        if(!pulled && !moving)
        {
            if(box.position != new Vector3(Mathf.Round(box.position.x), Mathf.Round(box.position.y), Mathf.Round(box.position.z)))
            {
                newPos = new Vector3(Mathf.Round(box.position.x), Mathf.Round(box.position.y), Mathf.Round(box.position.z));
                distance = Vector3.Distance(box.position, newPos);
                startTime = Time.time;
                moving = true;
            }
            aligned = true;
        }

        if(pulled)
        {
            aligned = false;
            this.transform.parent = boxManager.carryParent;


        }
        else if(stopped)
        {
            aligned = false;
            stopped = false;
            pulled = false;

            this.transform.parent = boxManager.boxParent;
        }

        if(moving)
        {
            aligned = false;
            float timeElapsed = (Time.time - startTime) * speed;
            float distElapsed = timeElapsed / distance;

            box.position = Vector3.Lerp(box.position, newPos, distElapsed);

            if (box.position == newPos)
            {
                moving = false;
                
            }
        }


        // Manually activate MoveBox
        if (move)
        {
            move = false;
            MoveBox(moveDir);
        }
    }

    public void MoveBox(string dir)
    {
        aligned = false;

        switch(dir)
        {
            case "up":
                if (CheckMovable(dir, Vector3.up))
                {
                    PushBox(dir, Vector3.up);
                    newPos = new Vector3(box.position.x, box.position.y + 1, box.position.z);
                    moving = true;
                }
                break;

            case "down":
                if (CheckMovable(dir, Vector3.down))
                {
                    PushBox(dir, Vector3.down);
                    newPos = new Vector3(box.position.x, box.position.y - 1, box.position.z);
                    moving = true;
                }
                break;

            case "right":
                if (CheckMovable(dir, Vector3.right))
                {
                    PushBox(dir, Vector3.right);
                    newPos = new Vector3(box.position.x + 1, box.position.y, box.position.z);
                    moving = true;
                }
                break;

            case "left":
                if (CheckMovable(dir, Vector3.left))
                {
                    PushBox(dir, Vector3.left);
                    newPos = new Vector3(box.position.x - 1, box.position.y, box.position.z);
                    moving = true;
                }
                break;

            case "forward":
                if (CheckMovable(dir, Vector3.forward))
                {
                    PushBox(dir, Vector3.forward);
                    newPos = new Vector3(box.position.x, box.position.y, box.position.z + 1);
                    moving = true;
                }
                break;

            case "back":
                if (CheckMovable(dir, Vector3.back))
                {
                    PushBox(dir, Vector3.back);
                    newPos = new Vector3(box.position.x, box.position.y, box.position.z - 1);
                    moving = true;
                }
                break;
        }

        audio.Play();

        startTime = Time.time;
        distance = Vector3.Distance(box.position, newPos);

        StartCoroutine(CheckFloating());
    }

    void PushBox(string dir, Vector3 vectorDir)
    {
        aligned = false;
        if (Physics.Raycast(box.position, box.TransformDirection(vectorDir), out hit, 1))
            if (hit.transform.tag.Equals("Box"))
                hit.transform.GetComponent<GridSnap>().MoveBox(dir);
    }
    

    public bool CheckMovable(string dir, Vector3 vectorDir)
    {
        if (Physics.Raycast(box.position, box.TransformDirection(vectorDir), out hit, 1))
            if (hit.transform.tag.Equals("Box"))
                return (hit.transform.GetComponent<GridSnap>().CheckMovable(dir, vectorDir));
            else if (hit.transform.tag.Equals("Wall"))
                return false;

        if(vectorDir == Vector3.up)
        {
            if (Physics.Raycast(box.position, box.TransformDirection(Vector3.down), out hit, 1))
                return true;
            else
                return false;
        }

        return true;
    }

    IEnumerator CheckFloating()
    {
        yield return new WaitForSeconds(1f);
        PushBox("down", Vector3.down);
        newPos = new Vector3(box.position.x, box.position.y - 1, box.position.z);
        moving = true;
    }

}
