using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public RoundGenerator roundGenerator;

    public bool isButton;


    public Rigidbody button;

    private void Update()
    {
        if(isButton)
            button.AddRelativeForce(button.transform.forward, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isButton)
        {
            if (other.tag.Equals("Trigger"))
            {
                if (roundGenerator.state == 1)
                    roundGenerator.StartCoroutine(roundGenerator.AcceptDelivery());
                else if(roundGenerator.state == 0)
                    roundGenerator.StartCoroutine(roundGenerator.StartRound());
                else if (roundGenerator.state == -1)
                    roundGenerator.StartCoroutine(roundGenerator.StartPrep());
            }
        }

    }
}
