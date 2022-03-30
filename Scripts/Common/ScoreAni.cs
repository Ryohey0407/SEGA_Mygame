using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAni : MonoBehaviour
{
    [SerializeField] Text scoreText;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int score = int.Parse(scoreText.text);
        Debug.Log(score);

        if (score < 2000)
        {
            animator.Play("RankC");
        }
        else if (score < 4000)
        {
            animator.Play("RankB");
        }
        else if (score < 7000)
        {
            animator.Play("RankA");
        }
        else
        {
            animator.Play("RankS");
        }
    }
    void OncompleteAnimation()
    {
        
    }
}
