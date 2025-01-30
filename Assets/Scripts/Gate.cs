using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private bool opening;
    private bool closing;
    public bool open;
    public bool closed;

    private float startTime;
    private float distance;
    public float openSpeed;
    public float closeSpeed;

    public Transform openPos;
    public Transform closePos;
    public Transform gate;

    // Update is called once per frame
    void Update()
    {
        if(opening)
        {
            float timeElapsed = (Time.time - startTime) * openSpeed;
            float distElapsed = timeElapsed / distance;

            gate.position = Vector3.Lerp(gate.position, openPos.position, distElapsed);

            if (gate.position == openPos.position)
            {
                opening = false;
            }
        }
        
        else if(closing)
        {
            float timeElapsed = (Time.time - startTime) * closeSpeed;
            float distElapsed = timeElapsed / distance;

            gate.position = Vector3.Lerp(gate.position, closePos.position, distElapsed);

            if (gate.position == closePos.position)
            {
                closing = false;
            }
        }
    }

    public void OpenGate()
    {
        opening = true;
        open = true;
        closed = false;
        closing = false;

        startTime = Time.time;
        distance = Vector3.Distance(gate.position, openPos.position);
    }

    public void CloseGate()
    {
        closing = true;
        closed = true;
        open = false;
        opening = false;

        startTime = Time.time;
        distance = Vector3.Distance(gate.position, closePos.position);
    }

}
