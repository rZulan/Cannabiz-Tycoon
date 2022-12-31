using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public Image image;
    public Sprite plant1;
    public Sprite plant2;
    public Sprite plant3;
    public Sprite plant4;
    public Sprite plant5;
    public Image progress;
    int _amount;
    int _sellPrice;
    int _playerMoney = 0;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI sellPrice;
    public TextMeshProUGUI playerMoney;


    public void OnPlantCLick()
    {
        if (image.sprite == plant1)
        {
            image.sprite = plant2;
            progress.rectTransform.localScale = new Vector3((float)0.25, 1);
        } else if(image.sprite == plant2)
        {
            image.sprite = plant3;
            progress.rectTransform.localScale = new Vector3((float)0.50, 1);
        } else if (image.sprite == plant3)
        {
            image.sprite = plant4;
            progress.rectTransform.localScale = new Vector3((float)0.75, 1);
        } else if (image.sprite == plant4)
        {
            image.sprite = plant5;
            progress.rectTransform.localScale = new Vector3((float)1.00, 1);
        } else if (image.sprite == plant5)
        {
            image.sprite = plant1;
            progress.rectTransform.localScale = new Vector3((float)0, 1);

            _amount++;
            _sellPrice = _amount * 10;
            amount.text = _amount.ToString() + "g";
            sellPrice.text = "$ " + _sellPrice.ToString();
        }
    }

    public void OnSellClick()
    {
        _playerMoney += _sellPrice;

        _amount = 0;
        _sellPrice = 0;

        playerMoney.text = "$ " + _playerMoney.ToString();
        amount.text = _amount.ToString() + "g";
        sellPrice.text = "$" + _sellPrice.ToString();
    }
}