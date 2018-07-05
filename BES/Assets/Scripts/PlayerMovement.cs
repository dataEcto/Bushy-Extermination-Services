using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D player_rb;
	public float speed;
	private Vector2 moveVelocity;

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
		///NEW Movement Code
		///
		//This variable detects where we want to move
		//This is a quick way to map movement! 
		//Left Arrow key - Horizontal input becomes -1, Right Arrow key, 1, and so on with the Vertical
		//In Edit -> Project settings -> input you can change these inputs.
		Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Veritical"));

		//Calculates the speed;
		//While also normalizing diagonal speed
		moveVelocity = moveInput.normalized * speed;

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

	void FixedUpdate()
	{
		//Fixedupdate is called on every physics step in our game.
		player_rb.MovePosition(player_rb.position + moveVelocity *Time.fixedDeltaTime);

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

	private void OnCollisionEnter2D(Collision2D other)
	{
		 if (other.gameObject.tag == "projectile")
		{
			print("Take Damage");
			DealDamage(5);
		}
	}
}
