using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour {
	public float speed;
	public int damage;
	private Transform player;
	private Vector2 target;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;

		target = new Vector2(player.position.x, player.position.y);

	}
	

	void Update () {

		//This is basically the core of making the projectile move.
		//If you want to have a homing projectile towards the player, change target with player
		transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

		//If the x and y coordinates are equal to the targets coordinates
		if (transform.position.x == target.x && transform.position.y == target.y)
		{
			DestroyProjectile();
		}
	}

	void DestroyProjectile()
	{
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "player")
		{
			DestroyProjectile();

		}
	}
}
