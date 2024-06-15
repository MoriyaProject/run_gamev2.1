using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    MainGameManager gM;
    [SerializeField] private AudioSource SEaudioSource;
    [Tooltip("0:UIのボタン　１:クリスタル　2:クリスタル金　3：ポーズ　4：ワープ　5：ボタン　6：ゴール")]
    [SerializeField] private AudioClip[] SElist;
    // Start is called before the first frame update
    void Start()
    {
        gM = GetComponent<MainGameManager>();
    }
    public void SEPlay(int SEnumber)
    {
        //送られた番号のSEを鳴らす
        SEaudioSource.PlayOneShot(SElist[SEnumber]);
            
    }

}
