using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportarMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    private bool isPlayer;   //magic
    [SerializeField] private bool isTransporting;     //magic

    [Tooltip("0:セレクト 1:1面 2:2面 3:3面")]
    [SerializeField] private string selectStageName;    //magic

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
    }

    //プレイヤーに触れられたとき、自分に振られた番号をGMに送る
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            isTransporting = true;
            Debug.Log("Playerに触れている:" + selectStageName);
            gM.ReadTransStatus(isTransporting, selectStageName);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            isTransporting = false;
            Debug.Log("Playerは離れた");
            gM.ReadTransStatus(isTransporting, null);
        }
    }

}
