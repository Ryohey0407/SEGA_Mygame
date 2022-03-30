using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour
{
    [SerializeField] Text myText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject Rank;

    float resultscore = -1;
    int score;

    bool resultON=false;

    // Start is called before the first frame update

    void Start()
    {
        score = int.Parse(scoreText.text);
        Invoke("Resultscore",2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(resultON == true)
        {
            resultscore = (int)Mathf.MoveTowards(resultscore, score, Time.time);
            myText.text = resultscore.ToString();
        }
        if(resultscore == score)
        {
            Rank.SetActive(true);
        }
    }

    void Resultscore()
    {
        resultON = true;
    }
}
