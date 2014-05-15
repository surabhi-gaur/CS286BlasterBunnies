using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public float speed;	// movement speed
	public float fireRate;	// time between shots
	public float fireCooldown; // time until next shot
	public BulletScript bullet; // bullet prefab used to shoot
	public Vector3 bulletDirection; // the direction of bullets shot
	public float life; // current lif
	public int shield; // current shield
	public ItemScript currentItem; // item held by the player but not used
	public ItemScript activeItem; // item that was held by the player and then activated
	public float itemCooldown; // cooldown for items
	private Animator animator;

	public GameManagerScript gameManager; // the scenes gamemanager

	public AllyScript[] allies = new AllyScript[3]; // array of allies

	void Awake()
	{
		// Get the animator
		animator = GetComponent<Animator>();
		
	}
	// Use this for initialization
	void Start () {
		int i = 0;
		foreach(Transform child in transform) { // for each child of the player
			allies[i] = child.GetComponent<AllyScript>(); // get the childs ally script
			allies[i].gameObject.SetActive(false); // set the ally to inactive
			i++; // next ally
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(gameManager.playing){ // if the game is playing
			
			//Movement
			float moveX = speed * Input.GetAxis("Horizontal") * Time.deltaTime; // get input from the user
			float moveY = speed * Input.GetAxis("Vertical") * Time.deltaTime;
			Vector3 moveVector = new Vector3(moveX,moveY,0); // create a move vector

			transform.Translate(moveVector); // translate the transform to the moveVector

			transform.position = new Vector3( // keep the player on screen
				Mathf.Clamp(transform.position.x, -3f, 3f),
				Mathf.Clamp(transform.position.y, -4.5f, 5f),
				0
			);

			//Shoot
			if(fireCooldown > 0) {
				fireCooldown -= Time.deltaTime; //reduce the cooldown
			}

			if(Input.GetButton("Fire1") && fireCooldown <= 0) { // if fire button pressed and cooldown is 0
				fire(bulletDirection, speed * 2); // fire a bullet
				fireCooldown = fireRate; // reset cooldown
			}

			if(Input.GetButton("Fire2")) { // if fire2 button is pressed
				useItem(); // use an item
			}

			if(Input.GetKeyDown("1")){ // jump was pressed
				fireItem(bulletDirection, speed * 2); // fire an item
			}

			if(activeItem != null) { // if there is an activeitem
				if(itemCooldown > 0) {
					itemCooldown -= Time.deltaTime; // reduce the item cooldown

				} else {
					Destroy(activeItem.gameObject); // destroy the active item when cooldown expires
					activeItem = null; //turn off animation...
//					animator.SetInteger("ShieldDown",0);//turns the shield off
				}

			}

			if( life <= 0 ) {
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if(col.gameObject.CompareTag("Bullet")) { // check for bullet collision

			if(col.GetComponent<BulletScript>().isEnemyShot) { // if the shot is from an enemy
				// Change animation
				animator.SetTrigger("Hit");
				if(shield > 0) { // if shield is up
					shield--; // reduce shield
				}else if(activeItem != null && activeItem.type == GameManagerScript.Type.Balanced) {
					life -= col.GetComponent<BulletScript>().damage / 2; //Reduction from balanced item
				}else {
					life -= col.GetComponent<BulletScript>().damage;
				}
				Destroy(col.gameObject); // destroy the bullet
			}
		} else if(col.gameObject.CompareTag("Item")) { // check for item collision
			if(currentItem == null && !col.GetComponent<ItemScript>().isMoving) { // if no current item and the item is pickupable
				currentItem = col.GetComponent<ItemScript>(); // pickup the item
				currentItem.gameObject.SetActive(false); // remove item from the scene but don't destroy it
			}
		}

	}
	// fires a bullet
	public void fire(Vector3 bulletDirection, float bulletSpeed) {
		Vector3 shotPos = new Vector3(transform.position.x, transform.position.y + .5f,0);
		BulletScript newBullet = Instantiate(bullet, shotPos, Quaternion.identity) as BulletScript;
		newBullet.motion = bulletDirection * bulletSpeed;
		newBullet.isEnemyShot = false;
		if(activeItem != null) {
			if(activeItem.type == GameManagerScript.Type.Aggressive) {
				newBullet.damage = 2;
			} else {
				newBullet.damage = 1;
			}
		}else {
			newBullet.damage = 1;
		}
	}
	// fires an item
	public void fireItem(Vector3 dir, float speed) {
		if(currentItem == null) {
			return;
		} else {
			currentItem.transform.position = transform.position;
			currentItem.gameObject.SetActive(true);

			currentItem.motion = dir * speed;
			currentItem.isMoving = true;
			currentItem = null;
		}
	}
	// use the currentItem
	public void useItem() {
		if(currentItem == null) {
			return;
		} else {
			itemCooldown = 8f;
			activeItem = currentItem;
			currentItem = null;
			switch(activeItem.type) { // turn animation on
				case GameManagerScript.Type.Aggressive:
					break;
				case GameManagerScript.Type.Defensive:
					shield = 2;
//					animator.SetTrigger("ShieldUp"); //shield animation on
					break;
				case GameManagerScript.Type.Balanced:
//					animator.SetTrigger("Heart"); //heart animation on
					break;
			}
			gameManager.increaseScore(10);
		}
	}

	public void OnGUI() {
		if(activeItem != null) {
			Texture2D texture = activeItem.GetComponent<SpriteRenderer>().sprite.texture;
			GUI.Box(new Rect(Screen.width - 30 - gameManager.sidebarWidth * 2/3, 20f, 60f, 65f), "Active");
			GUI.DrawTexture(new Rect(Screen.width - 30 - gameManager.sidebarWidth * 2/3, 40f, 40f, 40f), texture, ScaleMode.ScaleToFit, true, 0);
		}
		
		if(currentItem != null) {
			Texture2D texture = currentItem.GetComponent<SpriteRenderer>().sprite.texture;
			GUI.Box(new Rect(Screen.width - 30 - gameManager.sidebarWidth * 1/3, 20f, 60f, 65f), "Held");
			GUI.DrawTexture(new Rect(Screen.width - 30 - gameManager.sidebarWidth * 1/3, 40f, 40f, 40f), texture, ScaleMode.ScaleToFit, true, 0);
		}
	}
}
