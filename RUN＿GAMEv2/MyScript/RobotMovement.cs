using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//�I�u�W�F�N�g��NavMeshAgent�R���|�[�l���g��ݒu
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

        // autoBraking �𖳌��ɂ���ƁA�ڕW�n�_�̊Ԃ��p���I�Ɉړ�����
        //�G�[�W�F���g�͖ڕW�n�_�ɋ߂Â��Ă����x�𗎂Ƃ��Ȃ�
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        robotAS = this.GetComponent<AudioSource>();

        GotoNextPoint();

    }

    void GotoNextPoint()
    {
        // �n�_���Ȃɂ��ݒ肳��Ă��Ȃ��Ƃ��ɕԂ�
        if (destPoints.Length == 0)
            return;

        //�G�[�W�F���g�����ݐݒ肳�ꂽ�ڕW�n�_�ɍs���悤�ɐݒ�
        agent.destination = destPoints[destPoint];

        // �z����̎��̈ʒu��ڕW�n�_�ɐݒ肵�A�K�v�Ȃ�Ώo���n�_�ɖ߂�
        destPoint = (destPoint + 1) % destPoints.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerPos = gM.SendPlayerPosition();
        //Player�Ƃ��̃I�u�W�F�N�g�̋����𑪂�
        distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, playerPos);
        
        if (isTracking == true)
        {
            //�ǐՂ̎��AquitRange��苗�������ꂽ�璆�~
            if (distanceToPlayer > quitRange)
            {
                isTracking = false;
                gM.ForcusedActivate(isTracking);
                
            }

            //Player��ڕW�Ƃ���
            agent.destination = playerPos;

            //bikkuri��\��
            bikkuriBall.GetComponent<MeshRenderer>().enabled = true;
            bikkuriPoal.GetComponent<MeshRenderer>().enabled = true;

            //����炷
            if (robotAS.isPlaying == false)
            {
                robotAS.PlayOneShot(robotAlart);
            }

        }
        else
        {
            //����������
            //bikkuri������
            bikkuriBall.GetComponent<MeshRenderer>().enabled = false;
            bikkuriPoal.GetComponent<MeshRenderer>().enabled = false;
            //�����~�߂�
            robotAS.Stop();

            // �G�[�W�F���g�����ڕW�n�_�ɋ߂Â��Ă�����A���̖ڕW�n�_��I��
            if (!agent.pathPending && agent.remainingDistance < destChangeRange)
            {
                GotoNextPoint();
            }
               
        }

    }

    void Update()
    {
        //�S�[��or�Q�[���I�[�o�[�Ŏ~�܂�
        isSEStop = gM.RobotSEStop();
        if (isSEStop == true)
        {
            robotAS.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player����");
            //Player���G�ꂽ��ǐՊJ�n
            isTracking = true;
            gM.ForcusedActivate(isTracking);

        }
    }
}
