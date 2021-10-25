using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text HighScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_highScore;
    
    private bool m_GameOver = false;

    public string playerName;

    public GameObject myPlayerNameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
        if(PlayerNameManager.Instance != null)
        {
            playerName = PlayerNameManager.Instance.playerName;
        }
        else
        {
            playerName = "No Player Name";
        }
        HighScoreText.text = $"High Score: {m_highScore} | Name: {playerName}";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        m_highScore = Mathf.Max(m_Points, m_highScore);
        ScoreText.text = $"Score : {m_Points}";
        HighScoreText.text = $"High Score: {m_highScore} | Name: {playerName}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        SaveScore();
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    public class SaveData
    {
        public int highScore;
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_highScore = data.highScore;
        }
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.highScore = m_highScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
