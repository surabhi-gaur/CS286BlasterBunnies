using UnityEngine;

public class BossMainScript : MonoBehaviour
{
	private bool hasSpawn;
	
	//  Component references
	private Move moveScript;
//	private WeaponScript[] weapons;
	private Animator animator;
	private SpriteRenderer[] renderers;
	


	// Boss pattern
	public float minAttackCooldown = 0.5f;
	public float maxAttackCooldown = 2f;
	
	public float fireRate; // time between shots
	public float fireCooldown; // cooldown for shots
	public float pulseCooldown;
	public float pulseRate;
	public float bulletSpeed;
	public BulletScript bullet; // prefab of bullet being shot
	public Vector3 direction; // direction of bullets

	private float aiCooldown;
	private bool isAttacking;
	private Vector2 positionTarget;

	private int life = 35;
	
	void Awake()
	{
		// Get weapon

		// Get Movement
		moveScript = GetComponent<Move>();
		
		// Get the animator
		animator = GetComponent<Animator>();
		
	}
	
	void Start()
	{
		hasSpawn = false;
		
		// Disable everything
		// -- collider
		collider2D.enabled = false;
		// -- Moving
		moveScript.enabled = false;
		// -- Shooting
		// disable weapon
		
		// Default behavior
		isAttacking = false;
		aiCooldown = maxAttackCooldown;
	}
	
	void Update()
	{
		// Check if the enemy has spawned
		if (hasSpawn == false)
		{
			Spawn();
		}
		else
		{
			// AI
			//------------------------------------
			// Move or attack. Repeat.
			aiCooldown -= Time.deltaTime;
			
			if (aiCooldown <= 0f)
			{
				isAttacking = !isAttacking;
				aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
				positionTarget = Vector2.zero;
				
				// Set or unset the attack animation
				animator.SetBool("Attack", isAttacking);
			}
			
			// Attack
			//----------
			if (isAttacking)
			{
				// Stop any movement
				moveScript.direction = Vector2.zero;
				
				//enable and shoot weapon
				if(fireCooldown > 0) { // if cooldown is greater than zero then reduce it
					fireCooldown -= Time.deltaTime;
				}
				if(pulseCooldown > 0) { // if cooldown is greater than zero then reduce it
					pulseCooldown -= Time.deltaTime;
				}
				if(fireCooldown <= 0) { // when cooldown reaches 0 or below then fire
					
					if(life < 10) {
						pulse();
					} else {
						fire(new Vector3(-1,-1,0));
						fire(new Vector3(1,-1,0));
						fire(new Vector3(0,-1,0));
					}
					fireCooldown = fireRate; // reset the cooldown
				}
				
			}
			// Move
			//----------
			else
			{
				// Define a target?
				if (positionTarget == Vector2.zero)
				{
					// Get a point on the screen to target
					Vector2 randomPoint = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
					
					positionTarget = Camera.main.ViewportToWorldPoint(randomPoint);
				}
				
				// Are we at the target? If so, find a new one
				if (collider2D.OverlapPoint(positionTarget))
				{
					// Reset, will be set at the next frame
					positionTarget = Vector2.zero;
				}
				
				// Go to the point
				Vector3 direction = ((Vector3)positionTarget - this.transform.position);
				
				// Remember to use the move script
				moveScript.direction = Vector3.Normalize(direction);
				moveScript.move();
			}

			if(life < 0) {
				Destroy(gameObject, 2);
			}
		}
	}
	
	private void Spawn()
	{
		hasSpawn = true;
		
		// Enable everything
		// -- Collider
		collider2D.enabled = true;
		// -- Moving
		moveScript.enabled = true;
		// -- Shooting
		//enable weapon here
		
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider2D)
	{
		// If Boss is hit change animation
		BulletScript shot = otherCollider2D.gameObject.GetComponent<BulletScript>();
		if (shot != null)
		{
			if (shot.isEnemyShot == false)
			{
				// Stop attacks and start moving awya
				aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
				isAttacking = false;
				
				// Change animation
				animator.SetTrigger("Hit");
				
				if(pulseCooldown <= 0) { // when cooldown reaches 0 or below then fire
					
					pulse();
					pulseCooldown = pulseRate; // reset the cooldown
				}

				life--;
				Destroy(otherCollider2D.gameObject);
			}
		}
	}

	// fires a bullet in a given direction with a given speed
	public void fire(Vector3 bulletDirection) {
		BulletScript newBullet = Instantiate(bullet, transform.position, Quaternion.identity) as BulletScript; // create the bullet
		newBullet.motion = bulletDirection * bulletSpeed; // set the bullet's motion
		newBullet.isEnemyShot = true; // it is an Enemy shot
		newBullet.damage = 1; // set the damage
	}

	public void pulse() {
		fire(new Vector3(1,0,0));
		fire(new Vector3(1,.5f,0));
		fire(new Vector3(.5f,1,0));
		fire(new Vector3(0,1,0));
		fire(new Vector3(-.5f,1,0));
		fire(new Vector3(-1,1,0));
		fire(new Vector3(-1,.5f,0));
		fire(new Vector3(-1,0,0));
		fire(new Vector3(-1,-.5f,0));
		fire(new Vector3(-1,-1,0));
		fire(new Vector3(-.5f,-1,0));
		fire(new Vector3(0,-1,0));
		fire(new Vector3(.5f,-1,0));
		fire(new Vector3(1,-1,0));
		fire(new Vector3(1,-.5f,0));
	}
}