using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    //GameController gamecontroller;

    //public int[] card_num = new int[5];

    public GameObject[] CardPrefab = new GameObject[3];



    //カードをランダムに５枚生成する
    public void CardGenerate()
    {
        for (int i = 0; i < 5; i++)
        {
            //０：HP
            //１：物理
            //２：魔法
            int value = Random.Range(0, 2);

            //card_num[i] = value;

            GameObject card = Instantiate(CardPrefab[value]) as GameObject;
            card.transform.position = new Vector3(-2.2f + 1.1f * i, -3f, 0f);
        }
    }
}
