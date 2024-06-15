using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    MainGameManager gM;
    [SerializeField] private AudioSource SEaudioSource;
    [Tooltip("0:UI�̃{�^���@�P:�N���X�^���@2:�N���X�^�����@3�F�|�[�Y�@4�F���[�v�@5�F�{�^���@6�F�S�[��")]
    [SerializeField] private AudioClip[] SElist;
    // Start is called before the first frame update
    void Start()
    {
        gM = GetComponent<MainGameManager>();
    }
    public void SEPlay(int SEnumber)
    {
        //����ꂽ�ԍ���SE��炷
        SEaudioSource.PlayOneShot(SElist[SEnumber]);
            
    }

}
