using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//�ۑ�����f�[�^�̌^
[System.Serializable]
public class SaveData
{
    public float[] timeHighScore  = new float[3];
    public float[] scoreHighScore = new float[3];

}

[System.Serializable]
public class SettingsForScene
{
    public string sceneName;
    public int dataSlotNumber;
    public int BGMListNumber;
}


public class MainGameManager : MonoBehaviour
{
    [SerializeField] private SEManager   SEm;
    [SerializeField] private BGMManager  BGMm;
    [SerializeField] private UIMovement  UIm;

    [SerializeField] private SettingsForScene[] settingsForScenes;

    //save�֘A
    private SaveData data;
    private bool isBestTime = false;
    private bool isBestScore = false;
    SaveData MyLoad()
    {
        string datastr;

        if (PlayerPrefs.HasKey("PlayerData"))
        {
            //�f�[�^������Γǂݍ���
            datastr = PlayerPrefs.GetString("PlayerData");
            return JsonUtility.FromJson<SaveData>(datastr);
        }
        else
        {
            //�f�[�^���Ȃ�������V�������
            Debug.Log("new");
            SaveData newData = new SaveData();
            return newData;
        }

    }

    void MySave(SaveData myData)
    {
        //�ύX���ꂽmydata��json�ɂ���PlayerPrefs�ɏ㏑���ۑ�
        string datastr = JsonUtility.ToJson(myData);
        PlayerPrefs.SetString("PlayerData", datastr);
        PlayerPrefs.Save();
    }


    //scene�֘A
    private float fadeWaitTime = 3.0f;
    //Scene��ǂݍ��ފ֐�
    public void MainSceneManager(string roadSceneName)
    {
        UIm.DoFadeOut(true);
        SEm.SEPlay(4);
        StartCoroutine(Scenechange(roadSceneName));
    }

    IEnumerator Scenechange(string sceneName)
    {
        //delay�b�҂�
        yield return new WaitForSecondsRealtime(fadeWaitTime);
        /*����*/
        SceneManager.LoadScene(sceneName);
    }

    //UI�֘A


    //time�Epause�֘A
    private float currentGameTime = 0f;
    private bool isPause = false;
    public string TimeToString()
    {
        string timeText;
        timeText = currentGameTime.ToString();
        return timeText;
    }

    //SE�֘A
    public bool RobotSEStop()
    {
        bool isStop;
        if(isGoal == true || isGameOver == true)
        {
            isStop = true;
            return isStop;
        }

        return false;
    }

    //position�֘A
    private Vector3 playerPos;
    public void ReadPlayerPosition(Vector3 readPlayerPos)
    {
        playerPos = readPlayerPos;
    }
    public Vector3 SendPlayerPosition()
    {
        Vector3 sendPlayerPos;
        sendPlayerPos = playerPos;
        return sendPlayerPos;
    }

    //HP�֘A
    [SerializeField] private int  charaHP_MAX = 100;
    private float  charaHP     = 0;
    private bool   isForcused;

    [SerializeField] private int   damageScale = 10;
    [SerializeField] private float healScale   = 0.5f;
        //HP��UI�ɑ���
    public float SendHP()
    {
        return charaHP / charaHP_MAX;
    }

    //tracking�֘A
    public void ForcusedActivate(bool isReadTracking)
    {
        isForcused = isReadTracking;
        
    }

    //score�֘A
    private int  currentScore = 0;    
        //�X�R�A�̗ݐςƊl������SE��炷
    public void ScoreManager(int addingScore, bool isGold)
    {            
        
        currentScore += addingScore;
        
        if(isGold == true) {
            SEm.SEPlay(2);
         }
         else
         {
            SEm.SEPlay(1);
         }
            
     }
        //�X�R�A�����A���^�C���\��
    public string ScereToString()
    {
        string scoreText;
        scoreText = currentScore.ToString();
        return scoreText;
    }
        //�X�R�A�̋L�^�X�V
    public void BestScoreUpdate(int dataSlotNum)
    {
        if (data.timeHighScore[dataSlotNum] > currentGameTime || data.timeHighScore[dataSlotNum] == 0)
        {
            data.timeHighScore[dataSlotNum] = currentGameTime;
            isBestTime = true;
            MySave(data);
        }

        if (data.scoreHighScore[dataSlotNum] < currentScore || data.scoreHighScore[dataSlotNum] == 0)
        {
            data.scoreHighScore[dataSlotNum] = currentScore;
            isBestScore = true;
            MySave(data);
        }

    }

    //goal�֘A
    private bool isGoal         = false;
    private int  goalFlagNumber = 1;
    public void DoGoal()
    {
        isGoal = true;
        isPause = true;
    }

    //gameover�֘A
    private bool isGameOver;

    //transport�֘A
    private bool isTransporting;
    private string  selectingSceneName;
    private int selectingSceneDataSlot;
    public void ReadTransStatus(bool isReadTrans, string readSelectStageName)
    {
        isTransporting = isReadTrans;
        selectingSceneName = readSelectStageName;
    }
    public void TransportSelectScene(string selectSceneName)
    {
        SEm.SEPlay(4);
        MainSceneManager(selectSceneName);
    }
   

    private void Awake()
    {
        //�t�@�C���ǂݍ���
        data = MyLoad();
        //���Ԃ�i�߂�
        Time.timeScale = 1.0f;
    }


    private void Start()
    {

        Debug.Log(JsonUtility.ToJson(data, true));

        //HP�͊J�n����MAX
        charaHP = charaHP_MAX;

        //BGM���V�[���ɍ��킹�ĕς���
        for(int i = 0; i < settingsForScenes.Length; i++)
        {
            if(SceneManager.GetActiveScene().name == settingsForScenes[i].sceneName)
            {
                BGMm.BGMPlay(settingsForScenes[i].BGMListNumber);
            }
        }
        
    }

        private void Update()
        {

        //�픭������
        UIm.ForecusedUIActivate(isForcused);


        //transport���g���Ƃ��A�U��ꂽ�ԍ��ɉ������X�e�[�W�ɔ�΂�
        if (isTransporting == true && selectingSceneName != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && isPause == false)
            {
                Debug.Log("staged");
                TransportSelectScene(selectingSceneName);
            }
        }

        //HP���O�ɂȂ�����Q�[���I�[�o�[
        if (charaHP <= 0f)
        {
            isGameOver = true;
            UIm.GameOverUIActivate();
            Time.timeScale = 0f;
            BGMm.BGMStop();
            RobotSEStop();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MainSceneManager("SelectScene");
                SEm.SEPlay(0);
            }
            
        }

        //�|�[�Y��ʏ���
        //Q�������ꂽ��|�[�Y���
        //�|�[�Y����Q�������ꂽ��|�[�Y����
        UIm.PauseUIActivate(isPause, isGoal);

            if (Input.GetKeyDown(KeyCode.Q)) {

                if(isPause == false)
                {
                    isPause = true;
                    SEm.SEPlay(3);
                    
                }
                else
                {
                    isPause = false;
                    Time.timeScale = 1.0f;
                }
                
            }

            //�|�[�Y���͎��Ԓ�~
            //1�������ꂽ��ĊJ�A�Q�������ꂽ��Z���N�g��ʂցA�R�������ꂽ��Q�[���I��
            if (isPause == true)
            {
                Time.timeScale = 0f;

                if (Input.GetKeyDown(KeyCode.Alpha1)) { 
                    isPause = false;
                    SEm.SEPlay(0);
                    Time.timeScale = 1.0f;
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SEm.SEPlay(0);
                    MainSceneManager("SelectScene");
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Application.Quit();//�Q�[���v���C�I��
                }

            }

            //�S�[������
        if(isGoal == true)
        {
            if(goalFlagNumber == 1)
            {
                BGMm.BGMStop();
                RobotSEStop();
                SEm.SEPlay(6);
                goalFlagNumber = 2;
            }

            //�X�R�A�𒲂ׂăx�X�g�X�R�A��������L�^
            for(int i = 0; i < settingsForScenes.Length; i++)
            {
                if(SceneManager.GetActiveScene().name == settingsForScenes[i].sceneName)
                {
                    BestScoreUpdate(settingsForScenes[i].dataSlotNumber);
                }
            }
            UIm.GoalUIActivate(currentGameTime,currentScore,isBestTime,isBestScore);

            //Space�ŃZ���N�g�V�[���ɖ߂�
            if (Input.GetKeyDown(KeyCode.Space))
                {
                    SEm.SEPlay(0);
                    MainSceneManager("SelectScene");
                }
            }
            //�S�[�����������ŏI���



    }

    private void FixedUpdate()
    {
        

        //Time�𐔂���
        currentGameTime += Time.deltaTime;

        //HP�̌v�Z
        if (isForcused == true)
        {
            charaHP -= damageScale * Time.deltaTime;
           
        }
        else
        {
            if (charaHP < charaHP_MAX)
                charaHP += healScale;
        }

        
        

        if (isTransporting == true)
        {
            
            for (int i = 0; i < settingsForScenes.Length; i++)
            {
                if (selectingSceneName == settingsForScenes[i].sceneName)
                {
                    selectingSceneDataSlot = settingsForScenes[i].dataSlotNumber;
                    break;
                }

            }
            
        }

        UIm.TransUIActivate(isTransporting, selectingSceneName, data.timeHighScore[selectingSceneDataSlot], data.scoreHighScore[selectingSceneDataSlot]);

    }

}


