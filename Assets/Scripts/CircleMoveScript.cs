using UnityEngine;
using System.Collections;

public class CircleMoveScript : Move{
	public override void move() {
		transform.localPosition= new Vector3(Mathf.Cos (Time.time + phase ) * 2,Mathf.Sin (Time.time + phase) * 2,0);
	}
	
}
