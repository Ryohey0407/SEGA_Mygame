using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public bool WaitGame = false;
    [SerializeField] PlayerManager player;
    [SerializeField] GameObject GameStartText;
    [SerializeField] GameObject GameOverText;
    [SerializeField] GameObject GameClearText;
    [SerializeField] Text scoreText;

    const int MAX_SCORE = 99999;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameStartText.GetComponent<GameStartText>().SetScene();
        scoreText.text = score.ToString();
    }

    public void AddScore(int val)
    {
        score += val;
        if (score > MAX_SCORE)
        {
            score = MAX_SCORE;
        }
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.HP == 0)
        {
            GameOverText.SetActive(true);
            WaitGame = true;
        }
    }

    public void GameClear()
    {
        GameClearText.SetActive(true);
        WaitGame = true;
    }
}
