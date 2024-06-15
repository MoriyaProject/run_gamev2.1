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

    //GotoNextpoint��S�̂ŋ��L������

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // �n�_���Ȃɂ��ݒ肳��Ă��Ȃ��Ƃ��ɕԂ�
        if (destPoints.Length == 0)
            return;

        // �z����̎��̈ʒu��ڕW�n�_�ɐݒ肵�A�K�v�Ȃ�Ώo���n�_�ɖ߂�
        destPoint = (destPoint + 1) % destPoints.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�ڕW�Ɍ������Đi��
        Vector3 vec = (destPoints[destPoint] - transform.position).normalized;
        transform.position += vec * riftMoveSpeed * Time.deltaTime;

        //�ڕW���߂��Ȃ�����A�ڕW��ς���
        if (Vector3.Distance(this.gameObject.transform.position, destPoints[destPoint]) < destChangeRange)
        
        {
            transform.position = destPoints[destPoint];
            GotoNextPoint();
        }
    }

    //player�Ƀ��t�g�̉����x��^����
    public Vector3 GiveAccelToPlayer()
    {
        Vector3 riftVec;
        riftVec = (destPoints[destPoint] - transform.position).normalized;
        return riftVec * riftMoveSpeed;

    }

}
