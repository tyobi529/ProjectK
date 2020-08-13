﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardGenerator : MonoBehaviour
{

    //GameController gamecontroller;

    //public int[] card_num = new int[5];

    public GameObject[] CardPrefab = new GameObject[5];

    public GameObject[] GeneratedCard = new GameObject[5];

    //public int[] selectedcard = new int[3];
    List<int> selectedcard = new List<int>();

    //３枚まで
    public int cardcount = -1;


    public GameObject DecisionButton;

    private void Start()
    {
        //gamecontroller = GameObject.Find("GameController(Clone)").GetComponent<GameController>();
        //DecisionButton.SetActive(false);
      
    }

    private void Update()
    {
        //Debug.Log(cardcount);
        if (cardcount == 3)
        {
            DecisionButton.SetActive(true);
        }
        else if (0 <= cardcount && cardcount < 3)
        {
            DecisionButton.SetActive(false);

        }


    }




    //カードをランダムに５枚生成する
    public void CardGenerate()
    {
        for (int i = 0; i < 5; i++)
        {
            //０：物理
            //１：魔法
            //２：HP
            //３：火炎斬り
            //４：メラ

            int value = 0;

            if (i < 3)
                value = Random.Range(0, 2);
            else
                value = Random.Range(0, 5);

            //card_num[i] = value;

            //Debug.Log(value);

            //GameObject card = Instantiate(CardPrefab[value]) as GameObject;
            GeneratedCard[i] = Instantiate(CardPrefab[value]) as GameObject;

            //card.transform.position = new Vector3(-2.2f + 1.1f * i, -3f, 0f);
            GeneratedCard[i].transform.position = new Vector3(-2.2f + 1.1f * i, -3f, 0f);

        }
    }






}
