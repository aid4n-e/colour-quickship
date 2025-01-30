using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public int[] spawnedArr;

    public GameObject[] prefabArr;

    public int[] deliveryArr;

    public Transform carryParent;
    public Transform boxParent;

    public int roundsComplete;
    public int boxesDelivered;
    // Start is called before the first frame update
    void Start()
    {
        spawnedArr = new int[prefabArr.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
