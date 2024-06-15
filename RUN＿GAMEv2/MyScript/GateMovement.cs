using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    [SerializeField] GameObject gate;
    [SerializeField] private float gateY     = 5f;
    [SerializeField] private float gateSpeed = 2f;
    private float gateDest;

    GameObject bottunbody;
    [SerializeField] private float bottonY     = 0.2f;
    [SerializeField] private float bottonSpeed = 0.5f;
    private float       bottonDest;
    private AudioSource bottonAS;
    private bool        isPush;

    [SerializeField] AudioClip pushed;

    // Start is called before the first frame update
    void Start()
    {
        bottonAS   =  this.gameObject.GetComponent<AudioSource>();
        bottunbody =  transform.GetChild(0).gameObject;

        //�Q�[�g�A�{�^���̓��B�_�����߂�
        gateDest   = gate.transform.position.y + gateY;
        bottonDest = bottunbody.transform.position.y - bottonY;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�{�^���������ꂽ��Q�[�g�A�{�^�����Ɉړ�
        if(isPush == true)
        {
            if(gate.transform.position.y < gateDest)
            {
                gate.transform.position += new Vector3(0, gateSpeed * Time.deltaTime, 0);
            }
            
            if(bottunbody.transform.position.y > bottonDest)
            {
                bottunbody.transform.position -= new Vector3(0, bottonSpeed * Time.deltaTime, 0);
            }

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("Player"))
        {
            Debug.Log("�{�^���������ꂽ");
            bottunbody.GetComponent<Renderer>().material.color =  Color.green;
            gate.GetComponent<Renderer>().material.color       =  Color.green;
            if (bottonAS.isPlaying == false && isPush == false)
            {
                bottonAS.PlayOneShot(pushed);
            }
            isPush = true;

        }
    }

}
