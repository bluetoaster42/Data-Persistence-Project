using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerNameManager : MonoBehaviour
{
    public string playerName;
    public static PlayerNameManager Instance;

    public TMP_InputField nameField;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitPlayerNameAndBegin()
    {
        PlayerNameManager.Instance.playerName = nameField.text;
        SceneManager.LoadScene(1);
    }
}
