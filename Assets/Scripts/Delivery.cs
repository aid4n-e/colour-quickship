using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    public BoxManager boxManager;


    public void Check(GameObject boxObject)
    {
        BoxType boxType = boxObject.GetComponent<BoxType>();


        if(boxManager.deliveryArr[boxType.id] > 0)
        {
            boxManager.boxesDelivered++;
            boxManager.deliveryArr[boxType.id]--;
            boxManager.spawnedArr[boxType.id]--;
            Destroy(boxObject);
        }


    }

}
