using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks
{
    CardController cardcontroller;


    public int[] hp = new int[2];
    public int[] power = new int[2];
    public int[] magic = new int[2];




   





    int select = 0;
    //int select_2 = 0;




    public int id;

    //常に表示
    private GameObject PowerButton;
    private GameObject MagicButton;
    private GameObject SupportButton;


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
    int phase = 1;


    Methods methods;

    private GameObject TextController;

    private void Start()
    {
        id = PhotonNetwork.LocalPlayer.ActorNumber;

        cardcontroller = GameObject.Find("CardController").GetComponent<CardController>();

        cardcontroller.CardGenerate();



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

        if (phase == 1)
        {
            //カードの配置
            //ボタンの表示
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