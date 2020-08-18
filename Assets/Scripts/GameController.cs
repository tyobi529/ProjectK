using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks, IPunObservable
{
    CardGenerator cardgenerator;

    //List<int> selectedcard = new List<int>();

    //int[] selectedcard = new int[3];

    GameObject DecisionButton;

    //攻撃用の関数
    AttackMethods attackmethods;
    //特殊カード用の関数
    SpecialMethods specialmethods;



    //０：プレイヤー１
    //１：プレイヤー２
    //public int[] hp = new int[2] { 500, 500 };
    //public int[] power = new int[2] { 100, 100 };
    //public int[] magic = new int[2] { 100, 100 };
    //List<int>[] special = new List<int>[2];

    public int[] hp = new int[2] { 5000, 5000 };
    public int[] power = new int[2] { 100, 100 };
    public int[] magic = new int[2] { 100, 100 };
    public int[] defence = new int[2] { 100, 100 };
    public int[] speed = new int[2] { 100, 100 };

    public int guardcost = 5;





    //List<int> special = new List<int>();


    //必殺カウント
    int[] cost = new int[2] { 0, 0 };


    //0：Power
    //1：Magic
    //2：Defence
    //3：Speed
    //4：コスト
    //int[,] powerup = new int[2, 5];
    int[] powerup1 = new int[6];
    int[] powerup2 = new int[6];

    //特殊カード
    //List<int>[] special = new List<int>[2];
    //List<int> special1 = new List<int>();
    //List<int> special2 = new List<int>();
    //int[,] special = new int[2, 2];

    int[] special1 = new int[2];
    int[] special2 = new int[2];
    int[] skill1 = new int[2];
    int[] skill2 = new int[2];

    int[] savedstatus1 = new int[5];
    int[] savedstatus2 = new int[5];




    //選択した時にtrue
    bool[] turnend = new bool[2] { false, false };

    //攻撃した時にtrue
    bool[] isattack = new bool[2] { false, false };

    //攻撃が当たる場合はtrue
    bool[] ishit = new bool[2] { false, false };


    //１：物理攻撃
    //２：魔法攻撃
    //３：必殺
    //int[] attack_select = new int[2] { 0, 0 };
    //List<int> select1 = new List<int>();
    //List<int> select2 = new List<int>();

    //一時的に保存する用
    //int select_num = 0;
    //List<int> select_num = new List<int>();

    //物理を選択したらtrue
    bool[] select1 = new bool[2] { false, false };
    //魔法を選択したらtrue
    bool[] select2 = new bool[2] { false, false };
    //必殺を選択したらtrue
    bool[] select3 = new bool[2] { false, false };







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
    bool[] endphase1 = new bool[2] { false, false };
    bool[] endphase2 = new bool[2] { false, false };

    //フェーズ開始時にtrue
    bool startphase1 = false;
    bool startphase2 = false;

    //trueでゲームスタート
    bool gamestart = false;



    //テキスト更新用
    private TextController textcontroller;

    GameObject PowerButton;
    GameObject MagicButton;
    GameObject SkillButton;

    GameObject Phase2Text;

    private void Start()
    {
        id = PhotonNetwork.LocalPlayer.ActorNumber;

        cardgenerator = GameObject.Find("CardGenerator").GetComponent<CardGenerator>();


        DecisionButton = GameObject.Find("DecisionButton");
        PowerButton = GameObject.Find("PowerButton");
        MagicButton = GameObject.Find("MagicButton");
        SkillButton = GameObject.Find("SkillButton");


        DecisionButton.GetComponent<Button>().onClick.AddListener(OnDecisionButton);
        PowerButton.GetComponent<Button>().onClick.AddListener(OnPowerButton);
        MagicButton.GetComponent<Button>().onClick.AddListener(OnMagicButton);
        SkillButton.GetComponent<Button>().onClick.AddListener(OnSkillButton);

        DecisionButton.SetActive(false);
        PowerButton.SetActive(false);
        MagicButton.SetActive(false);
        SkillButton.SetActive(false);

        attackmethods = GameObject.Find("Methods").GetComponent<AttackMethods>();
        specialmethods = GameObject.Find("Methods").GetComponent<SpecialMethods>();



        textcontroller = GameObject.Find("TextController").GetComponent<TextController>();

        //Phase2Text = GameObject.Find("Phase2Text");
        //Phase2Text.SetActive(false);


        phase = 1;


        if (id == 2)
        {
            //プレイヤー１に参加を伝える
            photonView.RPC(nameof(Login2Player), RpcTarget.All);

        }

        // gameObject.GetComponent<Text>().text 

    }


    private void Update()
    {
        //テキスト更新
        TextUpdate();


        //プレイヤー１から実行する
        if (photonView.IsMine && gamestart)
        {

            if (phase == 1 && !startphase1)
            {
                //フェーズ１の開始
                photonView.RPC(nameof(StartPhase1), RpcTarget.All);
                startphase1 = true;
            }

            else if (phase == 1 && endphase1[0] && endphase1[1])
            {
                phase = 2;



            }

            else if (phase == 2 && !startphase2)
            {
                //フェーズ２の開始

                //先行、後攻決定
                //attack_id = Random.Range(1, 3);

                //先行決定
                if (speed[0] > speed[1])
                    attack_id = 1;
                else if (speed[0] < speed[1])
                    attack_id = 2;
                else
                    attack_id = Random.Range(1, 3);

                //攻撃が当たるかどうか
                //プレイヤー１から
                //回避率
                int avoid = 0;
                avoid = (speed[1] - speed[0]) / 2;

                if (avoid > 50)
                    avoid = 50;



                if (avoid < Random.Range(1, 101))
                {
                    ishit[0] = true;
                }

                //プレイヤー２から

                avoid = (speed[0] - speed[1]) / 2;

                if (avoid > 50)
                    avoid = 50;

                if (avoid < Random.Range(1, 101))
                {
                    ishit[1] = true;
                }

                photonView.RPC(nameof(SendAttackId), RpcTarget.All, attack_id);






                //フェーズ２の開始
                photonView.RPC(nameof(StartPhase2), RpcTarget.All);

                startphase2 = true;
            }

            else if (phase == 2 && endphase2[0] && endphase2[1])
            {

                //テキスト更新
                photonView.RPC(nameof(TextUpdate), RpcTarget.All);


                //フェーズ終了状態を戻す
                photonView.RPC(nameof(PhaseEndChange), RpcTarget.All, 1, 1);
                photonView.RPC(nameof(PhaseEndChange), RpcTarget.All, 1, 2);
                photonView.RPC(nameof(PhaseEndChange), RpcTarget.All, 2, 1);
                photonView.RPC(nameof(PhaseEndChange), RpcTarget.All, 2, 2);


                photonView.RPC(nameof(ToNextTurn), RpcTarget.All);





                //Phase2Text.SetActive(false);

            }


            //どちらも選択終了
            if (turnend[0] && turnend[1])
            {
                //育成フェーズ
                if (phase == 1)
                {

                    //ステータス強化
                    photonView.RPC(nameof(DoPhase1), RpcTarget.All);






                    //turnendをfalseに
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 1);
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 2);

                    //endphase1 = true;

                }


                //攻防フェーズ
                else if (phase == 2)
                {


                    //攻撃実行
                    photonView.RPC(nameof(DoPhase2), RpcTarget.All);


                    //turnendをfalseに
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 1);
                    photonView.RPC(nameof(SendTurnend), RpcTarget.All, 2);




                }

            }


        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 2; i++)
            {
                //Debug.Log("hp" + (i + 1) + "：　" + hp[i]);
                //Debug.Log("pw" + (i + 1) + "：　" + power[i]);
                //Debug.Log("mg" + (i + 1) + "：　" + magic[i]);

                //Debug.Log("プレイヤー" + (i + 1));
                //Debug.Log(hp[i]);
                //Debug.Log(power[i]);
                //Debug.Log(magic[i]);
                //Debug.Log(defence[i]);
                //Debug.Log(speed[i]);

                //Debug.Log(cost[i]);
                Debug.Log("special1" + special1[i]);
                Debug.Log("special2" + special2[i]);
                Debug.Log("skill1" + skill1[i]);
                Debug.Log("skill2" + skill2[i]);

            }

        }




    }

    [PunRPC]
    private void Login2Player()
    {
        gamestart = true;

    }

    //ボタンのオンオフ
    private void ShowButton()
    {
        PowerButton.SetActive(true);
        MagicButton.SetActive(true);

        if (attack_id != id)
            DecisionButton.SetActive(true);

        PowerButton.GetComponent<Image>().color = Color.white;
        MagicButton.GetComponent<Image>().color = Color.white;
        DecisionButton.GetComponent<Image>().color = Color.white;

        //if (cost[id - 1] >= 20)
        //{
        //    SkillButton.SetActive(true);
        //}

    }

    private void HideButton()
    {
        PowerButton.SetActive(false);
        MagicButton.SetActive(false);
        DecisionButton.SetActive(false);
        SkillButton.SetActive(false);


        PowerButton.GetComponent<Image>().color = Color.white;
        MagicButton.GetComponent<Image>().color = Color.white;
        DecisionButton.GetComponent<Image>().color = Color.white;
        SkillButton.GetComponent<Image>().color = Color.white;



    }


    public void OnDecisionButton()
    {
        if (phase == 1)
        {
            //決定ボタンが表示され続けないように
            cardgenerator.cardcount = -1;

            //初期化
            //if (id == 1)
            //{
            //    for (int i = 0; i < 6; i++)
            //    {
            //        powerup1[i] = 0;
            //    }
            //}
            //else if (id == 2)
            //{
            //    for (int i = 0; i < 6; i++)
            //    {
            //        powerup2[i] = 0;
            //    }

            //}

            for (int i = 0; i < 5; i++)
            {
                CardController cardcontroller = cardgenerator.GeneratedCard[i].GetComponent<CardController>();

                if (cardcontroller.isselect)
                {

                    if (id == 1)
                    {
                        powerup1[0] += cardcontroller.power;
                        powerup1[1] += cardcontroller.magic;
                        powerup1[2] += cardcontroller.defence;
                        powerup1[3] += cardcontroller.speed;
                        powerup1[4] += cardcontroller.cost;
                        powerup1[5] += cardcontroller.skill;


                        if (cardcontroller.special != 0)
                        {
                            for (int j = 0; j < special1.Length; j++)
                            {
                                if (special1[j] == 0)
                                {
                                    special1[j] = cardcontroller.special;
                                    break;
                                }
                            }
                        }

                        if (cardcontroller.skill != 0)
                        {
                            for (int j = 0; j < skill1.Length; j++)
                            {
                                if (skill1[j] == 0)
                                {
                                    skill1[j] = cardcontroller.skill;
                                    break;
                                }
                            }
                        }
                    }
                    else if (id == 2)
                    {
                        powerup2[0] += cardcontroller.power;
                        powerup2[1] += cardcontroller.magic;
                        powerup2[2] += cardcontroller.defence;
                        powerup2[3] += cardcontroller.speed;
                        powerup2[4] += cardcontroller.cost;
                        powerup2[5] += cardcontroller.skill;


                        if (cardcontroller.special != 0)
                        {
                            for (int j = 0; j < special1.Length; j++)
                            {
                                if (special2[j] == 0)
                                {
                                    special2[j] = cardcontroller.special;
                                    break;
                                }
                            }
                        }

                        if (cardcontroller.skill != 0)
                        {
                            for (int j = 0; j < skill1.Length; j++)
                            {
                                if (skill2[j] == 0)
                                {
                                    skill2[j] = cardcontroller.skill;
                                    break;
                                }
                            }
                        }
                    }

                }

                Destroy(cardgenerator.GeneratedCard[i]);

            }

            if (id == 1)
                photonView.RPC(nameof(ReinforceStatus), RpcTarget.All, id, powerup1, special1, skill1);
            else if (id == 2)
                photonView.RPC(nameof(ReinforceStatus), RpcTarget.All, id, powerup2, special2, skill2);




        }

        else if (phase == 2)
        {
            photonView.RPC(nameof(SendSelect), RpcTarget.All, id, select1[id - 1], select2[id - 1], select3[id - 1]);
            //select_num = 0;
         
        }

        //DecisionButton.SetActive(false);

        //ターンエンドを伝える
        photonView.RPC(nameof(SendTurnend), RpcTarget.All, id);

        HideButton();
    }



    public void OnPowerButton()
    {
        //攻撃側
        if (attack_id == id)
        {
            //if (select_num[0] != 1)
            //選ばれてない場合
            if (!select1[id - 1])
            {
                select1[id - 1] = true;
                select2[id - 1] = false;
                select3[id - 1] = false;


                PowerButton.GetComponent<Image>().color = Color.yellow;
                MagicButton.GetComponent<Image>().color = Color.white;
                SkillButton.GetComponent<Image>().color = Color.white;


                DecisionButton.SetActive(true);
            }

            //すでに選ばれている場合
            else
            {
                select1[id - 1] = false;

                PowerButton.GetComponent<Image>().color = Color.white;

                DecisionButton.SetActive(false);
            }
        }
        //防御側

        else
        {
            if (!select1[id - 1])
            {
                if (cost[id - 1] >= guardcost)
                {
                    cost[id - 1] -= guardcost;

                    PowerButton.GetComponent<Image>().color = Color.yellow;

                    select1[id - 1] = true;

                }



                //DecisionButton.SetActive(true);
            }
            else
            {
                cost[id - 1] += guardcost;

                PowerButton.GetComponent<Image>().color = Color.white;

                select1[id - 1] = false;

                //DecisionButton.SetActive(false);
            }
        }



    }

    public void OnMagicButton()
    {
        //攻撃側
        if (attack_id == id)
        {
            //if (select_num[0] != 1)
            //選ばれてない場合
            if (!select2[id - 1])
            {
                select1[id - 1] = false;
                select2[id - 1] = true;
                select3[id - 1] = false;


                PowerButton.GetComponent<Image>().color = Color.white;
                MagicButton.GetComponent<Image>().color = Color.yellow;
                SkillButton.GetComponent<Image>().color = Color.white;


                DecisionButton.SetActive(true);
            }

            //すでに選ばれている場合
            else
            {
                select2[id - 1] = false;

                MagicButton.GetComponent<Image>().color = Color.white;

                DecisionButton.SetActive(false);
            }
        }

        //防御側
        else
        {
            if (!select2[id - 1])
            {
                if (cost[id - 1] >= guardcost)
                {
                    cost[id - 1] -= guardcost;

                    MagicButton.GetComponent<Image>().color = Color.yellow;

                    select2[id - 1] = true;

                }



                //DecisionButton.SetActive(true);
            }
            else
            {
                cost[id - 1] += guardcost;

                MagicButton.GetComponent<Image>().color = Color.white;

                select2[id - 1] = false;

                //DecisionButton.SetActive(false);
            }
        }


    }

    //必殺技
    public void OnSkillButton()
    {
        //攻撃側
        if (attack_id == id)
        {
            //if (select_num[0] != 1)
            //選ばれてない場合
            if (!select3[id - 1])
            {
                select1[id - 1] = false;
                select2[id - 1] = false;
                select3[id - 1] = true;


                PowerButton.GetComponent<Image>().color = Color.white;
                MagicButton.GetComponent<Image>().color = Color.white;
                SkillButton.GetComponent<Image>().color = Color.yellow;


                DecisionButton.SetActive(true);
            }

            //すでに選ばれている場合
            else
            {
                select3[id - 1] = false;

                SkillButton.GetComponent<Image>().color = Color.white;

                DecisionButton.SetActive(false);
            }


        }

        //防御側
        else
        {
            if (!select3[id - 1])
            {
                if (cost[id - 1] >= guardcost)
                {
                    cost[id - 1] -= guardcost;

                    SkillButton.GetComponent<Image>().color = Color.yellow;

                    select3[id - 1] = true;

                }



                //DecisionButton.SetActive(true);
            }
            else
            {
                cost[id - 1] += guardcost;

                SkillButton.GetComponent<Image>().color = Color.white;

                select3[id - 1] = false;

                //DecisionButton.SetActive(false);
            }
        }

    }

    //ターン終了情報更新
    [PunRPC]
    private void SendTurnend(int id)
    {
        turnend[id - 1] = !turnend[id - 1];

    }

    //攻撃フェーズの選択を共有する
    [PunRPC]
    private void SendSelect(int Id, bool Select1, bool Select2, bool Select3)
    {
        select1[Id - 1] = Select1;
        select2[Id - 1] = Select2;
        select3[Id - 1] = Select3;

    }

    //フェーズの終了状態を反転する
    [PunRPC]
    private void PhaseEndChange(int Id, int phase_num)
    {
        if (phase_num == 1)
            endphase1[Id - 1] = !endphase1[Id - 1];
        else if (phase_num == 2)
            endphase2[Id - 1] = !endphase2[Id - 1];

    }

    //パラメータテキスト更新
    [PunRPC]
    private void TextUpdate()
    {
        textcontroller.TextUpdate(id, hp, power, magic, defence, speed, cost);
    }


    //ステータス強化量を通知
    [PunRPC]
    private void ReinforceStatus(int Id, int[] Powerup, int[] Special, int[] Skill)
    {


        if (Id == 1)
        {
            for (int i = 0; i < 6; i++)
            {
                powerup1[i] = Powerup[i];

            }

            //コストは10まで
            //if (powerup1[4] > 10)
            //{
            //    powerup1[4] = 10;
            //}

            special1 = Special;
            skill1 = Skill;
        }

        else if (Id == 2)
        {
            for (int i = 0; i < 6; i++)
            {
                powerup2[i] = Powerup[i];

            }

            //コストは10まで
            //if (powerup2[4] > 10)
            //{
            //    powerup2[4] = 10;
            //}

            special2 = Special;
            skill2 = Skill;
        }



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

    //育成フェーズの実行
    [PunRPC]
    private void DoPhase1()
    {
        //photonView.RPC(nameof(ReinforceStatus), RpcTarget.All, id, powerup);

        //まずお互いのステータスアップを反映する
        //プレイヤー1
        power[0] += powerup1[0];
        magic[0] += powerup1[1];
        defence[0] += powerup1[2];
        speed[0] += powerup1[3];
        cost[0] += powerup1[4];

        //コストは10まで
        if (cost[0] > 10)
            cost[0] = 10;

        //プレイヤー２
        power[1] += powerup2[0];
        magic[1] += powerup2[1];
        defence[1] += powerup2[2];
        speed[1] += powerup2[3];
        cost[1] += powerup2[4];

        if (cost[1] > 10)
            cost[1] = 10;


        //特殊カード用にステータスを保存する
        savedstatus1[0] = hp[0];
        savedstatus1[1] = power[0];
        savedstatus1[2] = magic[0];
        savedstatus1[3] = defence[0];
        savedstatus1[4] = speed[0];


        savedstatus2[0] = hp[1];
        savedstatus2[1] = power[1];
        savedstatus2[2] = magic[1];
        savedstatus2[3] = defence[1];
        savedstatus2[4] = speed[1];



        //特殊カードの処理
        //プレイヤー1
        foreach (int num in special1)
        {
            SpecialAction(1, num);
        }


        foreach (int num in special2)
        {
            SpecialAction(2, num);
        }


        //実行完了を通知
        photonView.RPC(nameof(PhaseEndChange), RpcTarget.All, id, 1);

    }

    private void SpecialAction(int Id, int num)
    {
        //１：このターン物理２倍
        //２：このターン魔法２倍
        //３：このターン防御２倍
        //４：このターン素早さ２倍
        //５：HP+200


        if (num == 1)
            specialmethods.TemporaryPowerUp(ref power[Id - 1]);

        else if (num == 2)
            specialmethods.TemporaryPowerUp(ref magic[Id - 1]);

        else if (num == 3)
            specialmethods.TemporaryPowerUp(ref defence[Id - 1]);

        else if (num == 4)
            specialmethods.TemporaryPowerUp(ref speed[Id - 1]);

        else if (num == 5)
            specialmethods.Healing(ref hp[Id - 1]);


    }

    //攻防フェーズを始める
    [PunRPC]
    private void StartPhase2()
    {
        Debug.Log("プレイヤー" + attack_id + "の攻撃");


        //ボタン表示
        ShowButton();

        if (attack_id == 1 && skill1[0] != 0)
        {
            SkillButton.SetActive(true);
            if (attack_id == id)
                SkillButton.transform.GetChild(0).GetComponent<Text>().text = "特技" + skill1[0].ToString();
            else
                SkillButton.transform.GetChild(0).GetComponent<Text>().text = "特技" + skill1[0].ToString() + "ガード";

        }
        else if (attack_id == 2 && skill2[0] != 0)
        {
            SkillButton.SetActive(true);

            if (attack_id == id)
                SkillButton.transform.GetChild(0).GetComponent<Text>().text = "特技" + skill2[0].ToString();
            else
                SkillButton.transform.GetChild(0).GetComponent<Text>().text = "特技" + skill2[0].ToString() + "ガード";
        }



        if (attack_id == id)
        {
            PowerButton.transform.GetChild(0).GetComponent<Text>().text = "物理攻撃";
            MagicButton.transform.GetChild(0).GetComponent<Text>().text = "魔法攻撃";

        }
        else
        {
            PowerButton.transform.GetChild(0).GetComponent<Text>().text = "物理ガード";
            MagicButton.transform.GetChild(0).GetComponent<Text>().text = "魔法ガード";

        }

        //Debug.Log("ccc");

        //if (attack_id == 1)


    }

    //攻防フェーズの実行
    [PunRPC]
    private void DoPhase2()
    {

        if (attack_id == 1)
        {

            if (select1[1])
                Debug.Log("プレイヤー２は物理攻撃をガードしている。");
            if (select2[1])
                Debug.Log("プレイヤー２は魔法攻撃をガードしている。");
            if (select3[1])
                Debug.Log("プレイヤー２は必殺技をガードしている。");


            if (select1[0] && !select1[1])
            {
                if (ishit[attack_id - 1])
                    Attack(attack_id, 1);
                else
                    Debug.Log("攻撃を回避した。");

            }

            else if (select2[0] && !select2[1])
            {
                if (ishit[attack_id - 1])
                     Attack(attack_id, 2);
                else
                    Debug.Log("攻撃を回避した。");

            }

            else if (select3[0] && !select3[1])
            {
                if (ishit[attack_id - 1])
                    Attack(attack_id, 3);
                else
                    Debug.Log("攻撃を回避した。");

            }
            else
            {
                Debug.Log("ガード成功");
            }

            //コストを同期
            if (id == 2)
                photonView.RPC(nameof(SendCost), RpcTarget.All, 2, cost[1]);

        }

        else if (attack_id == 2)
        {
            if (select1[0])
                Debug.Log("プレイヤー１は物理攻撃をガードしている。");
            if (select2[0])
                Debug.Log("プレイヤー１は魔法攻撃をガードしている。");
            if (select3[0])
                Debug.Log("プレイヤー１は必殺技をガードしている。");

            if (select1[1] && !select1[0])
            {
                if (ishit[attack_id - 1])
                    Attack(attack_id, 1);
                else
                    Debug.Log("攻撃を回避した。");


            }

            else if (select2[1] && !select2[0])
            {
                if (ishit[attack_id - 1])
                    Attack(attack_id, 2);
                else
                    Debug.Log("攻撃を回避した。");

            }

            else if (select3[1] && !select3[0])
            {
                if (ishit[attack_id - 1])
                    Attack(attack_id, 3);
                else
                    Debug.Log("攻撃を回避した。");

            }
            else
            {
                Debug.Log("ガード成功");
            }


            //コストを同期
            if (id == 1)
                photonView.RPC(nameof(SendCost), RpcTarget.All, 1, cost[0]);
        }

        isattack[attack_id - 1] = true;

        //選択を戻す
        for (int i = 0; i < 2; i++)
        {
            select1[i] = false;
            select2[i] = false;
            select3[i] = false;
        }

        //お互い攻撃終了
        if (isattack[0] && isattack[1])
        {
            photonView.RPC(nameof(PhaseEndChange), RpcTarget.All, id, 2);

            isattack[0] = false;
            isattack[1] = false;

        }
        //攻防交代でもう一度
        else
        {
            if (attack_id == 1)
                attack_id = 2;
            else if (attack_id == 2)
                attack_id = 1;

            StartPhase2();
        }





    }


    //次ターンへの処理
    [PunRPC]
    private void ToNextTurn()
    {


        //ステータスを戻す
        //hp[0] = savedstatus1[0];
        power[0] = savedstatus1[1];
        magic[0] = savedstatus1[2];
        defence[0] = savedstatus1[3];
        speed[0] = savedstatus1[4];

        //hp[1] = savedstatus2[0];
        power[1] = savedstatus2[1];
        magic[1] = savedstatus2[2];
        defence[1] = savedstatus2[3];
        speed[1] = savedstatus2[4];


        ishit[0] = false;
        ishit[1] = false;

        //初期化
        for (int i = 0; i < 6; i++)
        {
            powerup1[i] = 0;
            powerup2[i] = 0;

        }

        for (int i = 0; i < 2; i++)
        {
            special1[i] = 0;
            special2[i] = 0;
            skill1[i] = 0;
            skill2[i] = 0;
        }


        phase = 1;

        startphase1 = false;
        startphase2 = false;
        turncount++;

    }


    //先行を伝える
    [PunRPC]
    private void SendAttackId(int a)
    {

        attack_id = a;
    }

    //コストの同期をとる
    [PunRPC]
    private void SendCost(int Id, int Cost)
    {
        cost[Id - 1] = Cost;
    }





    //攻撃関数
    //攻撃する側のidと攻撃番号入力
    [PunRPC]
    private void Attack(int Id, int attack_number)
    {
        //Debug.Log("プレイヤー" + Id + "の攻撃");

        if (attack_number == 1)
            attackmethods.NormalPowerAttack(Id, ref hp, power, magic, defence, speed);

        else if (attack_number == 2)
            attackmethods.NormalMagicAttack(Id, ref hp, power, magic, defence, speed);


        //特技
        else if (attack_number == 3)
        {
            //attackmethods.SkillAttack(Id, ref hp, power, magic, defence);
            //cost[Id - 1] = 0;

            //if (skill1)
            if (Id == 1)
            {
                if (skill1[0] == 1)
                    attackmethods.ParameterAttack(Id, ref hp, power, defence, speed);
                else if (skill1[0] == 2)
                    attackmethods.ParameterAttack(Id, ref hp, magic, defence, speed);
                else if (skill1[0] == 3)
                    attackmethods.ParameterAttack(Id, ref hp, defence, defence, speed);
                else if (skill1[0] == 4)
                    attackmethods.ParameterAttack(Id, ref hp, speed, defence, speed);


            }
            else if (Id == 2)
            {
                if (skill2[0] == 1)
                    attackmethods.ParameterAttack(Id, ref hp, power, defence, speed);
                else if (skill2[0] == 2)
                    attackmethods.ParameterAttack(Id, ref hp, magic, defence, speed);
                else if (skill2[0] == 3)
                    attackmethods.ParameterAttack(Id, ref hp, defence, defence, speed);
                else if (skill2[0] == 4)
                    attackmethods.ParameterAttack(Id, ref hp, speed, defence, speed);
            }

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

            stream.SendNext(ishit[0]);
            stream.SendNext(ishit[1]);




        }
        else
        {
            attack_id = (int)stream.ReceiveNext();
            turncount = (int)stream.ReceiveNext();

            phase = (int)stream.ReceiveNext();

            ishit[0] = (bool)stream.ReceiveNext();
            ishit[1] = (bool)stream.ReceiveNext();


        }
    }






}