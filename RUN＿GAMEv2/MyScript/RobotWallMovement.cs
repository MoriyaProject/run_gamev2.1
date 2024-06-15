using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotWallMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    [SerializeField] Vector3 pointA;
    [SerializeField] Vector3 pointB;
    [SerializeField] bool    isZMove;
    private bool isReachToA;
    private GameObject player;
    private Vector3 playerdestination;
    [SerializeField] private float robotMoveSpeed = 5f;
    [SerializeField] private float playerChaceRange = 0.1f;
    [SerializeField] private float destChangeRange = 0.05f;
    private bool  isTracking;

    private AudioSource robotAS;
    [SerializeField] AudioClip robotAlart;       //magic
    [SerializeField] private bool isSEStop = false;
    //できるなら本家robotmovementと統合したい

    private void StopChace()
    {
        isTracking = false;
        gM.ForcusedActivate(isTracking);
        robotAS.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
        robotAS = this.gameObject.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerPos = gM.SendPlayerPosition();
        //isZMoveが有効なら移動をZ方向のみに絞る、そうでないならX方向のみ
        if (isZMove == true)
        {
            playerdestination = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, playerPos.z);
        }
        else
        {
            playerdestination = new Vector3(playerPos.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }
        
        //プレイヤーが触れたら
        if(isTracking == true)
        {
            //プレイヤーを追跡
            transform.position = Vector3.MoveTowards(transform.position, playerdestination, robotMoveSpeed * Time.deltaTime);
            //音を鳴らす
            if(robotAS.isPlaying == false)
            {
                robotAS.PlayOneShot(robotAlart);
            }

            //プレイヤーが遠くなったら追跡終了
            if (Vector3.Distance(this.gameObject.transform.position, playerdestination) > playerChaceRange)
            {
                StopChace();
            }

            //移動限界に来たら追跡終了
            if (Vector3.Distance(this.gameObject.transform.position, pointB) < destChangeRange)
            {
                isReachToA = false;
                StopChace();
            }
            if (Vector3.Distance(this.gameObject.transform.position, pointA) < destChangeRange)
            {
                isReachToA = true;
                StopChace();
            }

        }
        else
        {
            //目標地点を交互に移動
            if (isReachToA == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointB, robotMoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, pointA, robotMoveSpeed * Time.deltaTime);
            }

            //両端についたら方向転換
            if (Vector3.Distance(this.gameObject.transform.position, pointB) < destChangeRange)
            {
                isReachToA = false;
            }
            if (Vector3.Distance(this.gameObject.transform.position, pointA) < destChangeRange)
            {
                isReachToA = true;
            }
        }



    }

    void Update()
    {
        //ゴールorゲームオーバーで止まる
        isSEStop = gM.RobotSEStop();
        if(isSEStop == true)
        {
            robotAS.Stop();
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player発見");
            //Playerが触れたら追跡開始
            isTracking = true;
            gM.ForcusedActivate(isTracking);

        }
    }

}



