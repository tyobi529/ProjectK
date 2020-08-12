using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Methods : MonoBehaviour
{

    //Text message;
    //TextController TextController;

    //GameController gameController;

    //private void Start()
    //{
    //    message = GameObject.Find("ShowTurnText").GetComponent<Text>();

    //    TextController = GameObject.Find("TextController").GetComponent<TextController>();

    //    gameController = this.gameObject.GetComponent<GameController>();
    //}

    ////プレイヤー１用の関数
    ////攻撃関数
    //public void Attack(ref int hp, int attack, int turn, int id)
    //{
    //    hp -= attack;
    //    Debug.Log("攻撃");

    //    if (turn == id)
    //    {
    //        message.text = attack + "のダメージを与えた！";
    //    }
    //    else
    //    {
    //        message.text = attack + "のダメージを受けた！";
    //    }

    //    //StartCoroutine(Timer());

    //    //message.text = attack + "テスト！";

    //}


    //public void Power_0(ref int hp, int power, int turn, int id)
    //{
    //    Attack(ref hp, power, turn, id);
    //}

    //public void Power_1(ref int hp, int power, int turn, int id)
    //{
    //    Attack(ref hp, power + 150, turn, id);
    //}

    //public void Magic_0(ref int hp, int magic, int turn, int id)
    //{
    //    Attack(ref hp, magic, turn, id);
    //}

    //public void Magic_1(ref int hp, int magic, int turn, int id)
    //{
    //    Attack(ref hp, magic*2, turn, id);
    //}


    ////補助特技
    //public void Support_0(ref int power, int turn, int id)
    //{
    //    power *= 2;

    //    if (turn == id)
    //    {
    //        message.text = "攻撃力が" + power + "に上がった！！";
    //    }
    //    else
    //    {
    //        message.text = "相手の攻撃力が" + power + "に上がった！！";
    //    }

    //    StartCoroutine(Timer());

    //}

    //public void Support_1(ref int magic, int turn, int id)
    //{
    //    magic *= 2;

    //    if (turn == id)
    //    {
    //        message.text = "魔力が" + magic + "に上がった！！";
    //    }
    //    else
    //    {
    //        message.text = "相手の魔力が" + magic + "に上がった！！";
    //    }

    //    StartCoroutine(Timer());

    //}

    ////予測成功時
    //public void CanExpect()
    //{
    //    message.text = "予測成功！";

    //    StartCoroutine(Timer());

    //}


    ////テキストを更新して一定時間待つ
    ////IEnumerator Timer()
    ////{
    ////    //TextController.GetComponent<TextController>().ShowTurn(playerId, turn, attack_num, defence_num);

    ////    //Debug.Log("3秒待ちます。");
    ////    //3秒待つ
    ////    yield return new WaitForSeconds(3);
    ////    //Debug.Log("3秒待ちました。");


    ////    TextController.ShowTurn(gameController.playerId, gameController.turn);
    ////}


}
