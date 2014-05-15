using UnityEngine;
using System.Collections;

public class Move :  MonoBehaviour {
	public Vector3 direction;
	public float speed;
	public float phase;

	public virtual void move() {
		Vector3 moveVector = direction * speed * Time.deltaTime;

		transform.Translate(moveVector);
	}
}
