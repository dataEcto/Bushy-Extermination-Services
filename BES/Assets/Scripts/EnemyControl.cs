using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {

	public float speed;
	public float stoppingDistance;
	public float retreatDistance;

	//This is very similar to the code in reflecting the shield.
	private float timeBtwShots;
	public float startTimeBtwShots;

	public GameObject projectile;
	public Transform player;

	void Start () {
		timeBtwShots = startTimeBtwShots;
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	

	void Update () {

		//If the enemy is far away from the player, move closer
		if (Vector2.Distance(transform.position, player.position) > stoppingDistance) 
		{
			transform.position = Vector2.MoveTowards(transform.position, player.position, speed *Time.deltaTime);
		}
		//If it is near, stop moving
		else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
		{
			transform.position = this.transform.position;
		}
		//It may also want to retreat as well if the player gets close while its stopped
		else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
		{
			transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
		}

		//Like in attacking with our player, we want to prevent constant shooting.
		if (timeBtwShots <= 0)
		{
			//Instantiate(what we want to spawn, where, and what rotation)
			Instantiate(projectile, transform.position, Quaternion.identity);
			//We can edit startTimeBtwShots without using any hardcode
			//and just do it in the inspector.
			timeBtwShots = startTimeBtwShots;
		}
		else
		{
			timeBtwShots -= Time.deltaTime;
		}
	}
}
