﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour {
	public float speed;
	public int damage;
	private Transform player;
	private GameObject playerScript;
	private Vector2 target;
	public bool reflectProjectile;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		target = new Vector2(player.position.x, player.position.y);

	}
	

	void Update () {

		//This is basically the core of making the projectile move.
		//If you want to have a homing projectile towards the player, change "target" with player
		if (reflectProjectile == false)
		{
			transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
	
		}
		//This allows for reflection
		else
		{
			transform.position = Vector2.MoveTowards(transform.position, target, -speed * Time.deltaTime);
			
		}
		

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

		if (collisionInfo.gameObject.tag == "shield_tag")
		{
			reflectProjectile = true;
			//This function is here because
			//Whenever I reflect the projectile, the player still ends up taking damage
			//So a fix would be to restore health at the same time
			GameObject.Find("Player").GetComponent<PlayerMovement>().RestoreHealth(6);
		}

	}
}
