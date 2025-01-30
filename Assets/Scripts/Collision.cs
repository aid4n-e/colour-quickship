using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    public Transform head;
    public Transform feet;
    public CapsuleCollider capsule;

    void FixedUpdate()
    {
        capsule.height = Vector3.Distance( new Vector3(0, feet.transform.position.y, 0), new Vector3(0, head.transform.position.y, 0) );
        Vector3 capsuleCenter = transform.InverseTransformPoint(head.transform.position);
        capsule.center = new Vector3(capsuleCenter.x, capsule.height / 2, capsuleCenter.z);
    }
}