using UnityEngine;
using System.Collections;

public class AllyScript : MonoBehaviour {
	public float fireRate; // time between shots
	public float fireCooldown; // cooldown for shots
	public float bulletSpeed;
	public BulletScript bulletPrefab;
	private PlayerScript player;

	// Use this for initialization
	void Start () {
		player = transform.parent.GetComponent<PlayerScript>();
		fireRate = player.fireRate;
		fireCooldown = Random.Range(0f, fireRate);
	}
	
	// Update is called once per frame
	void Update () {
		if(player.gameManager.playing) {
			if(fireCooldown > 0 ){
				fireCooldown -= Time.deltaTime;
			} else {
				fire();
				fireCooldown = fireRate;
			}
		}

		
	}

	public void fire() {
		BulletScript bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as BulletScript;
		bullet.motion = new Vector3(0,1,0) * bulletSpeed;
		bullet.isEnemyShot = false;
		bullet.damage = 1;
	}
}
