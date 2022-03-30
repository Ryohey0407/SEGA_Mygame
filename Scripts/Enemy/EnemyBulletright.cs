using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletright : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.transform.localScale = new Vector3(parent.transform.localScale.x, 1, 1);
        rb.velocity = (transform.right * 6 * transform.localScale.x);
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
