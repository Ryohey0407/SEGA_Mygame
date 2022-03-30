using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleTextManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] Image textBack;

    AudioSource audioSource;
    [SerializeField] AudioClip Select;
    [SerializeField] AudioClip Decision;

    string state;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("down"))
        {
            state = "OPTION";
            animator.Play("option");
            audioSource.PlayOneShot(Select);
            textBack.GetComponent<RectTransform>().localScale = new Vector3(0, -1, 1);
        }
        if(Input.GetKeyDown("up"))
        {
            state = "GAME START";
            animator.Play("gamestart");
            audioSource.PlayOneShot(Select);
            textBack.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
        }

        if(Input.GetKeyDown("space"))
        {
            if(state == "GAME START")
            {
                audioSource.PlayOneShot(Decision);
                SceneManager.LoadScene("Training");
            }
            else if(state == "OPTION")
            {

            }
        }
    }
}
