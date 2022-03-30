using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    GameManager gameManager;
    public GameObject rightbullet;
    public GameObject leftbullet; 
    public GameObject upbullet;

    [SerializeField] GameObject DeathEffect;

    float timer;


    public int HP = 6;
    SpriteRenderer sp = null;

    // Start is called before the first frame update
    void Start()
    {
        timer = 7;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!sp.isVisible)
        {
           return;
        }
        timer -= Time.deltaTime;
        Debug.Log(Mathf.Round(timer));
        if (timer < 0)
        {
            timer = 7;
        }
        else if(Mathf.Round(timer) == 5f)
        {
            timer -= 1;
            Shot1();
        }
        else if (Mathf.Round(timer) == 2f)
        {
            timer -= 1;
            Shot2();
        }

        if (HP <= 0)
        {
            gameManager.AddScore(100);
            Instantiate(DeathEffect, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            HP -= 1;
        }
    }

    void Shot1()
    {
        Instantiate(rightbullet, transform.position, transform.rotation);
        Instantiate(leftbullet, transform.position, transform.rotation);
    }
    void Shot2()
    {
        Instantiate(upbullet, transform.position, transform.rotation);
    }
}
