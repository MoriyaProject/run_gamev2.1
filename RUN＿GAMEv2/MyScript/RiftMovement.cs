using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    public bool isRide = false;
    private int destPoint = 0;
    [SerializeField] private float     destChangeRange = 0.1f;
    [SerializeField] private Vector3[] destPoints;
    [SerializeField] private float     riftMoveSpeed = 3f;

    //GotoNextpointを全体で共有したい

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返す
        if (destPoints.Length == 0)
            return;

        // 配列内の次の位置を目標地点に設定し、必要ならば出発地点に戻る
        destPoint = (destPoint + 1) % destPoints.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //目標に向かって進む
        Vector3 vec = (destPoints[destPoint] - transform.position).normalized;
        transform.position += vec * riftMoveSpeed * Time.deltaTime;

        //目標が近くなったら、目標を変える
        if (Vector3.Distance(this.gameObject.transform.position, destPoints[destPoint]) < destChangeRange)
        
        {
            transform.position = destPoints[destPoint];
            GotoNextPoint();
        }
    }

    //playerにリフトの加速度を与える
    public Vector3 GiveAccelToPlayer()
    {
        Vector3 riftVec;
        riftVec = (destPoints[destPoint] - transform.position).normalized;
        return riftVec * riftMoveSpeed;

    }

}
