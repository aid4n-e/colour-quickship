using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTrigger : MonoBehaviour
{
    public bool selecting;
    public BoxManager boxManager;

    private void Update()
    {
        if(!selecting && boxManager.carryParent.childCount > 0)
        {
            for (int i = 0; i < boxManager.carryParent.childCount; i++)
            {
                boxManager.carryParent.GetChild(i).GetComponent<GridSnap>().pulled = false;
                boxManager.carryParent.GetChild(i).parent = boxManager.boxParent;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Box"))
        {
            if (selecting)
            {
                other.GetComponent<GridSnap>().pulled = true;
            }
        }

    }
}
