using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCheck : MonoBehaviour
{
    public GameObject parent;

    Rigidbody2D rb;
    public float waterDrag = 3.0f;
    float defaultDrag = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        rb = parent.GetComponent<Rigidbody2D>();
        defaultDrag = rb.drag;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("1");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Debug.Log("2");
            rb.drag = waterDrag;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Debug.Log("3");
            rb.drag = defaultDrag;
        }
    }

}
