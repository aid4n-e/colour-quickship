using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MoveGesture : MonoBehaviour
{
    public VRController vrController;
    public BoxManager boxManager;
    public SelectTrigger selectTrigger;

    public bool rightHand;
    public bool startGrab;
    public bool grabbing;
    private bool pulling;


    public SteamVR_Action_Boolean grabValueR;
    public SteamVR_Action_Boolean grabValueL;

    public string dir;

    public Transform laser;
    public Transform hand;
    public Transform target;

    public RaycastHit hit;

    private Vector3 startPos;
    private Vector3 endPos;


    void Start()
    {
        grabValueR = vrController.grabValueR;
        grabValueL = vrController.grabValueL;
    }

    // Update is called once per frame
    void Update()
    {
        target = IdentifyBlock();

        if(!grabValueR.state)
        {
            selectTrigger.selecting = false;
        }

        if (boxManager.carryParent.childCount > 0)
        {
            grabbing = false;
            pulling = true;
        }

        else if (grabbing)
        {
            RecordGesture();
        }


        if(!pulling && !grabbing)
        {
            
            if(grabValueL.state)
            {
                RecordGesture();
            }
            else if(grabValueR.state)
            {
                BlockCarry();
            }
        }



    }



    Transform IdentifyBlock()
    {
        if (Physics.Raycast(laser.position, laser.TransformDirection(Vector3.forward), out hit, 10))
            if (hit.transform.tag.Equals("Box"))
                return (hit.transform);

        return target;
    }

    void RecordGesture()
    {
        if (!grabbing && target != null)
        {
            grabbing = true;
            startPos = hand.position;
        }

        else if (!grabValueL.state)
        {
            grabbing = false;
            endPos = hand.position;
            IdentifyGesture(startPos, endPos);
            target.GetComponent<GridSnap>().MoveBox(dir);
        }
    }

    void BlockCarry()
    {
        selectTrigger.selecting = true;

    }


    void IdentifyGesture(Vector3 start, Vector3 end)
    {
        Vector3 dirVector = start - end;
        dir = DirectionSnap(dirVector);
        print(dir);
    }

    string DirectionSnap(Vector3 dir)
    {
        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            if (dir.x < 0)
                return "right";
            else
                return "left";
        }

        else if(Mathf.Abs(dir.y) > Mathf.Abs(dir.x) && Mathf.Abs(dir.y) > Mathf.Abs(dir.z))
        {
            if (dir.y < 0)
                return "up";
            else
                return "down";
        }

        else if (Mathf.Abs(dir.z) > Mathf.Abs(dir.x) && Mathf.Abs(dir.z) > Mathf.Abs(dir.y))
        {
            if (dir.z < 0)
                return "forward";
            else
                return "back";
        }

        return "up";
    }



}


