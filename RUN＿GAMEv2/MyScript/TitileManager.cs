using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitileManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Space�������ꂽ��Z���N�g�V�[����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SelectScene");
        }


    }
}
