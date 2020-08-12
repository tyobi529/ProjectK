using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{

    CardGenerator cardgenerator;

    //Button DecisionButton;

    public bool isselect = false;

    //public bool cardnum = 0;

    //// Start is called before the first frame update
    void Start()
    {
        cardgenerator = GameObject.Find("CardGenerator").GetComponent<CardGenerator>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void OnMouseDown()
    {
        //Debug.Log("OnMouseDown");

        //すでに選択されていたらキャンセル
        if (isselect)
        {
            cardgenerator.cardcount--;
            this.transform.Translate(0f, -0.5f, 0f);
            isselect = false;
        }
        //カード選択
        else
        {
            if (cardgenerator.cardcount < 3)
            {
                this.transform.Translate(0f, 0.5f, 0f);
                cardgenerator.cardcount++;

                isselect = true;
            }
        }

    }

    

}
