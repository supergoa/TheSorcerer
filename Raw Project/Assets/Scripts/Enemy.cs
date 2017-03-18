using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that attaches to Enemy prefabs to controll Enemy AI
 * 
 **/
public class Enemy : MonoBehaviour {

	// The sole instance player
	public GameObject player;
	// The sole instance player's transform
	public Transform target;

	// How much health the Enemy has
	public int health;
	// Display of health above the Enemy
	Slider healthBar;
	Image fillArea;

	// How much damage the Enemy does per 1.0s
	public int damage;

	public float moveSpeed;

	bool isDead = false;

	// How close the player has to be in order for the Enemy to activate
	public float aquireTargetDistance;

	Rigidbody rb;
	// Controlls the Enemy's animation
	public Animator anim;

	public int collisionMask;

	// Determines whether the Enemy will deal damage to the player
	bool playerInTerritory;
	float timePlayerInTerritory = 0;

	// Prevents edge case of Enemy's getting stuck on corners
	Vector3 leftJumpAngle = new Vector3(-.2f, 1, 0);
	Vector3 rightJumpAngle = new Vector3(.2f, 1, 0);

	// Initialization
	void Start () {
		healthBar = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
		fillArea = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
		healthBar.maxValue = health;
		healthBar.minValue = 0;
		healthBar.value = health;
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update ()
	{
		// Look in the direction of the player
		Vector3 toLook = new Vector3(target.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		transform.LookAt (toLook);

		// Modify the speed parameter of the Animator to match the actial movement speed
		anim.SetFloat("speed", Mathf.Abs(rb.velocity.x) / 3f);

		if (isDead) {
			anim.SetBool("death_1", false);
			anim.SetBool("death_2", false);
		}

		if (health <= 0 && !isDead) {
			die();
		}

		if (!isDead) {
			applyStayDamage();
		}
	}

	/**
	 * If the player never leaves the Enemy's collider, it keep attacking
	 * Restarts the current level
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void applyStayDamage(){
		if (playerInTerritory) {
			timePlayerInTerritory += Time.deltaTime;
		} else {
			timePlayerInTerritory = 0;
		}

		// Applies damage evers ~1.0 seconds
		if (timePlayerInTerritory > .03f && Mathf.Abs(timePlayerInTerritory % 1f) < .03f) {
			anim.SetTrigger("attack");
			player.GetComponent<Player>().ApplyDamage(damage);
			timePlayerInTerritory = .03f;
		}
	}

	/**
	 * Moves to the player if the player is in range
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void FixedUpdate() {
		if (!playerInTerritory && Mathf.Abs(player.transform.position.x - transform.position.x) <= aquireTargetDistance && Mathf.Abs(player.transform.position.y - transform.position.y) <= aquireTargetDistance && !isDead) {
			MoveToPlayer();
		}
	}

	/**
	 * A game event triggers this method.
	 * When the player enters the Enemy's collider, deal damage to it and play the appropriate animation
	 * 
	 * @param other
	 * @return
	 * 
	 **/
	void OnTriggerEnter (Collider other)
	{
		if (other.tag.Equals("Player") && !isDead) {
			anim.SetTrigger("attack");
			player.GetComponent<Player>().ApplyDamage(damage);
			//other.transform.parent.SendMessage("ApplyDamage", damage);
			playerInTerritory = true;
		}
	}

	/**
	 * A game event triggers this method.
	 * When the player exits the Enemy's collider, stop dealing damage
	 * 
	 * @param other is the Collider of the player object
	 * @return
	 * 
	 **/
	void OnTriggerExit (Collider other)
	{
		if (other.tag.Equals("Player")) {
			playerInTerritory = false;
		}
	}

	/**
	 * Called every fram when in range.
	 * Moves towards the player
	 * 
	 * @param
	 * @return
	 * 
	 **/
	void MoveToPlayer () {

		// Move right if the player is to the right of the Enemy
		if (player.transform.position.x >= transform.position.x) {

			// Make the mesh face the player
			transform.rotation = Quaternion.Euler(0, 90, 0);

			//move right at the Enemy's movespeed
			rb.AddForce(Vector3.right * moveSpeed);

			// Jump up and to the left when stuck
			if (Mathf.Abs(rb.velocity.x) <= .1f) {
				Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y+.05f, transform.position.z);
				if (Physics.Raycast(rayOrigin, Vector2.down, 1.25f)) {
					rb.AddForce(leftJumpAngle * 250f);
				}
			}
		}
		// Move left if the player is to the left of the Enemy
		else {
			
			// Make the mesh face the player
			transform.rotation = Quaternion.Euler(0, 270, 0);

			//move left at the Enemy's movespeed
			rb.AddForce(Vector3.left * moveSpeed);

			// Jump up and to the right when stuck
			if (Mathf.Abs(rb.velocity.x) <= .1f) {
				Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y+.05f, transform.position.z);
				if (Physics.Raycast(rayOrigin, Vector2.down, 1.25f)) {
					rb.AddForce(rightJumpAngle * 250f);
				}
			}
		}
	}

	/**
	 * Called by DealDamage.cs
	 * Deals damage to this Enemy
	 * 
	 * @param dmg is the amount of damage to subtract from the Enemy's current health
	 * @return
	 * 
	 **/
	public void ApplyDamage(int dmg) {
		health -= dmg;
		healthBar.value = health;
	}

	/**
	 * Called by ApplyFreeze.cs
	 * Reduces this Enemy velocity
	 * 
	 * @param percentSlow is the percentage the Enemy's velocity is being reduced
	 * @return
	 * 
	 **/
	public void ApplyFreeze(float percentSlow) {
		Vector3 tempVelocity = this.rb.velocity;
		this.rb.velocity = tempVelocity * (1f - percentSlow);
	}

	/**
	 * Called when health <= 0
	 * Plays Enemy's death animation and destroys its GameObject
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void die() {
		float whichDeath = Random.value;
		fillArea.color = Color.white;
		isDead = true;

		if (whichDeath >= 0.5f) {
			anim.SetBool("death_1", true);
		} else {
			anim.SetBool("death_2", true);
		}

		Destroy(transform.gameObject, 3f);
	}
}
