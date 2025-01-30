using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public Transform spawnPos;
    public Transform origin;

    public bool startTrigger = false;
    public bool switchDir = false;
    public bool majorSwitchDir = false;
    private bool clear = true;

    public BoxManager boxManager;

    private int turnCount = 0;
    private int majorTurnCount = 0;

    RaycastHit hit;

    void Update()
    {
        
        if(startTrigger)
        {
            BoxSpawn(Random.Range(1, boxManager.prefabArr.Length-1));
            startTrigger = false;

        }

    }

    public void BoxSpawn(int boxIndex)
    {
        clear = false;
        /*while (!clear)
        {
            if (Physics.Raycast(spawnPos.position + new Vector3(0, 0.5f, 0), spawnPos.TransformDirection(Vector3.down), out hit, 1))
            {
                if (hit.transform.tag.Equals("Box"))
                {
                    spawnPos.GetComponent<GridSnap>().MoveBox("up");
                }
                else
                    clear = true;
            }
        }*/



        GameObject newBox = Instantiate(boxManager.prefabArr[boxIndex], this.spawnPos.position, Quaternion.identity);
        newBox.GetComponent<BoxType>().boxManager = boxManager;
        newBox.GetComponent<GridSnap>().boxManager = boxManager;
        newBox.transform.parent = boxManager.boxParent;
        boxManager.spawnedArr[boxIndex]++;

        if(switchDir)
            spawnPos.GetComponent<GridSnap>().MoveBox("left");
        else
            spawnPos.GetComponent<GridSnap>().MoveBox("right");

        turnCount++;

        if (turnCount % 10 == 0)
        {
            if(!majorSwitchDir)
                spawnPos.GetComponent<GridSnap>().MoveBox("back");
            else
                spawnPos.GetComponent<GridSnap>().MoveBox("forward");

            switchDir = !switchDir;
        }

        if (turnCount == 100)
        {
            spawnPos.GetComponent<GridSnap>().MoveBox("up");
            majorSwitchDir = !majorSwitchDir;
            turnCount = 0;
        }


    }

    public void ResetProperties()
    {
        turnCount = 0;
        majorSwitchDir = false;
        switchDir = false;
    }


}
