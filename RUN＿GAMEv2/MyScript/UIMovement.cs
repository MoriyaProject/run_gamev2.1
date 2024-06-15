using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class UIMovement : MonoBehaviour
{

    GameObject gMobj;
    MainGameManager gM;
    SaveData data;
 

    [SerializeField] GameObject pauseCanvasObj;
    [SerializeField] GameObject goalCanvasObj;
    [SerializeField] GameObject recordCanvasObj;
    [SerializeField] GameObject gameoverCanvasObj;
    [SerializeField] GameObject forcusedCanvusObj;
    [SerializeField] GameObject darkImageObj;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI clearTimeText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] Slider     HPMeter;
    [SerializeField] GameObject timeNewRecObj;
    [SerializeField] GameObject scoreNewRecObj;
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TextMeshProUGUI stageText;

    private RectTransform goalUITf;
    private Canvas pauseCanvas;
    private Canvas recordCanvas;

    private bool isFadeIn;
    private bool isFadeOut;
    private bool isForcused;
    private float darkAlpha = 1.0f;
    [SerializeField] private float darkAlphaSpeed = 0.001f;
    [SerializeField] private float goalDownSpeed = 10f;
    [SerializeField] GameObject[] disableUIList;

    private bool isGoal;

    //�|�[�Y��ʏ���
    public void PauseUIActivate(bool isPause, bool isGoal)
    {
        if (isPause == true && isGoal != true)
        {
            pauseCanvas.enabled = true;
        }
        else
        {
            pauseCanvas.enabled = false;
        }
    }

    //��ʈÓ]�����̌Ăяo��
    public void DoFadeOut(bool isReadFadeOut)
    {
        isFadeOut = isReadFadeOut;
    }

    //�픭������
    public void ForecusedUIActivate(bool isForcused)
    {
        if (isForcused == true)
        {
            forcusedCanvusObj.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            forcusedCanvusObj.GetComponent<Canvas>().enabled = false;
        }
    }

    //�S�[������
    public void GoalUIActivate(float currentGameTime, float currentScore, bool isBestTime, bool isBestScore)
    {
        isGoal = true;
        
        //�N���A���̃X�R�A��\��
        clearTimeText.text = currentGameTime.ToString();
        highScoreText.text = currentScore.ToString();

        //�x�X�g�X�R�A�Ȃ�m�点��
        if (isBestTime == true)
        {
            timeNewRecObj.GetComponent<TextMeshProUGUI>().enabled = true;
        }

        if (isBestScore == true)
        {
            scoreNewRecObj.GetComponent<TextMeshProUGUI>().enabled = true;
        }
        
    }

    //�Q�[���I�[�o�[��ʂ��o��
    public void GameOverUIActivate()
    {
        gameoverCanvasObj.GetComponent<Canvas>().enabled = true;
    }

    //�X�e�[�W�Z���N�g��UI�\��
    public void TransUIActivate(bool isTrans, string selectSceneName, float bestTime, float bestScore)
    {
        if(isTrans == true)
        {
            recordCanvas.enabled = true;
            stageText.text = selectSceneName;
            bestTimeText.text = bestTime.ToString();
            bestScoreText.text = bestScore.ToString();
        }
        else
        {
            recordCanvas.enabled = false;
        }
    }

    private void Awake()
    {
        //�ŏ��̖��]
        isFadeIn = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
        //object�̎擾
        goalUITf      =  goalCanvasObj.GetComponent<RectTransform>();
        pauseCanvas   =  pauseCanvasObj.GetComponent<Canvas>();
        recordCanvas  =  recordCanvasObj.GetComponent<Canvas>();
        //HP�\���͑S���ɂ��Ă���
        HPMeter.value = 1;
        

        //�Z���N�g�V�[���̎��͗]�v��UI������
        if (SceneManager.GetActiveScene().name == "SelectScene")
        {
            foreach (GameObject disableObject in disableUIList)
            {
                Destroy(disableObject);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        //��ʖ��]����
        if(isFadeIn == true)
        {
            darkAlpha -= Time.unscaledDeltaTime * darkAlphaSpeed;
            darkImageObj.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, darkAlpha);
            if (darkAlpha <= 0f)
            {
                darkAlpha = 0f;
                isFadeIn = false;
            }
            
        }

        //��ʈÓ]����
        if (isFadeOut == true)
        {
            darkAlpha += Time.unscaledDeltaTime * darkAlphaSpeed;
            darkImageObj.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, darkAlpha);
            if (darkAlpha >= 1.0f)
            {
                darkAlpha = 1.0f;
                isFadeOut = false;
            }
            
        }

        //���Ԃ̕\��
        timeText.text  = gM.TimeToString();

        //�X�R�A�\��
        scoreText.text = gM.ScereToString();

        //HP�\��
        HPMeter.value  = gM.SendHP();

        //�S�[������
        if (isGoal == true)
        {
            //�S�[����ʂ��ړ�������
            goalUITf.localPosition += new Vector3(0, goalDownSpeed, 0);
            if (goalUITf.localPosition.y >= 0)
            {
                goalDownSpeed = 0;
            }

        }

    }

}
