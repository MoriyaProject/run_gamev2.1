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
    //�ł���Ȃ�{��robotmovement�Ɠ���������

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
        //isZMove���L���Ȃ�ړ���Z�����݂̂ɍi��A�����łȂ��Ȃ�X�����̂�
        if (isZMove == true)
        {
            playerdestination = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, playerPos.z);
        }
        else
        {
            playerdestination = new Vector3(playerPos.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }
        
        //�v���C���[���G�ꂽ��
        if(isTracking == true)
        {
            //�v���C���[��ǐ�
            transform.position = Vector3.MoveTowards(transform.position, playerdestination, robotMoveSpeed * Time.deltaTime);
            //����炷
            if(robotAS.isPlaying == false)
            {
                robotAS.PlayOneShot(robotAlart);
            }

            //�v���C���[�������Ȃ�����ǐՏI��
            if (Vector3.Distance(this.gameObject.transform.position, playerdestination) > playerChaceRange)
            {
                StopChace();
            }

            //�ړ����E�ɗ�����ǐՏI��
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
            //�ڕW�n�_�����݂Ɉړ�
            if (isReachToA == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointB, robotMoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, pointA, robotMoveSpeed * Time.deltaTime);
            }

            //���[�ɂ���������]��
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
        //�S�[��or�Q�[���I�[�o�[�Ŏ~�܂�
        isSEStop = gM.RobotSEStop();
        if(isSEStop == true)
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



