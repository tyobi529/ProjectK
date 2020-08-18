using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMethods : MonoBehaviour
{

    public void TemporaryPowerUp(ref int attack)
    {
        attack *= 2;

        Debug.Log("このターンステータスが倍になった！");
    }

    public void Healing(ref int Hp)
    {
        Debug.Log("HPが200回復した！！");
        Hp += 200;
    }
}
