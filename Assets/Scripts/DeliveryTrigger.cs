using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTrigger : MonoBehaviour
{
    public Delivery delivery;

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Box"))
        {
            print("Box");
            delivery.Check(other.gameObject);
        }
    }

}
