using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public Text[] HPText = new Text[2];
    public Text[] PWText = new Text[2];
    public Text[] MGText = new Text[2];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void TextUpdate(int[] hp, int[] power, int[] magic)
    {
        for (int i = 0; i < 2; i++)
        {
            HPText[i].text = hp[i].ToString();
            PWText[i].text = power[i].ToString();
            MGText[i].text = magic[i].ToString();

        }

    }
}

