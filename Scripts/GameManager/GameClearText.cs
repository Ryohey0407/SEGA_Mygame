using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearText : MonoBehaviour
{

    [SerializeField] GameManager GameManager;
    [SerializeField] GameObject ResultScene;
    [SerializeField] GameObject fadeImage;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Result", 2f);
    }


    void Result()
    {
        ResultScene.SetActive(true);
        Invoke("RestartScene", 5f);
    }


    void RestartScene()
    {
        SceneManager.LoadScene("END");
    }

    
}
