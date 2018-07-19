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

	//Progress Bar Variables
	public Slider progressBar;
	public float currentProgress { get; set; }
	public float maxProgress { get; set; }

	///Attacking Variables
	public float timeBtwAttack;
	public float startTimeBtwAttack;
	private Animator player_anim;
	public bool attacking;

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

		//The maximum amount of points needed to fill out the Progress Bar
		maxProgress = 20f;
		//Reset the progress bar to 0 everytime the game starts
		currentProgress = 0;
		//Similar to the healtbar, we need to set the value of the Progress Bar/Slider object
		progressBar.value = CalculateProgress();

		attacking = false;

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
		//Displays the CURRENT progress
		progressBar.value = CalculateProgress();

		///Attacking Code
		//Time between attack starts off as StartTimeBtwAttack everytime it is less than 0
		//It seems like a paradox in the way its written here, actually.
		if (timeBtwAttack <= 0)
		{
			
			//Enable the attack
			if (Input.GetKey("space"))
			{
				player_anim.SetTrigger("attack");

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

	//Same as above
	float CalculateProgress()
	{
		return currentProgress / maxProgress;
	}

	//This method will fill up the progress bar
	//Whenever the reflected projectile hits an enemy!
	//This method will be called in EnemyControl
	//Because there is code in there that focues on what to do
	//When the enemy is hit
	public void fillBar(float progressValue)
	{
		currentProgress += progressValue;
		progressBar.value = CalculateProgress();

		if (currentProgress > maxProgress)
		{
			currentProgress = maxProgress;

		}
	}

	//This function deals damage to the game object it is attached to
	//By "deal damage", i technically mean that this function will subtract
	//damageValue from currentHealth
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

	//This code restores health
	//Used for fixing the damage taking while reflecting issue
	public void RestoreHealth(float healthGained)
	{
		Debug.Log("Healing");
		//Deal damage to the health bar
		currentHealth += healthGained;
		healthBar.value = CalculateHealth();

		//Prevent the player from restoring full health
		if (currentHealth >= maxHealth)
		{
			currentHealth -= 1;
			Debug.Log("Health has been restored to full");
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
