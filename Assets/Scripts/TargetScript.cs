using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {
	public float speed;
	public float fireRate;
	public float fireCooldown;
	public BulletScript bullet;
	public GameManagerScript gameManager;
	public Vector3 direction;
	public bool canShoot;
	public float life;
	public Move moveScript;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
	}
	
	// Update is called once per frame
	void Update () {

		if(fireCooldown > 0) {
			fireCooldown -= Time.deltaTime;
		}
		if(fireCooldown <= 0 && canShoot) {
			fire(direction, speed * 2);
			fireCooldown = fireRate;
		}

		if(life <= 0) {
			Destroy(gameObject);
		}
		moveScript.move();
	}

	void OnTriggerEnter2D (Collider2D col) {
		if(col.gameObject.CompareTag("Bullet")) {
			if(!col.GetComponent<BulletScript>().isEnemyShot) {
				life -= col.GetComponent<BulletScript>().damage;
				Destroy(col.gameObject);
			}
		}else if(col.gameObject.CompareTag("Item")) {
			if(col.GetComponent<ItemScript>().isMoving) {
				Destroy(col.gameObject);
			}
		}
	}

	public void fire(Vector3 bulletDirection, float bulletSpeed) {
		BulletScript newBullet = Instantiate(bullet, transform.position, Quaternion.identity) as BulletScript;
		newBullet.motion = bulletDirection * bulletSpeed;
		newBullet.isEnemyShot = true;
		newBullet.damage = 1;
	}

}
