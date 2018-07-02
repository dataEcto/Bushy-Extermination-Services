using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D player_rb;
	public float speed;

	//Health Bar Variables
	public Slider healthBar;
	public float currentHealth { get; set; }
	public float maxHealth { get; set; }
	public TextMeshProUGUI healthBarText;

	public Animator shield_anim;

	void Start ()
	{
		player_rb = GetComponent<Rigidbody2D>();

		//can be any value of course
		maxHealth = 100f;

		//This is to reset the value of the health bar to full health every time the game is loaded.
		currentHealth = maxHealth;

		///Visual representation of the health.
		//Get the value of the slider 
		//set it to calculate health
		healthBar.value = CalculateHealth();

		//Display the intial health
		healthBarText.text = "Health: " + currentHealth;

	}
	
	
	void Update ()
	{
		player_rb.velocity = new Vector2(0, 0);

		//Movement
		if (Input.GetKey(KeyCode.W))
		{
			player_rb.velocity += new Vector2(0, speed);
		}

		if (Input.GetKey(KeyCode.S))
		{
			player_rb.velocity += new Vector2(0, -speed);
		}

		if (Input.GetKey(KeyCode.D))
		{
			player_rb.velocity += new Vector2(speed, 0);
		}

		if (Input.GetKey(KeyCode.A))
		{
			player_rb.velocity += new Vector2(-speed, 0);
		}

		//Displays the CURRENT health
		healthBarText.text = "Health: " + Mathf.Round(currentHealth);

		//Test Damage Input
		//Again, the player won't actually take damage when space bar is pressed.
		if (Input.GetKey(KeyCode.L))
		{
			DealDamage(5);
		}

		if (shield_anim.GetBool("Shield"))
		{
			shield_anim.SetBool("Shield", false);
			print("finished");
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			shield_anim.SetBool("Shield", true);
			print("attack");
		}

	

	}

	//The function that calculates what the health is currently.
	//What it does is take the health at the moment, then divide it by max health
	float CalculateHealth()
	{
		return currentHealth / maxHealth;
	}


	//A temporary function used just to check if damage is being dealt to the player
	//This will be deleted later, or perhaps changed so that when the player collides with an enemy/projectile,
	//This function will be called.
	void DealDamage(float damageValue)
	{

		//Deal damage to the health bar
		currentHealth -= damageValue;
		//Same as from start
		healthBar.value = CalculateHealth();

		//If the character is out of health, they die
		if (currentHealth <= 0)
		{
			//This displays the health to be 0.
			//This is to prevent negative numbers to show up
			healthBarText.text = "Health: " + 0;
			print("You Died");
			Destroy(gameObject);
		}

	}
}
