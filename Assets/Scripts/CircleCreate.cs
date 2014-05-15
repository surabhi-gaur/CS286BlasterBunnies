using UnityEngine;
using System.Collections;

public class CircleCreate : MonoBehaviour {

	public GameObject enemy2;
	public int amount; //Give a value in the Inspector for how many bunnies
	public GameManagerScript gameManager;
	void Start () {
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>(); // get the current gamemanager from the scene
		for(int i = 0 ;i< amount ;i++){
			//Create object and parent it to the Spawn object
			GameObject obj = (GameObject) Instantiate (enemy2, transform.position, transform.rotation);   
			obj.transform.parent = transform;
			
			PlaceBunny (obj,i,amount);
			
			// Getting the angle between the bunnyenemy2 and the Spawn object 
			// and use it as phase shift
			EnemyScript sc = obj.GetComponent<EnemyScript>();
			float angle = Vector3.Angle(obj.transform.localPosition, transform.forward);
			Vector3 cross = Vector3.Cross(obj.transform.localPosition, transform.forward);
			if (cross.y < 0) angle = -angle;
			//sc.phase = angle * Mathf.Deg2Rad;
			sc.moveScript.phase = angle * Mathf.Deg2Rad;
		}
	}
	
	void Update () {
		if(gameManager.playing) {
			transform.Translate (0,-Time.deltaTime,0);
		}
		if(!checkOnScreen()){
			Destroy(gameObject);
		}
	}
	
	// Using the index of the newly created object from the loop
	// Divide it by the amount of object and multiply by 2PI (full circle)
	void PlaceBunny (GameObject obj, int i, int amount){
		float u = ((float)i / (float)(amount)) * (Mathf.PI*2);
		float x = (float)Mathf.Cos(u);
		float y = (float)Mathf.Sin(u);
		
		obj.transform.localPosition = new Vector3(x, 0, y)*10;
	}

	public bool checkOnScreen() {
		if(transform.position.x < -7 || transform.position.x > 7) {
			return false;
		} else if (transform.position.y < -6) {
			return false;
		} else {
			return true;
		}
	}
}
