using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	public float fireRate; // time between shots
	public float fireCooldown; // cooldown for shots
	public float bulletSpeed;
	public BulletScript bullet; // prefab of bullet being shot
	public Vector3 direction; // direction of movement and bullets
	public float life; // current life
	private GameManagerScript gameManager; // connect to the gamemanager
	public GameManagerScript.Type type; // type of enemy
	
	public bool moveStraight = true; // move straight down
	public Move moveScript;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>(); // get the current gamemanager from the scene
		fireCooldown = Random.Range(0.5f, 1f); // random starting cooldown to make different enemies shoot at different times
	}
	
	// Update is called once per frame
	void Update () {

		if( gameManager.playing){ // check to see if the game is playing
			if(fireCooldown > 0) { // if cooldown is greater than zero then reduce it
				fireCooldown -= Time.deltaTime;
			}
			if(fireCooldown <= 0) { // when cooldown reaches 0 or below then fire
				fire(direction);
				fireCooldown = fireRate; // reset the cooldown
			}
			
			// Movement
			moveScript.move();

			if(life <= 0) { // check if the enemy still has life
				gameManager.changeLoyalty(type, -1);
				gameManager.increaseScore(5);
				if((int)(Random.value * 10) == 7) {
					gameManager.dropItem(type, gameObject.transform);
				}
				Destroy(gameObject);
			}
			if(!checkOnScreen()) { // check if enemy survived
				gameManager.changeLoyalty(type, 1);
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if(col.gameObject.CompareTag("Bullet")) { // handle bullet collision detection
			if(!col.GetComponent<BulletScript>().isEnemyShot) {
				life -= col.GetComponent<BulletScript>().damage;
				Destroy(col.gameObject);
			}
		}else if(col.gameObject.CompareTag("Item")) { // handle item collision detection
			ItemScript tempItem = col.GetComponent<ItemScript>();
			if(tempItem.isMoving && tempItem.type == type) {
				gameManager.changeLoyalty(type, 1);
				Destroy(col.gameObject);
			}
		}
	}

	// fires a bullet in a given direction with a given speed
	public void fire(Vector3 bulletDirection) {
		Vector3 shotPos = new Vector3(transform.position.x, transform.position.y - .5f,0);
		BulletScript newBullet = Instantiate(bullet, shotPos, Quaternion.identity) as BulletScript; // create the bullet
		newBullet.motion = bulletDirection * bulletSpeed; // set the bullet's motion
		newBullet.isEnemyShot = true; // it is an Enemy shot
		newBullet.damage = 1; // set the damage
	}

	// check if the enemy has left the screen
	public bool checkOnScreen() {
		if (transform.position.y < -6) {
			return false;
		} else {
			return true;
		}
	}

}
