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

        //プレイヤーが下に落ちたらゼロ地点に戻す
        if (this.gameObject.transform.position.y < fallRange)  
        {
            this.gameObject.transform.position = new Vector3(0, 0, 0);
        }

        //リフトに乗ったらリフトの加速度を受け取る
        if(isOnRift == true)
        {
            Vector3 riftAccel = riftMovement.GiveAccelToPlayer();
            transform.position += riftAccel * Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //リフトに乗ったらそのリフトに知らせる
        if (other.CompareTag("Rift"))
        {
            Debug.Log("Playerが乗った");
            isOnRift = true;
            other.transform.parent.gameObject.GetComponent<RiftMovement>().isRide = true;
            riftMovement = other.transform.parent.gameObject.GetComponent<RiftMovement>();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Rift"))
        {
            Debug.Log("Playerが降りた");
            isOnRift = false;
            other.transform.parent.gameObject.GetComponent<RiftMovement>().isRide = false;
            //fix
        }
    }


}
