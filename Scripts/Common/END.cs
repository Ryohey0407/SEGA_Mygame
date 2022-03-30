using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class END : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Restart", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Restart()
    {
        SceneManager.LoadScene("Title");
    }
}
