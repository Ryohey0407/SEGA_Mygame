using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
	private GameObject player;
	float speed = 48;
	float my_x;
	float my_y;
	float pl_x;
	float pl_y;


	Rigidbody2D rb;
	ContactFilter2D filter2d;

	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.FindWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
		this.transform.localScale =new Vector3(player.transform.localScale.x,1,1);
		rb.velocity = (transform.right * speed * transform.localScale.x);
	}

	// Update is called once per frame
	void Update()
    {
		my_x = transform.position.x;
		my_y = transform.position.y;
		pl_x = player.transform.position.x;
		pl_y = player.transform.position.y + 1f;
	}

	void FixedUpdate()
    {
		if(Mathf.Abs(rb.velocity.x) > 0 || (Mathf.Abs(rb.velocity.y) > 0))
        {
			Turn();
        }
		else
		{
			PlayerFollow();
        }
	}
	void Turn()
    {
		speed -= 3f;
		rb.velocity = (transform.right * speed * transform.localScale.x);
	}

	void PlayerFollow()
	{
		float fixed_x = (my_x + pl_x) / 2;
		float fixed_y = (my_y + pl_y) / 2;
		transform.position = new Vector3(fixed_x, fixed_y, 0);
		if (Mathf.Abs(fixed_x - player.transform.position.x) < Mathf.Abs(0.5f))
		{
			Destroy(gameObject);
		}
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject)
		{
			rb.velocity = new Vector2(0, 0);
		}
	}

}
