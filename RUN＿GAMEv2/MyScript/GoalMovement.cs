using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMovement : MonoBehaviour
{

    //���ꂮ�炢���������ĂȂ���GM�ɓ������Ă�������

    GameObject gMobj;
    MainGameManager gM;
    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
    }


    void OnTriggerEnter(Collider other)
    {

        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�GM�ɃS�[�������Ɠ`����
        if (other.CompareTag("Player"))
        {
            Debug.Log("�S�[���ɐG�ꂽ");
            gM.DoGoal();
            
        }
    }

}
