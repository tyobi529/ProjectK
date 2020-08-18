using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    //public Text[] HPText = new Text[2];
    //public Text[] PWText = new Text[2];
    //public Text[] MGText = new Text[2];

    public Text[] HPText = new Text[2];
    public Text[] MPText = new Text[2];
    public Text[] AttackText = new Text[2];
    public Text[] DefenceText = new Text[2];
    public Text[] SpeedText = new Text[2];
    public Text[] DeathCountText = new Text[2];



    public Text phase2Text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void TextUpdate(int id, int[] hp, int[] mp, int[] attack, int[] defence, int[] speed, int[] deathcount)
    {
        if (id == 1)
        {


            HPText[0].text = hp[0].ToString();
            MPText[0].text = mp[0].ToString();
            AttackText[0].text = attack[0].ToString();
            DefenceText[0].text = defence[0].ToString();
            SpeedText[0].text = speed[0].ToString();
            DeathCountText[0].text = deathcount[0].ToString();


            //Debug.Log("aaa");

            HPText[1].text = hp[1].ToString();
            MPText[1].text = mp[1].ToString();
            AttackText[1].text = attack[1].ToString();
            DefenceText[1].text = defence[1].ToString();
            SpeedText[1].text = speed[1].ToString();
            DeathCountText[1].text = deathcount[1].ToString();




        }

        if (id == 2)
        {
            HPText[0].text = hp[1].ToString();
            MPText[0].text = mp[1].ToString();
            AttackText[0].text = attack[1].ToString();
            DefenceText[0].text = defence[1].ToString();
            SpeedText[0].text = speed[1].ToString();
            DeathCountText[0].text = deathcount[1].ToString();


            HPText[1].text = hp[0].ToString();
            MPText[1].text = mp[0].ToString();
            AttackText[1].text = attack[0].ToString();
            DefenceText[1].text = defence[0].ToString();
            SpeedText[1].text = speed[0].ToString();
            DeathCountText[1].text = deathcount[0].ToString();

        }


    }

    public void Phase2Update(int id, int attack_id)
    {
        if (id == attack_id)
        {
            phase2Text.text = "攻撃ターンです。";
        }
        else
        {
            phase2Text.text = "防御ターンです。";
        }
    }
}

