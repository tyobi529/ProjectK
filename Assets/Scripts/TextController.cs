using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public Text[] HPText = new Text[2];
    public Text[] PWText = new Text[2];
    public Text[] MGText = new Text[2];

    public Text phase2Text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void TextUpdate(int id, int[] hp, int[] power, int[] magic)
    {
        if (id == 1)
        {

            HPText[0].text = hp[0].ToString();
            PWText[0].text = power[0].ToString();
            MGText[0].text = magic[0].ToString();

            HPText[1].text = hp[1].ToString();
            PWText[1].text = power[1].ToString();
            MGText[1].text = magic[1].ToString();


        }

        if (id == 2)
        {
            HPText[0].text = hp[1].ToString();
            PWText[0].text = power[1].ToString();
            MGText[0].text = magic[1].ToString();

            HPText[1].text = hp[0].ToString();
            PWText[1].text = power[0].ToString();
            MGText[1].text = magic[0].ToString();
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

