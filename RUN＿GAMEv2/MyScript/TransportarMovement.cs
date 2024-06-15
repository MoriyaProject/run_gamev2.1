using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportarMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    private bool isPlayer;   //magic
    [SerializeField] private bool isTransporting;     //magic

    [Tooltip("0:�Z���N�g 1:1�� 2:2�� 3:3��")]
    [SerializeField] private string selectStageName;    //magic

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
    }

    //�v���C���[�ɐG���ꂽ�Ƃ��A�����ɐU��ꂽ�ԍ���GM�ɑ���
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            isTransporting = true;
            Debug.Log("Player�ɐG��Ă���:" + selectStageName);
            gM.ReadTransStatus(isTransporting, selectStageName);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            isTransporting = false;
            Debug.Log("Player�͗��ꂽ");
            gM.ReadTransStatus(isTransporting, null);
        }
    }

}
