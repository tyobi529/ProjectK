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
    List<int>[] special = new List<int>[2];

    //選択した時にtrue
    bool[] turnend = new bool[2] { false, false };

    //攻撃した時にtrue
    bool[] isattack = new bool[2] { false, false };

    //攻防の選択
    //１：物理
    //２：魔法
    //３：必殺
    int[] select = new int[2] {0, 0};


    //必殺カウント
    int[] deathcount = new int[2] { 0, 0 };




    public int id;




    //１：プレイヤー１攻撃
    //２：プレイヤー２攻撃
    int attack_id = -1;

    int turncount = 1;

    //選択が終わるとtrue
    bool[] isselect = new bool[2] { false, false };

    //１：育成
    //２：攻防
    int phase;

    //フェーズ終了時にtrue
    bool endphase1 = false;
    bool endphase2 = false;

    //フェーズ開始時にtrue
    bool startphase1 = false;
    bool startphase2 = false;

    //trueでゲームスタート
    bool gamestart = false;


    Methods methods;

    //テキスト更新用
    private TextController textcontroller;

    GameObject PowerButton;
    GameObject MagicButton;
    GameObject DeadlyButton;

    GameObject Phase2Text;

    private void Start()
    {
        id = PhotonNetwork.LocalPlayer.ActorNumber;

        cardgenerator = GameObject.Find("CardGenerator").GetComponent<CardGenerator>();


        DecisionButton = GameObject.Find("DecisionButton");
        PowerButton = GameObject.Find("PowerButton");
        MagicButton = GameObject.Find("MagicButton");
        DeadlyButton = GameObject.Find("DeadlyButton");


        DecisionButton.GetComponent<Button>().onClick.AddListener(OnDecisionButton);
        PowerButton.GetComponent<Button>().onClick.AddListener(OnPowerButton);
        MagicButton.GetComponent<Button>().onClick.AddListener(OnMagicButton);
        DeadlyButton.GetComponent<Button>().onClick.AddListener(OnDeadlyButton);


        DecisionButton.SetActive(false);
        PowerButton.SetActive(false);
        MagicButton.SetActive(false);
        DeadlyButton.SetActive(false);




        textcontroller = GameObject.Find("TextController").GetComponent<TextController>();

        Phase2Text = GameObject.Find("Phase2Text");
        Phase2Text.SetActive(false);


        phase = 1;


        if (id == 2)
        {
            //プレイヤー１に参加を伝える
            photonView.RPC(nameof(Login2Player), RpcTarget.All);

        }


    }


    private void Update()
    {


        //プレイヤー１から実行する
        if (photonView.IsMine && gamestart)
        {

            if (phase == 1 && !startphase1)
            {
                //フェーズ１の開始
                photonView.RPC(nameof(StartPhase1), RpcTarget.All);
                startphase1 = true;
            }

            else if (phase == 1 && endphase1)
            {
                phase = 2;
            }

            else if (phase == 2 && !startphase2)
            {
                //フェーズ２の開始

                //先行、後攻決定
                attack_id = Random.Range(1, 3);

                //フェーズ２の開始
                photonView.RPC(nameof(StartPhase2), RpcTarget.All, attack_id);

                startphase2 = true;
            }

            else if (phase == 2 && endphase2)
            {
                phase = 1;

                //次のターンへ
                startphase1 = false;
                startphase2 = false;
                endphase1 = false;
                endphase2 = false;


                Phase2Text.SetActive(false);

                turncount++;
            }


            //どちらも選択終了
            if (turnend[0] && turnend[1])
            {
                //育成フェーズ
                if (phase == 1)
                {
                    //テキスト更新
                    photonView.RPC(nameof(TextUpdate), RpcTarget.All);


                    //turnendをfalseに
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 1);
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 2);

                    endphase1 = true;

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
                        isattack[0] = false;
                        isattack[1] = false;

                        endphase2 = true;

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

                        //攻守交代してもう一度フェーズ２
                        photonView.RPC(nameof(StartPhase2), RpcTarget.All, attack_id);
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

    [PunRPC]
    private void Login2Player()
    {
        gamestart = true;

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

                    if (cardgenerator.GeneratedCard[i].tag == "special1")
                    {
                        //selectedcard.Add(2);
                        selectedcard[j] = 3;
                        j++;
                    }

                    if (cardgenerator.GeneratedCard[i].tag == "special2")
                    {
                        //selectedcard.Add(2);
                        selectedcard[j] = 4;
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

    public void OnDeadlyButton()
    {

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

            //火炎斬り
            if (card[i] == 3)
            {
                special[id - 1].Add(1);
            }

            //メラ
            if (card[i] == 4)
            {
                special[id - 1].Add(2);
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
        Debug.Log("ターン：　" + turncount);

        //Debug.Log("")
        //カード表示
        cardgenerator.CardGenerate();
        cardgenerator.cardcount = 0;

        //決定ボタン表示
        DecisionButton.SetActive(true);
    }

    //攻防フェーズを始める
    [PunRPC]
    private void StartPhase2(int attack)
    {
        //ボタン表示
        ShowButton();

        textcontroller.Phase2Update(id, attack);

        Phase2Text.SetActive(true);


        if (deathcount[attack - 1] >= 3)
            DeadlyButton.SetActive(true);

        //特技
        //攻撃ターン
        if (attack == id)
        {

        }

    }



    //攻防フェーズへ
    //[PunRPC]
    //private void ChangePhase()
    //{
    //    if (phase == 1)
    //    {
    //        phase = 2;
    //    }
    //    else if (phase == 2)
    //    {
    //        phase = 1;
    //    }
    //}

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

        //乱数
        damage = damage * Random.Range(8, 13) / 10;

        if (id == 1)
        {
            hp[1] -= damage;
            deathcount[1]++;
            Debug.Log("プレイヤー２に" + damage + "のダメージを与えた");


        }
        else if (id == 2)
        {
            hp[0] -= damage;
            deathcount[1]++;
            Debug.Log("プレイヤー１に" + damage + "のダメージを与えた");


        }

    }


    // データを送受信するメソッド
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(attack_id);
            stream.SendNext(turncount);

            stream.SendNext(phase);



        }
        else
        {
            attack_id = (int)stream.ReceiveNext();
            turncount = (int)stream.ReceiveNext();

            phase = (int)stream.ReceiveNext();


        }
    }






}