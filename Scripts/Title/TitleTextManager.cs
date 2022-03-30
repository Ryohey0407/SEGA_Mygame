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
    private float beforeVertical;

    // Start is called before the first frame update
    void Start()
    {
        state = "GAME START";
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxis("Vertical");
        Debug.Log(y);

        if ( y < 0 && beforeVertical == 0.0f)
        {
            state = "OPTION";
            animator.Play("option");
            audioSource.PlayOneShot(Select);
            textBack.GetComponent<RectTransform>().localScale = new Vector3(0, -1, 1);
        }
        if( y > 0 && beforeVertical == 0.0f)
        {
            state = "GAME START";
            animator.Play("gamestart");
            audioSource.PlayOneShot(Select);
            textBack.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
        }

        beforeVertical = y;

        if (Input.GetButtonDown("Jump"))
        {
            if(state == "GAME START")
            {
                audioSource.PlayOneShot(Decision);
                SceneManager.LoadScene("Training");
            }
            else if(state == "OPTION")
            {
                Application.Quit();
            }
        }
    }
}
