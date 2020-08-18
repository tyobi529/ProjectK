using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardGenerator : MonoBehaviour
{

    //GameController gamecontroller;

    //public int[] card_num = new int[5];

    //public GameObject[] CardPrefab = new GameObject[5];
    public GameObject canvas;

    public GameObject CardPrefab;
    public GameObject[] GeneratedCard = new GameObject[5];

    //public int[] selectedcard = new int[3];
    List<int> selectedcard = new List<int>();

    //３枚まで
    public int cardcount = -1;


    public GameObject DecisionButton;

    public int parameter_card_num = 4;

    // 付くステータスの下限、上限
    public int status_low_num = 2;
    public int status_up_num = 4;

    public int status_low_value = 5;
    public int status_up_value = 15;

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
        for (int i = 0; i < parameter_card_num; i++)
        {
            //Debug.Log(i + "枚目");
            int status_count = 0;

            //ステータスの種類
            List<int> status_num = new List<int>();

            //選んだステータス
            List<int> status_select = new List<int>();

            //最初のカードはPWとMG
            // if (i == 0)
            // {
            //     status_count = 2;

            //     status_select.Add(0);
            //     status_select.Add(1);
            // }
            // else
            // {
            //付くステータスの数（２〜４）
            status_count = Random.Range(status_low_num, status_up_num + 1);

            for (int j = 0; j < 4; j++)
            {
                status_num.Add(j);
            }

            //ステータスをstatus_count分選ぶ
            for (int j = 0; j < status_count; j++)
            {
                //Debug.Log(status_num.Count);
                int index = Random.Range(0, status_num.Count);

                //Debug.Log(index);

                status_select.Add(status_num[index]);

                //Debug.Log(status_num[index]);

                status_num.RemoveAt(index);

                //foreach(var a in status_num)
                //{
                //    Debug.Log(a);
                //}
            }
            // }


            status_select.Sort();

            //カード生成
            GeneratedCard[i] = Instantiate(CardPrefab) as GameObject;
            GeneratedCard[i].transform.position = new Vector3(-292f + 146f * i, 300f, 0f);

            GeneratedCard[i].transform.SetParent(canvas.transform, false);


            CardController cardcontroller = GeneratedCard[i].GetComponent<CardController>();


            //０：PW
            //１：MG
            //２：DE
            //３：SP
            for (int j = 0; j < status_select.Count; j++)
            {
                int value = 0;

                //１枚目はステータス高めに
                // if (i == 0)
                // {
                //     value = Random.Range(10, 21);
                // }
                // else
                // {
                //５〜２０の中でパラメータを決定する。
                //value = Random.Range(5, 21);
                value = Random.Range(status_low_value, status_up_value);

                // }

                //foreach(var a in status_select)
                //{
                //    Debug.Log(a);
                //}

                //Debug.Log("status" + );
                //MP
                if (status_select[j] == 0)
                {
                    cardcontroller.power = value;

                    GeneratedCard[i].transform.GetChild(j).GetComponent<Text>().text = "PW: " + cardcontroller.power;
                    GeneratedCard[i].transform.GetChild(j).GetComponent<Text>().color = new Color(245f / 255f, 47f / 255f, 47f / 255f, 1f);


                }
                //AT
                else if (status_select[j] == 1)
                {
                    cardcontroller.magic = value;

                    GeneratedCard[i].transform.GetChild(j).GetComponent<Text>().text = "MG: " + cardcontroller.magic;
                    GeneratedCard[i].transform.GetChild(j).GetComponent<Text>().color = new Color(235f / 255f, 47f / 255f, 245f / 255f, 1f);

                }
                //DE
                else if (status_select[j] == 2)
                {
                    cardcontroller.defence = value;

                    GeneratedCard[i].transform.GetChild(j).GetComponent<Text>().text = "DE: " + cardcontroller.defence;
                    GeneratedCard[i].transform.GetChild(j).GetComponent<Text>().color = new Color(60f / 255f, 53f / 255f, 231f / 255f, 1f);

                }
                //SP
                else if (status_select[j] == 3)
                {
                    cardcontroller.speed = value;

                    GeneratedCard[i].transform.GetChild(j).GetComponent<Text>().text = "SP: " + cardcontroller.speed;
                    GeneratedCard[i].transform.GetChild(j).GetComponent<Text>().color = new Color(27f / 255f, 144f / 255f, 53f / 255f, 1f);

                }
            }



            int sum = cardcontroller.power + cardcontroller.magic + cardcontroller.defence + cardcontroller.speed;


            if (sum < 10)
            {
                cardcontroller.cost = 3;
            }
            else if (sum < 20)
            {
                cardcontroller.cost = 2;
            }
            else if (sum < 30)
            {
                cardcontroller.cost = 1;
            }
            else if (sum < 40)
            {
                cardcontroller.cost = 0;
            }
            else
            {
                cardcontroller.cost = 0;
            }



            GeneratedCard[i].transform.GetChild(4).GetComponent<Text>().text = "コス+" + cardcontroller.cost;
            GeneratedCard[i].transform.GetChild(4).GetComponent<Text>().color = new Color(217f / 255f, 17f / 255f, 169f / 255f, 1f);


        }

        //特殊カード
        for (int i = parameter_card_num; i < 5; i++)
        {

            int a = Random.Range(1, 3);

            //カード生成
            GeneratedCard[i] = Instantiate(CardPrefab) as GameObject;
            GeneratedCard[i].transform.position = new Vector3(-292f + 146f * i, 300f, 0f);
            GeneratedCard[i].transform.SetParent(canvas.transform, false);
            CardController cardcontroller = GeneratedCard[i].GetComponent<CardController>();



            //特殊カード
            if (a == 1)
            {
                cardcontroller.special = Random.Range(1, 6);
                //cardcontroller.special = 5;

                GeneratedCard[i].transform.GetChild(0).GetComponent<Text>().text = "特殊" + cardcontroller.special;

            }


            //特技カード
            if (a == 2)
            {
                cardcontroller.skill = Random.Range(1, 5);
                GeneratedCard[i].transform.GetChild(0).GetComponent<Text>().text = "特技" + cardcontroller.skill;

            }








            cardcontroller.cost = 0;


            GeneratedCard[i].transform.GetChild(4).GetComponent<Text>().text = "コス+" + cardcontroller.cost;
            GeneratedCard[i].transform.GetChild(4).GetComponent<Text>().color = new Color(217f / 255f, 17f / 255f, 169f / 255f, 1f);


        }
    }






}
