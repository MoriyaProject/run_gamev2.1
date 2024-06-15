using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//保存するデータの型
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

    //save関連
    private SaveData data;
    private bool isBestTime = false;
    private bool isBestScore = false;
    SaveData MyLoad()
    {
        string datastr;

        if (PlayerPrefs.HasKey("PlayerData"))
        {
            //データがあれば読み込む
            datastr = PlayerPrefs.GetString("PlayerData");
            return JsonUtility.FromJson<SaveData>(datastr);
        }
        else
        {
            //データがなかったら新しく作る
            Debug.Log("new");
            SaveData newData = new SaveData();
            return newData;
        }

    }

    void MySave(SaveData myData)
    {
        //変更されたmydataをjsonにしてPlayerPrefsに上書き保存
        string datastr = JsonUtility.ToJson(myData);
        PlayerPrefs.SetString("PlayerData", datastr);
        PlayerPrefs.Save();
    }


    //scene関連
    private float fadeWaitTime = 3.0f;
    //Sceneを読み込む関数
    public void MainSceneManager(string roadSceneName)
    {
        UIm.DoFadeOut(true);
        SEm.SEPlay(4);
        StartCoroutine(Scenechange(roadSceneName));
    }

    IEnumerator Scenechange(string sceneName)
    {
        //delay秒待つ
        yield return new WaitForSecondsRealtime(fadeWaitTime);
        /*処理*/
        SceneManager.LoadScene(sceneName);
    }

    //UI関連


    //time・pause関連
    private float currentGameTime = 0f;
    private bool isPause = false;
    public string TimeToString()
    {
        string timeText;
        timeText = currentGameTime.ToString();
        return timeText;
    }

    //SE関連
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

    //position関連
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

    //HP関連
    [SerializeField] private int  charaHP_MAX = 100;
    private float  charaHP     = 0;
    private bool   isForcused;

    [SerializeField] private int   damageScale = 10;
    [SerializeField] private float healScale   = 0.5f;
        //HPをUIに送る
    public float SendHP()
    {
        return charaHP / charaHP_MAX;
    }

    //tracking関連
    public void ForcusedActivate(bool isReadTracking)
    {
        isForcused = isReadTracking;
        
    }

    //score関連
    private int  currentScore = 0;    
        //スコアの累積と獲得時のSEを鳴らす
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
        //スコアをリアルタイム表示
    public string ScereToString()
    {
        string scoreText;
        scoreText = currentScore.ToString();
        return scoreText;
    }
        //スコアの記録更新
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

    //goal関連
    private bool isGoal         = false;
    private int  goalFlagNumber = 1;
    public void DoGoal()
    {
        isGoal = true;
        isPause = true;
    }

    //gameover関連
    private bool isGameOver;

    //transport関連
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
        //ファイル読み込み
        data = MyLoad();
        //時間を進める
        Time.timeScale = 1.0f;
    }


    private void Start()
    {

        Debug.Log(JsonUtility.ToJson(data, true));

        //HPは開始時にMAX
        charaHP = charaHP_MAX;

        //BGMをシーンに合わせて変える
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

        //被発見処理
        UIm.ForecusedUIActivate(isForcused);


        //transportを使うとき、振られた番号に応じたステージに飛ばす
        if (isTransporting == true && selectingSceneName != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && isPause == false)
            {
                Debug.Log("staged");
                TransportSelectScene(selectingSceneName);
            }
        }

        //HPが０になったらゲームオーバー
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

        //ポーズ画面処理
        //Qが押されたらポーズ画面
        //ポーズ中にQが押されたらポーズ解除
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

            //ポーズ中は時間停止
            //1が押されたら再開、２が押されたらセレクト画面へ、３が押されたらゲーム終了
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
                    Application.Quit();//ゲームプレイ終了
                }

            }

            //ゴール処理
        if(isGoal == true)
        {
            if(goalFlagNumber == 1)
            {
                BGMm.BGMStop();
                RobotSEStop();
                SEm.SEPlay(6);
                goalFlagNumber = 2;
            }

            //スコアを調べてベストスコアだったら記録
            for(int i = 0; i < settingsForScenes.Length; i++)
            {
                if(SceneManager.GetActiveScene().name == settingsForScenes[i].sceneName)
                {
                    BestScoreUpdate(settingsForScenes[i].dataSlotNumber);
                }
            }
            UIm.GoalUIActivate(currentGameTime,currentScore,isBestTime,isBestScore);

            //Spaceでセレクトシーンに戻る
            if (Input.GetKeyDown(KeyCode.Space))
                {
                    SEm.SEPlay(0);
                    MainSceneManager("SelectScene");
                }
            }
            //ゴール処理ここで終わり



    }

    private void FixedUpdate()
    {
        

        //Timeを数える
        currentGameTime += Time.deltaTime;

        //HPの計算
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


