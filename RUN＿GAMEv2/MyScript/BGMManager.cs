using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;
    [SerializeField] private AudioSource BGMaudioSource;
    [Tooltip("0:�^�C�g��,�e�X�g�@�P�F�Z���N�g�@�Q�F�P�ʁ@�R�F�Q�ʁ@�S�F�R��")]
    [SerializeField] private AudioClip[] BGMlist;


    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
    }

    public void BGMPlay(int readSceneBGMNum)
    {
        //GameManager����ǂ�Scene���󂯎��
        BGMaudioSource.clip = BGMlist[readSceneBGMNum];

        //�ŏ�����BGM��炷
        BGMaudioSource.Play();
    }

    private void Update()
    {

    }

    public void BGMStop()
    {
        BGMaudioSource.Stop();
    }

}
