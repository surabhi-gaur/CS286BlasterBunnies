using UnityEngine;
using System.Collections;

public class SinMoveScript : Move {
	float amplitude = 2;
	float omega = 0.5f;
	float index;
	float xPos;
	Vector3 velocity;
	public bool startRight = true;
	public void Start() {
		velocity = new Vector3(0,Random.Range(0.5f,1.5f),0);
		if(!startRight) {
			amplitude *= -1;
		}
	}
	public override void move() {
		index += Time.deltaTime;
		float xPos=amplitude*(Mathf.Cos(omega*index))*Time.deltaTime;
		transform.Translate ( xPos, -velocity.y*Time.deltaTime,0);
	}
}
