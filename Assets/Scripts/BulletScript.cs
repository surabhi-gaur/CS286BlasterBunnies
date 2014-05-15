using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public Vector3 motion;
	public bool isEnemyShot;
	public float damage;
	public GameManagerScript gameManager; // connect to the gamemanager
	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>(); // get the current gamemanager from the scene
	}
	
	// Update is called once per frame
	void Update () {
		if( gameManager.playing){ // check to see if the game is playing
			transform.position += motion * Time.deltaTime;

			if(!checkOnScreen()) {
				Destroy(gameObject);
			}
		}
	}

	public bool checkOnScreen() {
		if(transform.position.x < -7 || transform.position.x > 7) {
			return false;
		} else if (transform.position.y > 5 || transform.position.y < -5) {
			return false;
		} else {
			return true;
		}
	}
}
