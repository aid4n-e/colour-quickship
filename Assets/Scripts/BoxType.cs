using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxType : MonoBehaviour
{
    //public bool isList;
    //public GameObject[] list;
    public string type;
    public int id;

    public BoxManager boxManager;

    void Start()
    {

        for(int i = 0; i < boxManager.prefabArr.Length; i++)
        {
            if(boxManager.prefabArr[i].GetComponent<BoxType>().type.Equals(type))
            {
                id = i;
            }
        }

    }
}
