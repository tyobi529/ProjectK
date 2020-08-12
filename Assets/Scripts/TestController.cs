using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            Debug.Log("aa");
            this.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
