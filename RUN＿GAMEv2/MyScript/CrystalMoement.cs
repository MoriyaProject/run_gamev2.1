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
 
        //���g��object�̎擾
        CryOb = this.gameObject;
        CryTf = CryOb.transform;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //1�b��n�x��]
        CryTf.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);  
    }

    void OnTriggerEnter(Collider other)
    {
        
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("Player"))
        {
            Debug.Log("�N���X�^���ɐG�ꂽ"); 

           //��ނ̔���
            if(isGold == true)  
            {
                thisPoit = 1000;
            }
            else
            {
                thisPoit = 100;
            }

            //�_����GM�ɑ����ď�����
            gM.ScoreManager(thisPoit, isGold);   

            Destroy(this.gameObject);

        }
    }

}
