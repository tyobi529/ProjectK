using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{

    CardGenerator cardgenerator;

    //Button DecisionButton;

    public bool isselect = false;

    public int power = 0;
    public int magic = 0;
    public int defence = 0;
    public int speed = 0;

    public int cost = 0;

    public int special = 0;

    public int skill = 0;


    //// Start is called before the first frame update
    void Start()
    {
        cardgenerator = GameObject.Find("CardGenerator").GetComponent<CardGenerator>();
    }



    public void OnCard()
    {
        //Debug.Log("OnCard");

        //すでに選択されていたらキャンセル
        if (isselect)
        {
            cardgenerator.cardcount--;
            this.transform.Translate(0f, -100f, 0f);
            isselect = false;
        }
        //カード選択
        else
        {
            if (cardgenerator.cardcount < 3)
            {
                this.transform.Translate(0f, 100f, 0f);
                cardgenerator.cardcount++;

                isselect = true;
            }
        }

    }



}
