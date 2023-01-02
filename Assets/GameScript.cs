using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public Image image;
    public Sprite[] plant = new Sprite[5];
    public Image progress;
    public Image demandProgress;
    public Image tipClickHere;

    public int supply;
    public int demand;
    public double rate;
    public double basePrice;
    public double currentPrice;
    public double sellPrice;
    public double totalPrice;
    public int moreDemandTimer;

    public TextMeshProUGUI Text_PlayerMoney;
    public TextMeshProUGUI Text_Supply;
    public TextMeshProUGUI Text_Demand;
    public TextMeshProUGUI Text_Rate;
    public TextMeshProUGUI Text_BasePrice;
    public TextMeshProUGUI Text_CurrentPrice;
    public TextMeshProUGUI Text_TotalPrice;
    public TextMeshProUGUI Text_MoreDemandTimer;

    bool growing = false;
    private void Start()
    {
        plant[0] = Resources.Load<Sprite>("Pot2");
        plant[1] = Resources.Load<Sprite>("Pot3");
        plant[2] = Resources.Load<Sprite>("Pot4");
        plant[3] = Resources.Load<Sprite>("Pot5");
        plant[4] = Resources.Load<Sprite>("Pot6");

        supply = 0;
        demand = 15;
        rate = 1;
        basePrice = 6;
        currentPrice = 0;
        totalPrice = 0;
        moreDemandTimer = 180;

        StartCoroutine(MoreDemand());
    }

    private void Update()
    {
        Text_PlayerMoney.text = "$" + PlayerScript.Money.ToString("f2");
        Text_Supply.text = supply.ToString() + "g";
        Text_Demand.text = demand.ToString() + "g";
        Text_Rate.text = (rate * 100).ToString("f2") + "%";
        Text_BasePrice.text = "$" + basePrice.ToString("f2") + " / g";
        Text_CurrentPrice.text = "$" + currentPrice.ToString("f2") + " / g";
        Text_TotalPrice.text = "$" + totalPrice.ToString("f2");
        Text_MoreDemandTimer.text = moreDemandTimer.ToString() + "s";

        rate = currentPrice / basePrice;

        if (demand > supply)
        {
            currentPrice = basePrice + ((demand - supply) * (basePrice * 0.05));
        }
        else if (demand == supply)
        {
            currentPrice = basePrice;
        }
        else
        {
            currentPrice = basePrice - ((supply - demand) * (basePrice * 0.05));
        }
    }

    IEnumerator MoreDemand()
    {
        while(true)
        {

            moreDemandTimer--;

            if(moreDemandTimer <= 0)
            {
                moreDemandTimer = 180;
                demand += 10;
            }

            yield return new WaitForSeconds(1f);
        }
        
    }

    public void OnPlantCLick()
    {
        if(!growing)
        {
            tipClickHere.enabled= false;
            StartCoroutine(GrowPlant());
        } else
        {
            Debug.Log("Plant is still growing!");
        }
    }

    IEnumerator GrowPlant()
    {
        growing = true;
        image.sprite = plant[0];

        image.sprite = plant[1];
        progress.rectTransform.localScale = new Vector3((float)0.25, 1);

        while (growing)
        {

            yield return new WaitForSeconds(1f);

            if (image.sprite == plant[1])
            {
                image.sprite = plant[2];
                progress.rectTransform.localScale = new Vector3((float)0.50, 1);
            }
            else if (image.sprite == plant[2])
            {
                image.sprite = plant[3];
                progress.rectTransform.localScale = new Vector3((float)0.75, 1);
            }
            else if (image.sprite == plant[3])
            {
                image.sprite = plant[4];
                progress.rectTransform.localScale = new Vector3((float)1.00, 1);
            }
            else if (image.sprite == plant[4])
            {
                image.sprite = plant[0];
                progress.rectTransform.localScale = new Vector3((float)0, 1);

                if(demand > supply)
                {
                    totalPrice += basePrice + ((demand - supply) * (basePrice * 0.05));
                } else if(demand == supply)
                {
                    totalPrice += basePrice;
                } else
                {
                    totalPrice += basePrice - ((supply - demand) * (basePrice * 0.05));
                }

                demand--;
                supply++;

                growing = false;
            }
        }
    }

    public void OnSellClick()
    {
        if(!growing)
        {
            PlayerScript.Money += totalPrice;
            totalPrice = 0;

            supply = 0;
        }
    }
}
