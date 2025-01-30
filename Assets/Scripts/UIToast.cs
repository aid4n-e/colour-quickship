using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToast : MonoBehaviour
{
    public TextMesh uiText;
    
    private bool moving;

    public float speed;
    private float startTime;
    private float distance;

    public Transform textTrans;

    public Transform hudPos;
    public Transform offHudPos;
    private Vector3 newPos;

    void Start()
    {
        
    }

    private void Update()
    {
        if(moving)
        {
            float timeElapsed = (Time.time - startTime) * speed;
            float distElapsed = timeElapsed / distance;

            textTrans.transform.position = Vector3.Lerp(textTrans.transform.position, newPos, distElapsed);
            if (textTrans.transform.position == newPos)
                moving = false;
            if(textTrans.transform.position == offHudPos.position)
            {
                moving = false;
                uiText.text = "";
            }

        }
    }


    public IEnumerator Alert(string message)
    {
        uiText.text = message;
        startTime = Time.time;

        textTrans.position = offHudPos.position;
        newPos = hudPos.position;
        distance = Vector3.Distance(hudPos.position, textTrans.position);
        moving = true;

        yield return new WaitForSeconds(5f);

        startTime = Time.time;
        newPos = offHudPos.position;
        distance = Vector3.Distance(offHudPos.position, textTrans.position);
        moving = true;

        
    }
}
