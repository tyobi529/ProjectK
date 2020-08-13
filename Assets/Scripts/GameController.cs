using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks, IPunObservable
{
    CardGenerator cardgenerator;

    //List<int> selectedcard = new List<int>();

    int[] selectedcard = new int[3];

    GameObject DecisionButton;


    //０：プレイヤー１
    //１：プレイヤー２
    public int[] hp = new int[2] { 500, 500 };
    public int[] power = new int[2] { 100, 100 };
    public int[] magic = new int[2] { 100, 100 };

    //選択した時にtrue
    bool[] turnend = new bool[2] { false, false };

    //攻撃した時にtrue
    bool[] isattack = new bool[2] { false, false };

    //攻防の選択
    //１：物理
    //２：魔法
    //３：必殺
    int[] select = new int[2] {0, 0};




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
    int attack_id = -1;

    int turncount = 1;

    //選択が終わるとtrue
    bool[] isselect = new bool[2] { false, false };

    //１：育成
    //２：攻防
    int phase;


    Methods methods;

    //テキスト更新用
    private TextController textcontroller;

    GameObject PowerButton;
    GameObject MagicButton;
    GameObject DeadlyButton;

    private void Start()
    {
        id = PhotonNetwork.LocalPlayer.ActorNumber;

        cardgenerator = GameObject.Find("CardGenerator").GetComponent<CardGenerator>();


        DecisionButton = GameObject.Find("DecisionButton");
        PowerButton = GameObject.Find("PowerButton");
        MagicButton = GameObject.Find("MagicButton");

        DecisionButton.GetComponent<Button>().onClick.AddListener(OnDecisionButton);
        PowerButton.GetComponent<Button>().onClick.AddListener(OnPowerButton);
        MagicButton.GetComponent<Button>().onClick.AddListener(OnMagicButton);

        DecisionButton.SetActive(false);
        PowerButton.SetActive(false);
        MagicButton.SetActive(false);



        textcontroller = GameObject.Find("TextController").GetComponent<TextController>();



        phase = 1;

        //ゲームスタート
        StartPhase1();



    }


    private void Update()
    {
        //GameController生成側から実行する
        if (photonView.IsMine)
        {
            //どちらも選択終了
            if (turnend[0] && turnend[1])
            {
                //育成フェーズ
                if (phase == 1)
                {
                    //テキスト更新
                    photonView.RPC(nameof(TextUpdate), RpcTarget.All);

                    //先行、後攻決定
                    attack_id = Random.Range(1, 3);
                    //attack_id = 2;

                    Debug.Log("player" + attack_id + "の攻撃");

                    //turnendをfalseに
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 1);
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 2);


                    //ボタン表示
                    photonView.RPC(nameof(ShowButton), RpcTarget.All);

                    //次フェーズへ
                    photonView.RPC(nameof(ChangePhase), RpcTarget.All);

                }


                //攻防フェーズ
                else if (phase == 2)
                {

                    //予測が外れる
                    //攻撃成功
                    if (select[0] != select[1])
                    {
                        photonView.RPC(nameof(Damage), RpcTarget.All, attack_id, select[attack_id - 1]);


                    }

                    //turnendをfalseに
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 1);
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 2);

                    //テキスト更新
                    photonView.RPC(nameof(TextUpdate), RpcTarget.All);

                    //攻撃済にする
                    isattack[attack_id - 1] = true;


                    if (isattack[0] && isattack[1])
                    {
                        //次のターンへの準備
                        photonView.RPC(nameof(ChangePhase), RpcTarget.All);
                        isattack[0] = false;
                        isattack[1] = false;


                        //フェーズ１の開始
                        photonView.RPC(nameof(StartPhase1), RpcTarget.All);

                    }
                    //攻守交代
                    else
                    {
                        if (attack_id == 1)
                        {
                            attack_id = 2;
                        }
                        else if (attack_id == 2)
                        {
                            attack_id = 1;
                        }

                        photonView.RPC(nameof(ShowButton), RpcTarget.All);
                    }
                }

            }


        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 2; i++)
            {
                Debug.Log("hp" + (i + 1) + "：　" + hp[i]);
                Debug.Log("pw" + (i + 1) + "：　" + power[i]);
                Debug.Log("mg" + (i + 1) + "：　" + magic[i]);
            }
            
        }




    }


    public void OnDecisionButton()
    {
        if (phase == 1)
        {
            int j = 0;

            for (int i = 0; i < 5; i++)
            {

                if (cardgenerator.GeneratedCard[i].GetComponent<CardController>().isselect)
                {
                    //Debug.Log(i);

                    if (cardgenerator.GeneratedCard[i].tag == "HP")
                    {
                        //selectedcard.Add(0);
                        selectedcard[j] = 0;
                        j++;
                    }

                    if (cardgenerator.GeneratedCard[i].tag == "PW")
                    {
                        //selectedcard.Add(1);
                        selectedcard[j] = 1;
                        j++;
                    }

                    if (cardgenerator.GeneratedCard[i].tag == "MG")
                    {
                        //selectedcard.Add(2);
                        selectedcard[j] = 2;
                        j++;
                    }

                }

                Destroy(cardgenerator.GeneratedCard[i]);

            }

            photonView.RPC(nameof(ReinforceStatus), RpcTarget.All, id, selectedcard);


            //DecisionButton.SetActive(false);
        }

        //ターンエンドを伝える
        photonView.RPC(nameof(SendTurnend), RpcTarget.All, id);

        HideButton();
    }

    //ボタンのオンオフ
    [PunRPC]
    private void ShowButton()
    {
        PowerButton.SetActive(true);
        MagicButton.SetActive(true);
        DecisionButton.SetActive(true);

        PowerButton.GetComponent<Image>().color = Color.white;
        MagicButton.GetComponent<Image>().color = Color.white;
        DecisionButton.GetComponent<Image>().color = Color.white;

    }

    [PunRPC]
    private void HideButton()
    {
        PowerButton.SetActive(false);
        MagicButton.SetActive(false);
        DecisionButton.SetActive(false);

        PowerButton.GetComponent<Image>().color = Color.white;
        MagicButton.GetComponent<Image>().color = Color.white;
        DecisionButton.GetComponent<Image>().color = Color.white;


    }

    public void OnPowerButton()
    {
        photonView.RPC(nameof(SendSelect), RpcTarget.All, id, 1);

        PowerButton.GetComponent<Image>().color = Color.yellow;

    }

    public void OnMagicButton()
    {

        photonView.RPC(nameof(SendSelect), RpcTarget.All, id, 2);

        MagicButton.GetComponent<Image>().color = Color.yellow;


    }

    //ターン終了情報更新
    [PunRPC]
    private void SendTurnend(int id)
    {
        turnend[id - 1] = !turnend[id - 1];
        
    }

    //パラメータテキスト更新
    [PunRPC]
    private void TextUpdate()
    {
        textcontroller.TextUpdate(id, hp, power, magic);
    }



    //ステータスを強化
    [PunRPC]
    private void ReinforceStatus(int id, int[] card)
    {
        //Debug.Log("dd");

        for (int i = 0; i < 3; i++)
        {

            if (card[i] == 0)
            {
                hp[id - 1] += 50;
            }

            if (card[i] == 1)
            {
                power[id - 1] += 20;
            }

            if (card[i] == 2)
            {
                magic[id - 1] += 20;
            }
        }

        //Debug.Log("cc");


        //初期化
        //selectedcard.Clear();



    }

    //育成フェーズを始める
    [PunRPC]
    private void StartPhase1()
    {
        //カード表示
        cardgenerator.CardGenerate();
        cardgenerator.cardcount = 0;

        //決定ボタン表示
        DecisionButton.SetActive(true);
    }



    //攻防フェーズへ
    [PunRPC]
    private void ChangePhase()
    {
        if (phase == 1)
        {
            phase = 2;
        }
        else if (phase == 2)
        {
            phase = 1;
        }
    }

    //選択を共有
    [PunRPC]
    private void SendSelect(int id, int num)
    {
        select[id - 1] = num;
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
    [PunRPC]
    private void Damage(int id, int select)
    {
        int damage = 0;

        if (select == 1)
        {
            damage = power[id - 1];
        }
        else if (select == 2)
        {
            damage = magic[id - 1];
        }
        else if (select == 3)
        {
            damage = power[id - 1] + magic[id - 1];
        }

        if (id == 1)
            hp[1] -= damage;
        else if (id == 2)
            hp[0] -= damage;

        Debug.Log("プレイヤー" + (id - 1) + "に" + damage + "のダメージを与えた");
    }


    // データを送受信するメソッド
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(attack_id);
        }
        else
        {
            attack_id = (int)stream.ReceiveNext();
        }
    }






}