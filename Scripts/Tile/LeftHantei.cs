using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHantei : MonoBehaviour
{
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
            if (this.transform.position.x > player.transform.position.x + 0.4f)
            {
                parent.GetComponent<PolygonCollider2D>().enabled = false;
            }
            else
            {
                parent.GetComponent<PolygonCollider2D>().enabled = true;
            }
        }        
    }
}
