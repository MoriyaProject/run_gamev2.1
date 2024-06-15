using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrystalMoement : MonoBehaviour
{
    GameObject CryOb;
    Transform  CryTf;
    [SerializeField] private bool isGold;
    [SerializeField] private float rotateSpeed = 120f;
    [SerializeField] private int thisPoit;

    GameObject gMobj;
    MainGameManager gM;

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
 
        //自身のobjectの取得
        CryOb = this.gameObject;
        CryTf = CryOb.transform;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //1秒にn度回転
        CryTf.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);  
    }

    void OnTriggerEnter(Collider other)
    {
        
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("Player"))
        {
            Debug.Log("クリスタルに触れた"); 

           //種類の判別
            if(isGold == true)  
            {
                thisPoit = 1000;
            }
            else
            {
                thisPoit = 100;
            }

            //点数をGMに送って消える
            gM.ScoreManager(thisPoit, isGold);   

            Destroy(this.gameObject);

        }
    }

}
