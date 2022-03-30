using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBrock : MonoBehaviour
{
    float yspeed;
    float Maxyspeed = 2;
    public int newPosx;
    Vector3 pos;
    public string state = "UP";

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(transform.position.y > pos.y + newPosx)
        {
            state = "DOWN";
        }
        else if(transform.position.y < pos.y)
        {
            state = "UP";
        }

        if(state == "UP")
        {
            if(yspeed >= Maxyspeed)
            {
                yspeed = Maxyspeed;
            }
            else
            {
                yspeed += 0.05f;
            }
        }
        else if(state == "DOWN")
        {
            if (yspeed <= -Maxyspeed)
            {
                yspeed = -Maxyspeed;
            }
            else
            {
                yspeed -= 0.05f;
            }
        }
        transform.Translate(new Vector2(0, yspeed * Time.deltaTime));
    }
}
