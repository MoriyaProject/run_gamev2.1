using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//オブジェクトにNavMeshAgentコンポーネントを設置
[RequireComponent(typeof(NavMeshAgent))]

public class RobotMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    public   Vector3[]     destPoints;
    private  int           destPoint = 0;
    private  NavMeshAgent  agent;
    private  float         distanceToPlayer;

    [SerializeField] private float trackingRange   = 3f;
    [SerializeField] private float quitRange       = 5f;
    [SerializeField] private float destChangeRange = 0.5f;
    [SerializeField] private float robotMoveSpeed  = 3f;
    [SerializeField] private bool  isTracking      = false;

    [SerializeField] GameObject bikkuriBall;
    [SerializeField] GameObject bikkuriPoal;

    private AudioSource robotAS;
    [SerializeField] AudioClip robotAlart;
    private bool isSEStop;

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();

        robotAS = this.gameObject.GetComponent<AudioSource>();

        // autoBraking を無効にすると、目標地点の間を継続的に移動する
        //エージェントは目標地点に近づいても速度を落とさない
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        robotAS = this.GetComponent<AudioSource>();

        GotoNextPoint();

    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返す
        if (destPoints.Length == 0)
            return;

        //エージェントが現在設定された目標地点に行くように設定
        agent.destination = destPoints[destPoint];

        // 配列内の次の位置を目標地点に設定し、必要ならば出発地点に戻る
        destPoint = (destPoint + 1) % destPoints.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerPos = gM.SendPlayerPosition();
        //Playerとこのオブジェクトの距離を測る
        distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, playerPos);
        
        if (isTracking == true)
        {
            //追跡の時、quitRangeより距離が離れたら中止
            if (distanceToPlayer > quitRange)
            {
                isTracking = false;
                gM.ForcusedActivate(isTracking);
                
            }

            //Playerを目標とする
            agent.destination = playerPos;

            //bikkuriを表示
            bikkuriBall.GetComponent<MeshRenderer>().enabled = true;
            bikkuriPoal.GetComponent<MeshRenderer>().enabled = true;

            //音を鳴らす
            if (robotAS.isPlaying == false)
            {
                robotAS.PlayOneShot(robotAlart);
            }

        }
        else
        {
            //未発見時は
            //bikkuriを消去
            bikkuriBall.GetComponent<MeshRenderer>().enabled = false;
            bikkuriPoal.GetComponent<MeshRenderer>().enabled = false;
            //音を止める
            robotAS.Stop();

            // エージェントが現目標地点に近づいてきたら、次の目標地点を選択
            if (!agent.pathPending && agent.remainingDistance < destChangeRange)
            {
                GotoNextPoint();
            }
               
        }

    }

    void Update()
    {
        //ゴールorゲームオーバーで止まる
        isSEStop = gM.RobotSEStop();
        if (isSEStop == true)
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
