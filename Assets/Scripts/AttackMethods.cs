using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMethods : MonoBehaviour
{

    public void NormalPowerAttack(int Id, ref int[] hp, int[] power, int[] magic, int[] defence, int[] speed)
    {
        Debug.Log("通常物理攻撃");

        int damage = 0;

        if (Id == 1)
        {
            //回避率
            //int avoid = (speed[0] - speed[1]) / 2;
            //if (avoid > 50)
            //    avoid = 50;

            //if (avoid >= Random.Range(1, 101)) 
            //{
            //    Debug.Log("攻撃を回避した！！");
            //    return;
            //}

            damage = power[0] * power[0] / defence[1];
            hp[1] -= damage;


        }

        else if (Id == 2)
        {
            //回避率
            //int avoid = (speed[1] - speed[0]) / 2;
            //if (avoid > 50)
            //    avoid = 50;

            //if (avoid >= Random.Range(1, 101))
            //{
            //    Debug.Log("攻撃を回避した！！");
            //    return;
            //}


            damage = power[1] * power[1] / defence[0];
            hp[0] -= damage;
        }

        if (Id == 1)
            Debug.Log("プレイヤー２に" + damage + "ダメージを与えた。");
        else if (Id == 2)
            Debug.Log("プレイヤー１に" + damage + "ダメージを与えた。");

    }

    public void NormalMagicAttack(int Id, ref int[] hp, int[] power, int[] magic, int[] defence, int[] speed)
    {
        Debug.Log("通常魔法攻撃");

        int damage = 0;

        if (Id == 1)
        {
            //回避率
            //int avoid = (speed[0] - speed[1]) / 2;
            //if (avoid > 50)
            //    avoid = 50;

            //if (avoid >= Random.Range(1, 101))
            //{
            //    Debug.Log("攻撃を回避した！！");
            //    return;
            //}

            damage = magic[0] * magic[0] / defence[1];
            hp[1] -= damage;


        }

        else if (Id == 2)
        {

            //回避率
            //int avoid = (speed[1] - speed[0]) / 2;
            //if (avoid > 50)
            //    avoid = 50;

            //if (avoid >= Random.Range(1, 101))
            //{
            //    Debug.Log("攻撃を回避した！！");
            //    return;
            //}

            damage = magic[1] * magic[1] / defence[0];
            hp[0] -= damage;
        }

        if (Id == 1)
            Debug.Log("プレイヤー２に" + damage + "ダメージを与えた。");
        else if (Id == 2)
            Debug.Log("プレイヤー１に" + damage + "ダメージを与えた。");
    }

    //public void SkillAttacck(int Id, ref int[] hp, int[] power, int[] magic, int[] defence, int[] speed, int Skill)
    //{
    //    if (Skill == 1)
    //    {

    //    }
    //}



    //特技系

    //各種パラメータで計算する
    public void ParameterAttack(int Id, ref int[] hp, int[] Para, int[] defence, int[] speed)
    {
        Debug.Log("特技を使った！");



        int damage = 0;

        if (Id == 1)
        {
            //回避率
            //int avoid = (speed[0] - speed[1]) / 2;
            //if (avoid > 50)
            //    avoid = 50;

            //if (avoid >= Random.Range(1, 101))
            //{
            //    Debug.Log("攻撃を回避した！！");
            //    return;
            //}

            damage = Para[0] * Para[0] / defence[1];

            //damage *= 2;

            hp[1] -= damage;

        }

        else if (Id == 2)
        {
            //回避率
            //int avoid = (speed[1] - speed[0]) / 2;
            //if (avoid > 50)
            //    avoid = 50;

            //if (avoid >= Random.Range(1, 101))
            //{
            //    Debug.Log("攻撃を回避した！！");
            //    return;
            //}


            damage = Para[1] * Para[1] / defence[0];

            //damage *= 2;

            hp[0] -= damage;
        }

        if (Id == 1)
            Debug.Log("プレイヤー２に" + damage + "ダメージを与えた。");
        else if (Id == 2)
            Debug.Log("プレイヤー１に" + damage + "ダメージを与えた。");
    }


    public void DeadlyAttack(int Id, ref int[] hp, int[] power, int[] magic, int[] defence)
    {
        int damage = 0;

        if (Id == 1)
        {
            damage = power[0] * power[0] / defence[1] + magic[0] * magic[0] / defence[1];


            hp[1] -= damage;


        }

        else if (Id == 2)
        {
            damage = power[1] * power[1] / defence[0] + magic[1] * magic[1] / defence[0];


            hp[0] -= damage;
        }

        Debug.Log("必殺");
        if (Id == 1)
            Debug.Log("プレイヤー２に" + damage + "ダメージを与えた。");
        else if (Id == 2)
            Debug.Log("プレイヤー１に" + damage + "ダメージを与えた。");
    }
}
