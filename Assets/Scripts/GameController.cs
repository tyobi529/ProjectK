using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks
{
    CardGenerator cardgenerator;

    List<int> selectedcard = new List<int>();

    //int[] selected = new int[3];

    GameObject DecisionButton;


    public int[] hp = new int[2] { 500, 500 };
    public int[] power = new int[2] { 100, 100 };
    public int[] magic = new int[2] { 100, 100 };





    int select = 0;
    //int select_2 = 0;




    public int id;

    //常に表示
    //private GameObject PowerButton;
    //private GameObject MagicButton;
    //private GameObject SupportButton;


    private GameObject[] PowerAction = new GameObject[3];
    private GameObject[] MagicAction = new GameObject[3];
    private GameObject[] SupportAction = new GameObject[3];



    //１：プレイヤー１攻撃
    //２：プレイヤー２攻撃
    public int attack_id = 1;

    int turncount = 1;

    //選択が終わるとtrue
    bool[] isselect = new bool[2] { false, false };

    //１：育成
    //２：攻防
    int phase;


    Methods methods;

    //テキスト更新用
    private TextController textcontroller;

    private void Start()
    {
        id = PhotonNetwork.LocalPlayer.ActorNumber;

        cardgenerator = GameObject.Find("CardGenerator").GetComponent<CardGenerator>();


        DecisionButton = GameObject.Find("DecisionButton");

        DecisionButton.GetComponent<Button>().onClick.AddListener(OnDecisionButton);
        DecisionButton.SetActive(false);


        textcontroller = GameObject.Find("TextController").GetComponent<TextController>();



        //ゲームスタート
        //カード表示
        phase = 1;
        cardgenerator.CardGenerate();
        cardgenerator.cardcount = 0;

        //テキスト表示オブジェクト
        //TextController = GameObject.Find("TextController");


        //DecisionButton.SetActive(false);


        //AttackButtonText = GameObject.Find("AttackButtonText").GetComponent<Text>();
        //DispatchButtonText = GameObject.Find("DispatchButtonText").GetComponent<Text>();


        //if (turn == playerId)
        //{
        //    AttackButtonText.text = "こうげきする";
        //    DispatchButtonText.text = "派遣する";
        //}
        //else
        //{
        //    AttackButtonText.text = "ぼうぎょする";
        //    DispatchButtonText.text = "待機する";
        //}




        //if (playerId == 1)
        //{
        //    TextController.GetComponent<TextController>().TextUpdate(hp_1, hp_2);
        //}

        //else if (playerId == 2)
        //{
        //    TextController.GetComponent<TextController>().TextUpdate(hp_2, hp_1);

        //}

    }


    private void Update()
    {

        if (phase == 2)
        {
            //カードの配置
            //ボタンの表示

            foreach(var a in selectedcard)
            {
                Debug.Log(a);

            }

            phase = 1;
        }




    }


    public void OnDecisionButton()
    {
        for (int i = 0; i < 5; i++)
        {

            if (cardgenerator.GeneratedCard[i].GetComponent<CardController>().isselect)
            {
                Debug.Log(i);

                if (cardgenerator.GeneratedCard[i].tag == "HP")
                {
                    selectedcard.Add(0);
                }

                if (cardgenerator.GeneratedCard[i].tag == "PW")
                {
                    selectedcard.Add(1);
                }

                if (cardgenerator.GeneratedCard[i].tag == "MG")
                {
                    selectedcard.Add(2);
                }

            }

            Destroy(cardgenerator.GeneratedCard[i]);
        }




        //ステータスアップ
        photonView.RPC(nameof(ReinforceStatus), RpcTarget.All, id);

        selectedcard.Clear();




        textcontroller.TextUpdate(hp, power, magic);

        phase = 2;


    }


    [PunRPC]
    private void ReinforceStatus(int id)
    {

        for (int i = 0; i < 3; i++)
        {

            if (selectedcard[i] == 0)
            {
                hp[id - 1] += 50;
            }

            if (selectedcard[i] == 1)
            {
                power[id - 1] += 20;
            }

            if (selectedcard[i] == 2)
            {
                magic[id - 1] += 20;
            }
        }
        
    }



    //turnendを送る
    [PunRPC]
    private void TurnEnd(int id)
    {
        if (id == 1)
        {
            isselect[0] = true;
        }
        else if (id == 2)
        {
            isselect[1] = true;
        }
    }

    //次のturnへ
    private void TurnChange()
    {
        //if (turn == 1)
        //{
        //    turn = 2;
        //    //Debug.Log("turn" + turn);

        //}
        //else if (turn == 2)
        //{
        //    turn = 1;
        //}




    }

    //ダメージ関数
    private void Damage(ref int hp, int mon)
    {
        hp -= mon*10;
    }



    
    


}