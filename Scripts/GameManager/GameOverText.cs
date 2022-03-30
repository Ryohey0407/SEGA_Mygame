using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverText : MonoBehaviour
{
    [SerializeField] GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("RestartScene", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
