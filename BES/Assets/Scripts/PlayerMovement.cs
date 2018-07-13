using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour {

	///Movement Variables
	Rigidbody2D player_rb;
	public float speed;
	private Vector2 moveVelocity;

	///Health Bar Variables
	public Slider healthBar;
	public float currentHealth { get; set; }
	public float maxHealth { get; set; }
	public TextMeshProUGUI healthBarText;

	///Attacking Variables
	public float timeBtwAttack;
	public float startTimeBtwAttack;
	private Animator player_anim;

	//Parameters for OverlapCircleAll()
	public Transform attackPos;
	public float attackRange;
	public LayerMask whatIsProjectile;

	void Start ()
	{
		player_rb = GetComponent<Rigidbody2D>();
		player_anim = GetComponent<Animator>();

		//The Maximum Health the player has
		maxHealth = 100f;
		//This is to reset the value of the health bar to full health every time the game is loaded.
		currentHealth = maxHealth;
		///Visual representation of the health.
		//Get the value of the slider object, 
		//set it to calculate health
		healthBar.value = CalculateHealth();
		//Display the intial health
		healthBarText.text = "Health: " + currentHealth;

	}
	
	
	void Update ()
	{
		///NEW Movement Code
		//This variable detects where we want to move
		//This is a quick way to map movement! 
		//Left Arrow key - Horizontal input becomes -1, Right Arrow key, 1, and so on with the Vertical
		//In Edit -> Project settings -> input you can change these inputs.
		Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));
		//Calculates the speed;
		//While also normalizing diagonal speed
		moveVelocity = moveInput.normalized * speed;

		//Displays the CURRENT health
		healthBarText.text = "Health: " + Mathf.Round(currentHealth);

		///Attacking Code
		//Time between attack starts off as StartTimeBtwAttack everytime it is less than 0
		//It seems like a paradox in the way its written here, actually.
		if (timeBtwAttack <= 0)
		{
			//Enable the attack
			if (Input.GetKey("space"))
			{
				player_anim.SetTrigger("attack");

				//Here, I am making an array of 2d colliders 
				//Whats in this array?
				//It all depends on what the OverlapCircleAll() function has found within itself.
				Collider2D[] projectilesToDeflect = Physics2D.OverlapCircleAll(attackPos.position,attackRange,whatIsProjectile);

				for (int i = 0; i < projectilesToDeflect.Length; i++)
				{
					//Code that makes it reflect back. 
					//Go back to this once the enemy ai is done.
					Debug.Log("Reflect!");
				}

				Debug.Log("Attacking");

				timeBtwAttack = startTimeBtwAttack;
			}

			

		}

		else
		{
			//Start counting down to allow attack
			timeBtwAttack -= Time.deltaTime;
		}

		
	}

	//Fixedupdate is called on every physics step in our game.
	void FixedUpdate()
	{
		
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
	public void DealDamage(float damageValue)
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		 if (other.gameObject.tag == "projectile")
		{
			print("Take Damage");
			DealDamage(5);
		}
	}

	//Visual representation of hitbox
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}
}
