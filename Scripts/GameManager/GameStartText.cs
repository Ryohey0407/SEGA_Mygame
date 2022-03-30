using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartText : MonoBehaviour
{
    [SerializeField] GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetScene()
    {
        this.gameObject.SetActive(true);
        GameManager.WaitGame = true;
        Invoke("Vanish", 2f);
    }

    void Vanish()
    {
        GameManager.WaitGame = false;
        Destroy(gameObject);
    }


}
