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
    public TextMeshProUGUI amount;
    public TextMeshProUGUI sellPrice;
    public TextMeshProUGUI sellPricePerGram;
    public TextMeshProUGUI playerMoney;

    public TextMeshProUGUI supplyText;
    public TextMeshProUGUI demandText;
    public TextMeshProUGUI moreDemandText;

    public int supply;
    public int demand;
    public double basePrice;
    public double pricePerGram;
    public int moreDemandTimer;

    bool growing = false;
    private void Start()
    {
        plant[0] = Resources.Load<Sprite>("Pot2");
        plant[1] = Resources.Load<Sprite>("Pot3");
        plant[2] = Resources.Load<Sprite>("Pot4");
        plant[3] = Resources.Load<Sprite>("Pot5");
        plant[4] = Resources.Load<Sprite>("Pot6");

        supply = 0;
        demand = 10;
        basePrice = 10;
        moreDemandTimer = 180;

        supplyText.text = "Supply: " + supply.ToString();
        demandText.text = "Demand: " + demand.ToString();

        StartCoroutine(MoreDemand());
    }

    private void Update()
    {
        supplyText.text = "Supply: " + supply.ToString();
        demandText.text = "Demand: " + demand.ToString();
        moreDemandText.text = "More Demand in: " + moreDemandTimer.ToString() + "s";

        playerMoney.text = "$ " + PlayerScript.Money.ToString();
        amount.text = PlantScript.HarvestAmount.ToString() + "g";
        sellPrice.text = "$" + PlantScript.SellPrice.ToString();

        if (demand > supply)
        {
            pricePerGram = basePrice + ((demand - supply) * (basePrice * 0.05));
            
        }
        else if (demand == supply)
        {
            pricePerGram = basePrice;
        }
        else
        {
            pricePerGram = basePrice - ((supply - demand) * (basePrice * 0.05));
        }

        sellPricePerGram.text = "($" + pricePerGram.ToString() + " / g)";
    }

    IEnumerator MoreDemand()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);

            moreDemandTimer--;

            if(moreDemandTimer <= 0)
            {
                moreDemandTimer = 180;
                demand += 10;
            }
        }
        
    }

    public void OnPlantCLick()
    {
        if(!growing)
        {
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

                PlantScript.HarvestAmount++;

                if(demand > supply)
                {
                    PlantScript.SellPrice += basePrice + ((demand - supply) * (basePrice * 0.05));
                } else if(demand == supply)
                {
                    PlantScript.SellPrice += basePrice;
                } else
                {
                    PlantScript.SellPrice += basePrice - ((supply - demand) * (basePrice * 0.05));
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
            PlayerScript.Money += PlantScript.SellPrice;
            PlantScript.HarvestAmount = 0;
            PlantScript.SellPrice = 0;

            supply = 0;
        }
    }
}
