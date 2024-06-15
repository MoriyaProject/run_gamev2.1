using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAdditionnalMovement : MonoBehaviour
{
    GameObject      gMobj;
    MainGameManager gM;
    private bool    isOnRift;
    RiftMovement    riftMovement;

    private float   fallRange = -5.0f;
    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = this.gameObject.transform.position;
        gM.ReadPlayerPosition(playerPos);

        //�v���C���[�����ɗ�������[���n�_�ɖ߂�
        if (this.gameObject.transform.position.y < fallRange)  
        {
            this.gameObject.transform.position = new Vector3(0, 0, 0);
        }

        //���t�g�ɏ�����烊�t�g�̉����x���󂯎��
        if(isOnRift == true)
        {
            Vector3 riftAccel = riftMovement.GiveAccelToPlayer();
            transform.position += riftAccel * Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //���t�g�ɏ�����炻�̃��t�g�ɒm�点��
        if (other.CompareTag("Rift"))
        {
            Debug.Log("Player�������");
            isOnRift = true;
            other.transform.parent.gameObject.GetComponent<RiftMovement>().isRide = true;
            riftMovement = other.transform.parent.gameObject.GetComponent<RiftMovement>();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Rift"))
        {
            Debug.Log("Player���~�肽");
            isOnRift = false;
            other.transform.parent.gameObject.GetComponent<RiftMovement>().isRide = false;
            //fix
        }
    }


}
