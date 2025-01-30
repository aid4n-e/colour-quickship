using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundGenerator : MonoBehaviour
{

    public int unlocked;

    public int amount;

    public BoxSpawner boxSpawner;
    public BoxManager boxManager;
    public Timer timer;
    public UIToast uiToast;

    public TextMesh info;

    public GameObject deliveryTrigger;

    public bool spawn;
    public bool start;

    public int roundNumber = 1;
    public int state = -1;

    public Transform[] gates;
    public Transform[] scanners;


    void Start()
    {
        deliveryTrigger.SetActive(false);
    }


    void Update()
    {
        if(spawn)
        {
            spawn = false;
            StartCoroutine(SpawnShipment(unlocked, amount));
        }
        if (start)
        {
            start = false;
            GenerateDelivery();
        }


    }

    public IEnumerator StartPrep()
    {
        state = 0;

        timer.ResetTimer(20 + (roundNumber*5));

        amount = Random.Range(((roundNumber * 5)+5) / 2, Mathf.Clamp(((roundNumber * 5) + 5),1,100));
        StartCoroutine(SpawnShipment(unlocked, amount));


        yield return new WaitForSeconds(5f);

        gates[1].GetComponent<Gate>().OpenGate();
        gates[0].GetComponent<Gate>().CloseGate();

    }

    public IEnumerator StartRound()
    {

        if(!DeliveryCollected())
        {
            uiToast.StartCoroutine(uiToast.Alert("Remove items from shipping bay!"));
        }

        else
        {
            state = 1;

            GenerateDelivery();

            gates[0].GetComponent<Gate>().OpenGate();
            gates[1].GetComponent<Gate>().CloseGate();

            timer.timerOn = true;

            uiToast.StartCoroutine(uiToast.Alert("Round " + roundNumber + " Start!"));

            string temp = "";
            for (int i = 0; i < boxManager.deliveryArr.Length; i++)
            {
                print("Text generating");
                if (boxManager.deliveryArr[i] > 0)
                {
                    temp += boxManager.deliveryArr[i] + " " + boxManager.prefabArr[i].GetComponent<BoxType>().type + "\n";
                }
            }

            info.text = temp;

            yield return new WaitForSeconds(60f);

            gates[0].GetComponent<Gate>().CloseGate();




        }

    }

    IEnumerator SpawnShipment(int unlocked, int amount)
    {
        boxSpawner.spawnPos.position = boxSpawner.origin.position;
        for (int i = 0; i < amount; i++)
        {
            int rand = Random.Range(0, unlocked);
            boxSpawner.BoxSpawn(rand);
            yield return new WaitForSeconds(0.2f);
        }

        boxSpawner.ResetProperties();

    }

    public void GenerateDelivery()
    {
        int sum = 0;
        for(int i = 0; i < boxManager.spawnedArr.Length; i++)
        {
            sum += boxManager.spawnedArr[i];
        }
        print("sum1 = " + sum);

        int deliveryAmount = Random.Range(sum / 5, (sum / 5) + 5);
        boxManager.deliveryArr = new int[unlocked];
        print("deliveryAmount = " + deliveryAmount);
        print("");

        sum = 0;
        int failsafe = 0;
        while (sum < deliveryAmount)
        {
            print("ITERATION " + (failsafe+1));
            int randType = Random.Range(0, unlocked);
            int randAmount = 0;

            print("randType = " + randType);

        if (deliveryAmount - sum <= (boxManager.spawnedArr[randType]-boxManager.deliveryArr[randType]))
            {
                print("CASE1");
                randAmount = Random.Range(0, (deliveryAmount-sum)+1);
            }
            else
            {
                print("CASE2");
                randAmount = Random.Range(0, boxManager.spawnedArr[randType] - boxManager.deliveryArr[randType]);
            }

            print("randAmount = " + randAmount);

        boxManager.deliveryArr[randType] += randAmount;
            sum += randAmount;

            print("sum2 = " + sum);


            failsafe++;
            if (failsafe == 20)
                break;
        }

    }

    public IEnumerator AcceptDelivery()
    {
        gates[0].GetComponent<Gate>().CloseGate();

        yield return new WaitForSeconds(0.5f);
        deliveryTrigger.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        int sum = 0;
        for (int i = 0; i < boxManager.deliveryArr.Length; i++)
        {
            sum += boxManager.deliveryArr[i];
        }



        if(sum>0)
        {
            deliveryTrigger.SetActive(false);
            gates[0].GetComponent<Gate>().OpenGate();

            string temp = "";
            for (int i = 0; i < boxManager.deliveryArr.Length; i++)
            {
                print("Text generating");
                if (boxManager.deliveryArr[i] > 0)
                {
                    temp += boxManager.deliveryArr[i] + " " + boxManager.prefabArr[i].GetComponent<BoxType>().type + "\n";
                }
            }
        }

        else
        {
            info.text = "";

            boxManager.roundsComplete++;
            roundNumber++;
            if(unlocked < boxManager.prefabArr.Length)
                unlocked++;
            state = -1;

            deliveryTrigger.SetActive(false);

            uiToast.StartCoroutine(uiToast.Alert("Round " + roundNumber + " Complete!"));
            yield return new WaitForSeconds(6f);
            uiToast.StartCoroutine(uiToast.Alert("Press button to receive shipment."));

        }


    }

    private bool DeliveryCollected()
    {
        RaycastHit hit;
        for(int i = 0; i < scanners.Length; i++)
        {
            if (Physics.Raycast(scanners[i].position, scanners[i].TransformDirection(Vector3.forward), out hit, 20))
            {
                if (hit.transform.tag.Equals("Box"))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public IEnumerator Failure()
    {
        print("FAILURE!");
        info.text = "";
        uiToast.StartCoroutine(uiToast.Alert("Time ran out!\nYou lose!"));
        yield return new WaitForSeconds(6f);
        uiToast.StartCoroutine(uiToast.Alert("Rounds Complete: " + boxManager.roundsComplete + "\nBoxes Delivered: " + boxManager.boxesDelivered ));
        yield return new WaitForSeconds(6f);
        Application.Quit();
    }
}
