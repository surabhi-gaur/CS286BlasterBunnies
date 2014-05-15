using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {
	public Vector3 motion;
	public bool isMoving;
	public Texture2D texture;

	public GameManagerScript.Type type;
	public GameManagerScript gameManager; // connect to the gamemanager
	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>(); // get the current gamemanager from the scene
		isMoving = false;
		Destroy(gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () {
		if(gameManager.playing) {
			if(isMoving) {
				transform.position += motion * Time.deltaTime;

				if(!checkOnScreen()) {
					Destroy(gameObject);
				}
			}
		}
	}

	public bool checkOnScreen() {
		if(transform.position.x < -7 || transform.position.x > 7) {
			return false;
		} else if (transform.position.y > 6 || transform.position.y < -6) {
			return false;
		} else {
			return true;
		}
	}
}
