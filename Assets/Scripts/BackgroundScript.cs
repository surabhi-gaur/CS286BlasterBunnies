using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {
	public Vector3 direction;
	public float speed;
	public Vector3 start;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveVector = direction * speed * Time.deltaTime;
		transform.Translate(moveVector);

		if(transform.position.y < -28) {

			transform.position = start;
		}
	}
}
